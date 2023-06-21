using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metric
{
    public float TimeElapsed { get; set; }
    public bool IsGameCompleted { get; set; }
    public float PercentageOfCompletion { get; set; }
    public int Score { get; set; }
    public DificultLevel Difficulty { get; set; }
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public string Comments { get; set; }
}
