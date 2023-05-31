using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnatomicSystem : ScriptableObject
{
    public string title;
    public Sprite image;
    [TextArea] public string description;
    public GameObject model;
}
