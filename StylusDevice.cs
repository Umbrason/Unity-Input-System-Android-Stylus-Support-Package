using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

[InputControlLayout(displayName = "Stylus", stateType = typeof(StylusData))]
#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
#endif
public class StylusDevice : InputDevice, IInputUpdateCallbackReceiver
{
    private static StylusAndroidHelper helper = new();

    public ButtonControl buttonTip { get; private set; }
    public ButtonControl buttonEraser { get; private set; }
    public ButtonControl buttonPrimary { get; private set; }
    public ButtonControl buttonSecondary { get; private set; }
    public AxisControl positionX { get; private set; }
    public AxisControl positionY { get; private set; }
    public AxisControl pressure { get; private set; }
    public AxisControl tilt { get; private set; }
    public AxisControl rotation { get; private set; }

    protected override void FinishSetup()
    {
        base.FinishSetup();
        buttonTip = GetChildControl<ButtonControl>("tip");
        buttonEraser = GetChildControl<ButtonControl>("eraser");
        buttonPrimary = GetChildControl<ButtonControl>("primary");
        buttonSecondary = GetChildControl<ButtonControl>("secondary");
        positionX = GetChildControl<AxisControl>("positionX");
        positionY = GetChildControl<AxisControl>("positionY");
        pressure = GetChildControl<AxisControl>("pressure");
        tilt = GetChildControl<AxisControl>("tilt");
        rotation = GetChildControl<AxisControl>("rotation");
    }

    public void OnUpdate()
    {
        var state = new StylusData();
#if !UNITY_EDITOR
        helper.QueryInput(ref state);
#endif
        InputSystem.QueueStateEvent(this, state);
    }
    #region Layout Registration / API Setup
    public static Action<string> deviceAdded;
    public static Action<string> deviceRemoved;
    static StylusDevice()
    {
        InputSystem.RegisterLayout<StylusDevice>();
        InputSystem.AddDevice<StylusDevice>();
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeInPlayer() { }
    #endregion
}

public struct StylusData : IInputStateTypeInfo
{
    public FourCC format => new FourCC('S', 'T', 'Y', 'L');
    [InputControl(name = "tip",         layout = "Button", bit = 0)]
    [InputControl(name = "eraser",      layout = "Button", bit = 1)]
    [InputControl(name = "primary",     layout = "Button", bit = 2)]
    [InputControl(name = "secondary",   layout = "Button", bit = 3)]
    public ushort buttons;

    [InputControl(layout = "Axis")]
    public float positionX;

    [InputControl(layout = "Axis")]
    public float positionY;

    [InputControl(layout = "Axis")]
    public float pressure;

    [InputControl(layout = "Axis")]
    public float tilt;

    [InputControl(layout = "Axis")]
    public float rotation;
}

