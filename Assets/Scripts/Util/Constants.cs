using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    private static string[] loginMessages = new[]
    {
        "Iniciando sesi�n",
        "Bonito correo",
        "Bienvenido a tu viaje anat�mico",
        "�Prep�rate!",
        "Preparando la poci�n m�gica de inicio de sesi�n",
        "Accediendo al conocimiento en 3, 2, 1...",
        "�Y esta rosa?",
    };

    public static string LoginMessage
    {
        
        get {
            return loginMessages[Random.Range(0, loginMessages.Length)];
        }
    }
}
