using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Facts : ScriptableObject
{
    [TextArea] public List<string> easy;
    [TextArea] public List<string> medium;
    [TextArea] public List<string> hard;

}
