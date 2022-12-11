package com.umbrason.umbrapixel;
import com.unity3d.player.UnityPlayerActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.MotionEvent;
import java.util.ArrayList;

public class PenSupportActivity extends UnityPlayerActivity {

    public static ArrayList<MotionEvent> motionEvents = new ArrayList<MotionEvent>();

    public boolean dispatchTouchEvent(MotionEvent ev)
    {
        motionEvents.add(ev);
        return super.dispatchTouchEvent(ev);
    }

    public boolean dispatchGenericMotionEvent(MotionEvent ev)
    {
        motionEvents.add(ev);
        return super.dispatchGenericMotionEvent(ev);
    }
}