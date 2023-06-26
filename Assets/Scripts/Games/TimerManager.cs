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

    private IEnumerator timerCorroutine;
    private static TimerManager instance;

    public static TimerManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
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
        timerCorroutine = Timer(1);
        CancelInvoke("TimerInvoke");
        InvokeRepeating("TimerInvoke", 0, 1);

        // StartCoroutine(timerCorroutine);
    }
    private void TimerInvoke()
    {
        if (timeLimit >= time) { 
            uiTimer.text = "" + ((increasing ? time : timeLimit - time));
            if (time >= timeLimit - 3)
            {
                float intensity = time - timeLimit;
                AudioManager.Instance.PlayOnShot(lowTimeTicSound);
                uiTimer.color = new Color(255, 38 * (intensity), 38 * (intensity), 1);
            }
            if (time >= timeLimit)
            {
                //AudioManager.Instance.PlayOnShot(timeEnded);
                CancelInvoke("TimerInvoke");
                OnRunOutTime?.Invoke();
            }
            time += 1;

        }
    }
    IEnumerator Timer(float waitTimeseconds)
    {
        for (; ; )
        {
            time += waitTimeseconds;
            uiTimer.text = "" +( (increasing ? time : timeLimit - time)+1);
            if (time > timeLimit - 3)
            {
                float intensity = time -timeLimit;
                AudioManager.Instance.PlayOnShot(lowTimeTicSound);
                uiTimer.color = new Color(255, 38*(intensity), 38 * (intensity), 1);
            }
            if (time >= timeLimit)
            {
                Debug.Log("End corroutine");
                StopCoroutine(timerCorroutine);
                OnRunOutTime?.Invoke();
            }
            yield return new WaitForSeconds(waitTimeseconds);
        }
    }
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
