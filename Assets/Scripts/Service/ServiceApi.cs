
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


public class ServiceApi : MonoBehaviour
{
    private static string _baseUrl;

    public ServiceApi()
    {
        _baseUrl = "https://arappbackend.azurewebsites.net/";
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
            { "userId", userId},
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
            if (request.result == UnityWebRequest.Result.Success)
            {
                var json_response = request.downloadHandler.text;
                response = JsonConvert.DeserializeObject<Models.ApiResponse<T>>(json_response);

                Debug.Log("Done en la solicitud: " + json_response);
            }
            else
            {
                var json_response = request.downloadHandler.text;
                response = JsonConvert.DeserializeObject<Models.ApiResponse<T>>(json_response);

            }
        }
        catch (HttpRequestException e)
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


    private byte[] GenerateFormBytes(Dictionary<string, string> fields, string boundary)
    {
        List<byte[]> formDataParts = new();

        foreach (var field in fields)
        {
            string formDataPart = "--" + boundary + "\r\n" +
                                  "Content-Disposition: form-data; name=\"" + field.Key + "\"\r\n\r\n" +
                                  field.Value + "\r\n";

            byte[] formDataPartBytes = System.Text.Encoding.UTF8.GetBytes(formDataPart);
            formDataParts.Add(formDataPartBytes);
        }

        string finalBoundary = "--" + boundary + "--\r\n";
        byte[] finalBoundaryBytes = System.Text.Encoding.UTF8.GetBytes(finalBoundary);
        formDataParts.Add(finalBoundaryBytes);

        byte[] formDataBytes = formDataParts.SelectMany(x => x).ToArray();
        return formDataBytes;
    }

    public async Task<Models.ApiResponse<T>> PostRequest3<T>(Dictionary<string, string> fields, string url, string loadingMessage="Cargando")
    {
        Models.ApiResponse<T> response;

        Loading.instance.SetLoading(true, loadingMessage);

        
        try
        {
            HttpClient client = new()
            {
                Timeout = TimeSpan.FromSeconds(15)
            };

            HttpResponseMessage httpResponse;

            var formContent = new FormUrlEncodedContent(fields);
            httpResponse = await client.PostAsync(url, formContent);

            // Obtiene la respuesta del servidor
            string responseContent = await httpResponse.Content.ReadAsStringAsync();

            // Convierte la respuesta a objeto ApiResponse<T>
            response = JsonConvert.DeserializeObject<Models.ApiResponse<T>>(responseContent);

            // Realiza cualquier procesamiento adicional con la respuesta
        }
        catch (HttpRequestException e)
        {
            // Maneja cualquier excepción de solicitud HTTP
            response = new Models.ApiResponse<T>()
            {
                code = 503,
                message = e.Message
            };
        }

        Loading.instance.SetLoading(false);
        return response;
    }

        public async Task<Models.ApiResponse<T>> PostRequest2<T>(Dictionary<string, string> fields,string url, string message="Cargando")
    {
        Models.ApiResponse<T> response;

        string boundary = "-----------------------" + DateTime.Now.Ticks.ToString("x");

        // Genera los bytes del formulario
        byte[] formDataBytes = GenerateFormBytes(fields, boundary);

        // Crea una solicitud POST con los datos de inicio de sesión
        UnityWebRequest request = UnityWebRequest.Post(url, "");

        request.SetRequestHeader("Content-Type", "multipart/form-data; boundary=" + boundary);
        //request.SetRequestHeader("Content-Type", "multipart/form-data);
        request.uploadHandler = new UploadHandlerRaw(formDataBytes);
        Loading.instance.SetLoading(true,message);

        try
        {
            var asyncOperation = request.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Delay(100);
            }
            if (request.result == UnityWebRequest.Result.Success)
            {
                var json_response = request.downloadHandler.text;
                response = JsonConvert.DeserializeObject<Models.ApiResponse<T>>(json_response);

                Debug.Log("Done en la solicitud: " + json_response);
            }
            else
            {
                var json_response = request.downloadHandler.text;
                response = JsonConvert.DeserializeObject<Models.ApiResponse<T>>(json_response);
                response.message = json_response;
            }
        }
        catch (Exception e)
        {
            response = new()
            {
                code = 500,
                message = e.Message
            };
        }
        Loading.instance.SetLoading(false);

        return response;
    }

    public async Task<Models.ApiResponse<T>> GetRequest<T>(string url, bool loading, string loadingMessage = "Cargando")
    {
        Models.ApiResponse<T> response=new();
        UnityWebRequest request = UnityWebRequest.Get(url);
        if (loading)
        {
            Loading.instance.SetLoading(true, loadingMessage);
        }
        try
        {
            var asyncOperation = request.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Delay(100);
            }
            var json_response = request.downloadHandler.text;
            response = JsonConvert.DeserializeObject<Models.ApiResponse<T>>(json_response);
        }
        catch (Exception e)
        {
            response.message = e.Message;
        }
        if (loading)
        {
            Loading.instance.SetLoading(false, loadingMessage);
        }
        return response;
    }

    public async Task<FactCat> GetCatFact()
    {
        string url = "https://catfact.ninja/fact";
        FactCat response = new();
        UnityWebRequest request = UnityWebRequest.Get(url);
     
        
        Loading.instance.SetLoading(true, "Veo que tienes buen gusto");
        
        try
        {
            var asyncOperation = request.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Delay(100);
            }
            var json_response = request.downloadHandler.text;
            response = JsonConvert.DeserializeObject<FactCat>(json_response);
        }
        catch (Exception e)
        {
            response.fact = e.Message;
        }
            Loading.instance.SetLoading(false);
        
        return response;
    }

    public class FactCat
    {
        public string fact { get; set; }
        public int length { get; set; }
    }
  
}
