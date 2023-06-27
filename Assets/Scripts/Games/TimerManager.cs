using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiTimer;
    [SerializeField] private AudioClip lowTimeTicSound;
    public static event Action OnRunOutTime;
    private float time;
    private float timeLimit;
    private bool increasing;
    private float timeStamp;


    private IEnumerator timerCorroutine;
    private static TimerManager instance;

    public static TimerManager Instance
    {
        get
        {
            return instance;
        }
    }

    public float TimeStamp
    {
        set { timeStamp = value; }
        get { return timeStamp; }   
    }
    void Awake()
    {
        timeStamp = 0.01f;
        if (instance == null)
        {
            instance = this;
        }
    }

    public void StartTimer(float limitTime, bool increasing)
    {
        time = 0;
        this.timeLimit = limitTime;
        this.increasing = increasing;
        uiTimer.color = Color.white;
        //timerCorroutine = Timer(1);
        CancelInvoke("TimerInvoke");
        InvokeRepeating("TimerInvoke", 0, timeStamp);

        // StartCoroutine(timerCorroutine);
    }
    private void TimerInvoke()
    {
        if (timeLimit >= time) { 
            uiTimer.text = ((increasing ? time : timeLimit - time)).ToString("0");
            if (time >= timeLimit - 3)
            {
                float intensity = time - timeLimit;
                AudioManager.Instance.PlayOnShot(lowTimeTicSound);
                uiTimer.color = new Color(255, 38 * (intensity), 38 * (intensity), 1);
            }
            
            if (time >= timeLimit-0.01f)
            {
                Debug.Log("Time run out");
                //AudioManager.Instance.PlayOnShot(timeEnded);
                OnRunOutTime?.Invoke();
                CancelInvoke("TimerInvoke");
                
            }
            time += timeStamp;
        }
    }
    //IEnumerator Timer(float waitTimeseconds)
    //{
    //    for (; ; )
    //    {
    //        time += waitTimeseconds;
    //        uiTimer.text = "" +( (increasing ? time : timeLimit - time)+1);
    //        if (time > timeLimit - 3)
    //        {
    //            float intensity = time -timeLimit;
    //            AudioManager.Instance.PlayOnShot(lowTimeTicSound);
    //            uiTimer.color = new Color(255, 38*(intensity), 38 * (intensity), 1);
    //        }
    //        if (time >= timeLimit)
    //        {
    //            Debug.Log("End corroutine");
    //            StopCoroutine(timerCorroutine);
    //            OnRunOutTime?.Invoke();
    //        }
    //        yield return new WaitForSeconds(waitTimeseconds);
    //    }
    //}
    public float StopTimer()
    {
        if(timerCorroutine != null)
        {
            StopCoroutine(timerCorroutine);
        }
        CancelInvoke("TimerInvoke");
        return time;
    }
}
