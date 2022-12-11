package com.umbrason.androidstylussupport;
import com.unity3d.player.UnityPlayerActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.MotionEvent;
import java.util.ArrayList;

public class StylusSupportActivity extends UnityPlayerActivity {

    public static ArrayList<MotionEvent> motionEvents = new ArrayList<MotionEvent>();

    public void OnResume()
    {
        System.out.print("test");
    }

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