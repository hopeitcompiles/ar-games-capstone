using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameStatsSender : MonoBehaviour
{
    public static GameStatsSender instance;
    void Awake()
    {
        instance = this;
    }

    public async Task<string> SendStats(Models.GameMetric metric)
    {
        ServiceApi service = new();
        Models.ApiResponse<string> response=await service.SendGameStats(metric);
        if(response.code == 200)
        {
            return "Tus resultados han sido enviados";
        }
        return true?response.message:"No se ha podido enviar tus resultados";
    }
}
