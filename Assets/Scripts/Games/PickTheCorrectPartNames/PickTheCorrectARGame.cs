using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickTheCorrectARGame : ARGame
{
    [SerializeField] TextMeshProUGUI title;
    PickeablePiece[] pieces;
    protected override void Start()
    {
        float[] values= { 15f, 10f, 5f };
        timeByDificult = values;
        base.Start();
        pieces =GetComponentsInChildren<PickeablePiece>();

    }
   

    public override void StartGame()
    {
        TimerManager.Instance.StartTimer(timeLimit, false);
        Debug.Log("pick the correct has started");
    }
    public override void EndGame()
    {
        
        Debug.Log("pick the correct has ended");
    }
}
