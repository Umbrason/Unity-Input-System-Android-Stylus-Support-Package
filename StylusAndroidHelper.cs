
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;

namespace Umbrason.AndroidStylusSupport
{
    public class StylusAndroidHelper
    {
        #region static fields
        private int _actionDown = -1;
        public int ActionDown
        {
            get
            {
                if (_actionDown == -1)
                {
                    _actionDown = motionEventClass.GetStatic<int>("ACTION_DOWN");
                }
                return _actionDown;
            }
        }

        private int _actionMove = -1;
        public int ActionMove
        {
            get
            {
                if (_actionMove == -1)
                {
                    _actionMove = motionEventClass.GetStatic<int>("ACTION_MOVE");
                }
                return _actionMove;
            }
        }

        private int _actionUp = -1;
        public int ActionUp
        {
            get
            {
                if (_actionUp == -1)
                {
                    _actionUp = motionEventClass.GetStatic<int>("ACTION_UP");
                }
                return _actionUp;
            }
        }

        private int _axisPressure = -1;
        public int AxisPressure
        {
            get
            {
                if (_axisPressure == -1)
                {
                    _axisPressure = motionEventClass.GetStatic<int>("AXIS_PRESSURE");
                }
                return _axisPressure;
            }
        }

        private int _axisTilt = -1;
        public int AxisTilt
        {
            get
            {
                if (_axisTilt == -1)
                {
                    _axisTilt = motionEventClass.GetStatic<int>("AXIS_TILT");
                }
                return _axisTilt;
            }
        }

        private int _axisX = -1;
        public int AxisX
        {
            get
            {
                if (_axisX == -1)
                {
                    _axisX = motionEventClass.GetStatic<int>("AXIS_X");
                }
                return _axisX;
            }
        }

        private int _axisY = -1;
        public int AxisY
        {
            get
            {
                if (_axisY == -1)
                {
                    _axisY = motionEventClass.GetStatic<int>("AXIS_Y");
                }
                return _axisY;
            }
        }

        private int _toolTypeStylus = -1;
        public int ToolTypeStylus
        {
            get
            {
                if (_toolTypeStylus == -1)
                {
                    _toolTypeStylus = motionEventClass.GetStatic<int>("TOOL_TYPE_STYLUS");
                }
                return _toolTypeStylus;
            }
        }
        private int _toolTypeEraser = -1;
        public int ToolTypeEraser
        {
            get
            {
                if (_toolTypeEraser == -1)
                {
                    _toolTypeEraser = motionEventClass.GetStatic<int>("TOOL_TYPE_ERASER");
                }
                return _toolTypeEraser;
            }
        }

        private int _buttonStylusPrimary = -1;
        public int ButtonStylusPrimary
        {
            get
            {
                if (_buttonStylusPrimary == -1)
                {
                    _buttonStylusPrimary = motionEventClass.GetStatic<int>("BUTTON_STYLUS_PRIMARY");
                }
                return _buttonStylusPrimary;
            }
        }

        private int _buttonStylusSecondary = -1;
        public int ButtonStylusSecondary
        {
            get
            {
                if (_buttonStylusSecondary == -1)
                {
                    _buttonStylusSecondary = motionEventClass.GetStatic<int>("BUTTON_STYLUS_SECONDARY");
                }
                return _buttonStylusSecondary;
            }
        }
        #endregion
        // Create an instance of the AndroidJavaObject class to represent the current Android activity.
        // This will be used to access the input events from the stylus.
        private AndroidJavaObject motionEvents;

        // Create an instance of the AndroidJavaClass class to represent the Java MotionEvent class.
        // This will be used to access the input events from the stylus.
        private AndroidJavaClass motionEventClass;


        // Override the Update() method of the InputDevice class to access the input events from the stylus.
        public void QueryInput(ref StylusData state)
        {
            // Check if the current activity and MotionEvent class instances have been initialized.
            if (motionEvents == null || motionEventClass == null)
            {
                // If not, try to initialize them.
                InitializeAndroidObjects();
            }
            // Check if the current activity and MotionEvent class instances have been successfully initialized.
            if (motionEvents != null && motionEventClass != null)
            {
                // If so, get the current list of input events from the stylus using the AndroidJavaObject and AndroidJavaClass instances.
                // This will typically involve calling a Java method to get the list of input events and then iterating over the events
                // to process them individually.
                var inputEvents = GetInputEventsFromStylus();

                // Iterate over the input events and process them.
                foreach (var inputEvent in inputEvents)
                {
                    // Get the type of the input event.
                    var eventType = inputEvent.Call<int>("getActionMasked");
                    var pointerCount = inputEvent.Call<int>("getPointerCount");
                    for (int index = 0; index < pointerCount; index++)
                    {
                        var toolType = inputEvent.Call<int>("getToolType", index);
                        if (!(toolType == ToolTypeEraser || toolType == ToolTypeStylus))
                            continue;
                        ProcessInputEvent(eventType, index, toolType, inputEvent, ref state);
                    }
                }
            }
        }

        // A helper method to initialize the AndroidJavaObject and AndroidJavaClass instances used to access the stylus input events.
        private void InitializeAndroidObjects()
        {
            // Use the AndroidJNIHelper class to get the current Android activity.
            var unityPlayerClass = new AndroidJavaClass("com.umbrason.androidstylussupport.StylusSupportActivity");
            motionEvents = unityPlayerClass.GetStatic<AndroidJavaObject>("motionEvents");

            // Use the AndroidJavaClass class to get a reference to the Java MotionEvent class.                
            motionEventClass = new AndroidJavaClass("android.view.MotionEvent");
        }

        // A helper method to get the input events from the stylus.
        private AndroidJavaObject[] GetInputEventsFromStylus()
        {
            // Call a Java method on the current activity to get the list of input events from the stylus.
            // You may need to specify the appropriate Java method and parameters based on how your Android
            // app is set up to receive input events from the stylus.       
            var events = motionEvents.Call<AndroidJavaObject[]>("toArray");
            motionEvents.Call("clear");
            return events;
        }

        // A helper method to process an input event from the stylus.
        private void ProcessInputEvent(int eventType, int pointerIndex, int toolType, AndroidJavaObject inputEvent, ref StylusData state)
        {
            // Check if the input event is a button press event.                

            var pressure = inputEvent.Call<float>("getPressure", pointerIndex);
            var tilt = inputEvent.Call<float>("getAxisValue", AxisTilt, pointerIndex);
            var rotation = inputEvent.Call<float>("getOrientation", pointerIndex);
            var buttons = inputEvent.Call<int>("getButtonState");
            var positionX = inputEvent.Call<float>("getAxisValue", AxisX, pointerIndex);
            var positionY = inputEvent.Call<float>("getAxisValue", AxisY, pointerIndex);
            state.pressure = pressure;
            state.tilt = tilt;
            state.rotation = rotation;
            state.position = new(positionX, Screen.height - positionY); //inverted Y axis

            var pressed = eventType == ActionDown || eventType == ActionMove;
            var tip = pressed && toolType == ToolTypeStylus ? 1 : 0;
            var eraser = pressed && toolType == ToolTypeEraser ? 2 : 0;

            var primaryButton = (buttons & ButtonStylusPrimary) != 0 ? 4 : 0;
            var secondaryButton = (buttons & ButtonStylusSecondary) != 0 ? 8 : 0;

            state.buttons = (ushort)(tip | eraser | primaryButton | secondaryButton);

        }
    }
}