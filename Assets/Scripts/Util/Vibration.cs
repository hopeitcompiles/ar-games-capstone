using System;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    public static Vibration instance;
    float duration;
    bool canVibrate;
    bool vibrateDisabled;

    public bool VibrateDisabled
    {
        set { vibrateDisabled = value; }
    }
    void Awake()
    {
        duration = 0.5f;
        instance = this;
        canVibrate = IsVibrationSupported();
    }


    bool IsVibrationSupported()
    {
        try
        {
            AndroidJavaObject vibratorService = new AndroidJavaClass("android.os.Vibrator").CallStatic<AndroidJavaObject>("getService", "vibrator");
            return vibratorService.Call<bool>("hasVibrator");
        }catch(Exception e)
        {
            return false;
        }
        
    }

    public void Vibrate()
    {
        if (canVibrate && !vibrateDisabled)
        {
            Handheld.Vibrate();
            Invoke(nameof(StopVibration), duration);
        }
    }

    void StopVibration()
    {
        // stop vibration
        Handheld.Vibrate();
    }
}
