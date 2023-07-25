using System;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    [SerializeField] private string partName;
    [SerializeField] private ANATOMIC_SYSTEM system;

    [SerializeField] private Facts facts;

    public string PartName
    {
        get { return partName!=""?partName:gameObject.name; }
    }
    public ANATOMIC_SYSTEM System
    {
        get { return system; }
    }
  
    public string GetRamdonFact(DificultLevel level)
    {
        System.Random random = new();

        switch (level)
        {
            case DificultLevel.EASY:
                {
                    if (facts.easy.Count == 0)
                    {
                        return "No hay easy facts asignados a esta parte";
                    }
                    int index = random.Next(0, facts.easy.Count - 1);
                    return facts.easy[index];
                }
            case DificultLevel.MEDIUM:
                {
                    if (facts.medium.Count == 0)
                    {
                        return "No hay medium facts asignados a esta parte";
                    }
                    int index = random.Next(0, facts.medium.Count - 1);
                    return facts.easy[index];
                }
            case DificultLevel.HARD:
                {
                    if (facts.hard.Count == 0)
                    {
                        return "No hay hard facts asignados a esta parte";
                    }
                    int index = random.Next(0, facts.hard.Count - 1);
                    return facts.easy[index];
                }
            default:
                {
                    return "Algo no ha salido bien";
                }
        }
    }
}