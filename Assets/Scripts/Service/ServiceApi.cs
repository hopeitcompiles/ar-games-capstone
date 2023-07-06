
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


public class ServiceApi : MonoBehaviour
{
    private static string _baseUrl;
    private static readonly HttpClient httpClient = new();

    public ServiceApi()
    {
        _baseUrl = "https://arappbackend.onrender.com";
    }

    public async Task<Models.ApiResponse<Models.ProfileData>> LoginRequest(string username, string password)
    {

        string url = _baseUrl + "/post-login";
        Dictionary<string, string> fields = new()
        {
            { "email", username},
            { "password", password }
        };
        return await PostRequest<Models.ProfileData>(fields, url, Constants.LoginMessage);
    }
    public async Task<Models.ApiResponse<string>> RegisterRequest(string username, string password, string name, string lastName, int age, Role role)
    {
        string url = _baseUrl + "/post-createuser";
        Dictionary<string, string> fields = new()
        {
            { "Email", username},
            { "Password", password },
            { "Firstname", name },
            { "Lastname", lastName },
            { "Age", age.ToString() },
            { "Role",role.ToString() }
        };
        return await PostRequest<string>(fields, url);
    }

    public async Task<Models.ApiResponse<string>> RegisterInClass(string userId, string classCode)
    {
        string url = _baseUrl + "/post-adduserinclass";
        Dictionary<string, string> fields = new()
        {
            { "userId", userId},
            { "classCode", classCode }
        };
        return await PostRequest<string> (fields, url);
    }
    public async Task<Models.ApiResponse<Models.ClassData>> CreateClass(string userId, string className,string course)
    {
        string url=_baseUrl + "/post-createclass";
        Dictionary<string, string> fields = new()
        {
            { "UserId", userId},
            { "ClassName", className },
            { "Grade", course }
        };
        return await PostRequest<Models.ClassData>(fields, url);
    }

    public async Task<Models.ApiResponse<List<Models.ClassData>>> GetClassesByUSerId(string userId, bool loading)
    {
        string url = _baseUrl + "/get-getclassesofuser/"+userId;
        return await GetRequest<List<Models.ClassData>>(url,loading);
    }

    public async Task<Models.ApiResponse<string>> SendGameStats(Models.GameMetric stats, string loadingMessage="Enviando...")
    {
        string url = _baseUrl + "/post-creategamemetric";
        
        Dictionary<string, string> fields = new()
        {
            { "GameId", stats.gameId.ToString()},
            { "UserId", Profile.instance.User.id.ToString()},
            { "ClassId", Profile.instance.Classes[0].id.ToString() },
            { "Score", stats.score.ToString() },
            { "TimeElapsed", stats.timeElapsed.ToString() },
            { "IsGameCompleted", (stats.percentageOfCompletion>=100).ToString() },
            { "PercentageOfCompletion", stats.percentageOfCompletion.ToString() },
            { "SuccessCount", stats.successCount.ToString() },
            { "FailureCount", stats.failureCount.ToString() },
            { "Difficulty", stats.difficulty.ToString() },
            { "Comments", stats.comments.ToString() },
        };
        return await PostRequest<string>(fields, url,loadingMessage);
    }



    public async Task<Models.ApiResponse<T>> PostRequest<T>(Dictionary<string, string> fields, string url, string loadingMessage = "Cargando")
    {
        Models.ApiResponse<T> response;

        Loading.instance.SetLoading(true, loadingMessage);

        WWWForm form = new();

        foreach (var kvp in fields)
        {
            form.AddField(kvp.Key, kvp.Value);
        }

        UnityWebRequest request = UnityWebRequest.Post(url, form);

        // Envía la solicitud al servidor
        var asyncOperation = request.SendWebRequest();

        while (!asyncOperation.isDone)
        {
            await Task.Delay(100);
        }

        try
        {
            var json_response = request.downloadHandler.text;
            if (request.result == UnityWebRequest.Result.Success)
            {
                response = JsonUtility.FromJson<Models.ApiResponse<T>>(json_response);
                Debug.Log("Done en la solicitud: " + json_response);
            }
            else
            {
                throw new Exception(json_response);

            }
        }
        catch (Exception e)
        {
           
            response = new Models.ApiResponse<T>()
            {
                code = 503,
                message = e.Message
            };
        }

        Loading.instance.SetLoading(false);
        return response;
    }

    public async Task<Models.ApiResponse<T>> GetRequest<T>(string url, bool loading, string loadingMessage = "Cargando")
    {
            Models.ApiResponse<T> response;

            Loading.instance.SetLoading(true, loadingMessage);

            try
            {
                HttpResponseMessage httpResponse = await httpClient.GetAsync(url);
                string jsonResponse = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    response = JsonUtility.FromJson<Models.ApiResponse<T>>(jsonResponse);
                }
                else
                {
                    throw new Exception(jsonResponse);
                }
            }
            catch (Exception e)
            {
                response = new Models.ApiResponse<T>()
                {
                    code = 503,
                    message = e.Message
                };
            }

            Loading.instance.SetLoading(false);
            return response;
        }
 
    public async Task<FactCat> GetCatFact()
    {
        string url = "https://catfact.ninja/fact";
        FactCat response = new();
        
        Loading.instance.SetLoading(true, "Veo que tienes buen gusto");

        try
        {
            HttpResponseMessage httpResponse = await httpClient.GetAsync(url);
            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();

            if (httpResponse.IsSuccessStatusCode)
            {
                response = JsonUtility.FromJson<FactCat>(jsonResponse);
            }
            else
            {
                throw new Exception(jsonResponse);
            }
        }
        catch (Exception e)
        {
            response = new FactCat()
            {
                fact = e.Message,
                length =0
            };
        }

        Loading.instance.SetLoading(false);

        return response;
    }

    [Serializable]
    public class FactCat
    {
        public string fact;
        public int length;
    }
  
}
