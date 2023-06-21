using DG.Tweening;
using Newtonsoft.Json;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    [SerializeField]
    TMP_Text titleText;
    [SerializeField]
    TMP_InputField _username;
    [SerializeField]
    TMP_InputField _password;
    [SerializeField]
    TMP_InputField _name;
    [SerializeField]
    TMP_InputField _lastName;
    [SerializeField]
    TMP_InputField _age;
    [SerializeField]
    Button loginButton;
    [SerializeField]
    Button registerButton;
    [SerializeField]
    TextMeshProUGUI errorText;
    [SerializeField]
    GameObject loginPanel;


    ServiceApi service;
    Models.ProfileData user;

    bool registering;

    public event Action LoggedIn;
    public event Action LoggedOut;

    Transform registerPanel;


    public static Profile instance;
    public Models.ProfileData User
    {
        get { return user; }
    }
    private void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        _name.gameObject.SetActive(false);
        _lastName.gameObject.SetActive(false);
        _age.gameObject.SetActive(false);  
        RoleSelector.instance.gameObject.SetActive(false);
        loginButton.onClick.AddListener(HandleLogin);
        registerButton.onClick.AddListener(HandleRegister);
        service = new();
        LoggedIn += Profile_LoggedIn;
        LoggedOut += Profile_LoggedOut;
        string information = PlayerPrefs.GetString("UserData");
        Debug.Log(information);
        try
        {
            user = JsonConvert.DeserializeObject<Models.ProfileData>(information);
            LoggedIn.Invoke();
        }
        catch (Exception ex)
        {
            user = null;
        }
        registering = false;
        registerPanel = registerButton.transform.parent;
        errorText.text = "";
        _username.onValueChanged.AddListener(ClearError);
        _password.onValueChanged.AddListener(ClearError);
    }


    private void Profile_LoggedOut()
    {
        user = null;
        HideLoginForm(false);
        PlayerPrefs.DeleteKey("UserData");
        ProfileOptions.instance.HideProfileOptions();
        loginPanel.transform.DOScale(Vector3.one, 0.6f).SetEase(Ease.InOutBounce);
    }

    private void Profile_LoggedIn()
    {
        _username.text = "";
        _password.text = "";
        _name.text = "";
        _lastName.text = "";
        _age.text = "";
        string information = JsonConvert.SerializeObject(user);
        PlayerPrefs.SetString("UserData", information);
        ProfileOptions.instance.EnableProfileOptions();
        loginPanel.transform.localScale = Vector3.zero;
    }

    private void HideLoginForm(bool state)
    {
        Vector3 scale = !state ? Vector3.one : Vector3.zero;
        if(state)
        {
            _username.transform.DOScale(scale, 0.3f);
            _username.gameObject.SetActive(!state);
            _password.transform.DOScale(scale, 0.3f);
            _password.gameObject.SetActive(!state);
        }
        else
        {
            _username.gameObject.SetActive(!state);
            _username.transform.DOScale(scale, 0.3f);
            _password.gameObject.SetActive(!state); 
            _password.transform.DOScale(scale, 0.3f);
        }
        registerButton.transform.parent.DOScale(scale, 0.3f);
        errorText.text = "";
    }
    private void HideRegisterForm(bool state)
    {
        Vector3 scale = !state ? Vector3.one : Vector3.zero;
        //_username.transform.DOScale(scale, 0.3f);
        //_password.transform.DOScale(scale, 0.3f);
        _name.gameObject.SetActive(!state);
        _name.transform.DOScale(scale, 0.3f);
        _lastName.gameObject.SetActive(!state);
        _lastName.transform.DOScale(scale, 0.3f);
        _age.gameObject.SetActive(!state);
        _age.transform.DOScale(scale, 0.3f);
        RoleSelector.instance.gameObject.SetActive(!state);
        RoleSelector.instance.transform.DOScale(scale, 0.3f);
        errorText.text = "";
    }
    public void HandleRegister()
    {
        registering = !registering;
        registerPanel.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text 
            = registering? "�Ya tienes una cuenta?":"�A�n no tienes una cuenta?";
        registerButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = registering ? "Iniciar Sesi�n" : "Crea una Cuenta";
        loginButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = registering ? "Registrarse" : "Iniciar Sesi�n";
        HideRegisterForm(!registering);
        
        
    }
    public void ClearError(string spam)
    {
        if (errorText.text == string.Empty)
        {
            return;
        }
        errorText.text = string.Empty;
    }
    private async void HandleLogin()
    {
        if (_username.text == "")
        {
            errorText.text = "Ingrese el correo";
            return;
        }
        if (_password.text == "")
        {
            errorText.text = "Ingrese la contrase�a";
            return;
        }
        ClearError("");
        Models.ApiResponse<Models.ProfileData> response;
        if (registering)
        {
            int age = int.TryParse(_age.text, out age) ? age : 0;
   
                Models.ApiResponse<string> _response = await service.RegisterRequest(_username.text, _password.text,_name.text,_lastName.text, age, Role.STUDENT);
            
            if (_response.code==200)
            {
                response = await service.LoginRequest(_username.text, _password.text);
                if (response.code == 200)
                {
                    user = (Models.ProfileData) response.data;
                    LoggedIn.Invoke();
                }
                else
                {
                    errorText.text = response.message;
                }
            }
            return;
        }
        response = await service.LoginRequest(_username.text, _password.text);
        if(response.code==200)
        {

            user = (Models.ProfileData) response.data;
            LoggedIn.Invoke();
        }
        else
        {
            errorText.text = response.message=="Incorrect email or password"?"Correo o contrase�a incorrecta": response.message;
        }
    }
   public void LogOut()
    {
        LoggedOut.Invoke();
    }
}