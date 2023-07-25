using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public static  SystemManager instance;
    private static ANATOMIC_SYSTEM activeSystem=ANATOMIC_SYSTEM.OTHER;
    void Awake()
    {
        instance = this;
    }
    public ANATOMIC_SYSTEM ActiveSystem {
        get { return activeSystem; }
        set { activeSystem = value; }
    }

}
public enum ANATOMIC_SYSTEM
{
    DIGESTIVE,
    RESPIRATORY,
    OTHER
}