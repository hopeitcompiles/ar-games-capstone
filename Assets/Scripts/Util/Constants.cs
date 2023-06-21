using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    private static string[] loginMessages = new[]
    {
        "Iniciando sesión",
        "Bonito correo",
        "Bienvenido a tu viaje anatómico",
        "¡Prepárate!",
        "Preparando la poción mágica de inicio de sesión",
        "Accediendo al conocimiento en 3, 2, 1...",
        "¿Y esta rosa?",
    };

    public static string LoginMessage
    {
        
        get {
            return loginMessages[Random.Range(0, loginMessages.Length)];
        }
    }
}
