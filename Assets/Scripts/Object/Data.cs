using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    [SerializeField] private string partName;
    [SerializeField] private DataStructure[] easyFacts;
    [SerializeField] private DataStructure[] mediumFacts;
    [SerializeField] private DataStructure[] hardFacts;
    private List<int> easyUsed=new();
    private List<int> mediumUsed = new();
    private List<int> hardUsed = new();
    public string PartName
    {
        get { return partName!=""?partName:gameObject.name; }
    }
    private void Start()
    {
        Guid uuid = Guid.NewGuid();
        foreach (DataStructure fact in easyFacts)
        {
            fact.Id=uuid.ToString();
        }
        foreach (DataStructure fact in mediumFacts)
        {
            fact.Id=uuid.ToString();
        }
        foreach (DataStructure fact in hardFacts)
        {
            fact.Id=uuid.ToString();
        }
    }
    public string GetRamdonFact(DificultLevel level)
    {
        System.Random random = new();

        switch (level)
        {
            case DificultLevel.EASY:
                {
                    if (easyFacts.Length == 0)
                    {
                        return "No hay easy facts asignados a esta parte";
                    }
                    if (easyUsed.Count == easyFacts.Length)
                    {
                        easyUsed=new List<int>();
                    }
                    int index = random.Next(0, easyFacts.Length - 1);

                    while (easyUsed.Contains(index))
                    {
                        index = random.Next(0, easyFacts.Length - 1);
                    }
                    return easyFacts[index].Fact;
                }
            case DificultLevel.MEDIUM:
                {
                    if (mediumFacts.Length == 0)
                    {
                        return "No hay medium facts asignados a esta parte";
                    }
                    if (mediumUsed.Count == mediumFacts.Length)
                    {
                        mediumUsed = new List<int>();
                    }
                    int index = random.Next(0, mediumFacts.Length - 1);

                    while (mediumUsed.Contains(index))
                    {
                        index = random.Next(0, mediumFacts.Length - 1);
                    }
                    return mediumFacts[index].Fact;
                }
            case DificultLevel.HARD:
                {
                    if (hardFacts.Length == 0)
                    {
                        return "No hay hard facts asignados a esta parte";
                    }
                    if (hardUsed.Count == hardFacts.Length)
                    {
                        hardUsed = new List<int>();
                    }
                    int index = random.Next(0, hardFacts.Length - 1);
                    while (hardUsed.Contains(index))
                    {
                        index = random.Next(0, hardFacts.Length - 1);
                    }
                    return hardFacts[index].Fact;
                }
            default:
                {
                    return "Algo no ha salido bien";
                }
        }
    }
}


[Serializable]
public class DataStructure
{
    private string id;
    [SerializeField] [TextArea] private string fact;
    public string Id
    {
        get { return id; }
        set { id = value; }
    }
    public string Fact
    {
            get { return fact; }
    }
}