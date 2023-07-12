using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConexionChecker : MonoBehaviour
{
    public static ConexionChecker Instance;
    public bool HasInternet {
        get { return HasInternetConnection(); }
    }
    void Awake()
    {
        Instance = this;
    }

    private bool HasInternetConnection()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
