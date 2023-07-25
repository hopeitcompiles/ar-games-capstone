using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MicroGame : ScriptableObject
{
    public string title;
    public Sprite image;
    [TextArea] public string description;
    public List<SystemData> models;

    [System.Serializable]
    public class SystemData
    {
        public AnatomicSystem system;
        public GameObject gameObject;
    }
}
