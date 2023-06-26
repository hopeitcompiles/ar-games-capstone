using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Header : MonoBehaviour
{
    [SerializeField]
    TMP_Text username;
    void Awake()
    {
        username.text = "";
        Profile.instance.LoggedIn += Instance_LoggedIn;
        Profile.instance.LoggedOut += Instance_LoggedOut;
    }

    private void Instance_LoggedOut()
    {
        username.text = "";
    }

    private void Instance_LoggedIn()
    {
        username.text = Profile.instance.User.firstname;
        Debug.Log("Changed name");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
