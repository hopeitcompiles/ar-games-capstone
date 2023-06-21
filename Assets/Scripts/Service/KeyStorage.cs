using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyStore : MonoBehaviour
{
    private const string ANDROID_KEYSTORE_CLASS = "com.example.keystorelibrary.KeyStoreUtils";
    private const string KEY_ALIAS = "my_key_alias";

    private AndroidJavaObject keystoreUtils;

    void Start()
    {
        // Crea una instancia de la clase KeystoreUtils
        keystoreUtils = new AndroidJavaObject(ANDROID_KEYSTORE_CLASS);
    }

    public void SaveToKeyStore(string data)
    {
        // Guarda los datos en el Keystore
        keystoreUtils.Call("saveToKeyStore", KEY_ALIAS, data);
    }

    public string RetrieveFromKeyStore()
    {
        // Recupera los datos del Keystore
        return keystoreUtils.Call<string>("retrieveFromKeyStore", KEY_ALIAS);
    }
}

