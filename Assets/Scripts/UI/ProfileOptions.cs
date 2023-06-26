using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileOptions : MonoBehaviour
{
    [SerializeField]
    GameObject panel;
    [SerializeField]
    TextMeshProUGUI title;
    [SerializeField]
    TextMeshProUGUI role; 
    [SerializeField]
    TextMeshProUGUI message;
    [SerializeField]
    TMP_InputField inputClass;
    [SerializeField]
    TMP_InputField inputCourse;
    [SerializeField]
    Button codeButton;
    [SerializeField]
    Button showClassesButton;
    [SerializeField]
    Button logOut;
    [SerializeField]
    TextMeshProUGUI error;
    Role _role;
    public static ProfileOptions instance;
    private void Awake()
    {
        instance = this;
        panel.SetActive(false);
        panel.transform.localScale = Vector3.zero;
    }
    void Start()
    {
        error.text = string.Empty;
        logOut.onClick.AddListener(Profile.instance.LogOut);
        codeButton.onClick.AddListener(HandleButtonClick);
    }

    public void HideProfileOptions()
    {
        panel.transform.DOScale(Vector3.zero, 0.2f);
    }
    public void EnableProfileOptions()
    {
        
        title.text = Profile.instance.User.getNames();
        _role = (Role)Enum.Parse(typeof(Role), Profile.instance.User.role);
        TextMeshProUGUI classText=codeButton.GetComponentInChildren<TextMeshProUGUI>();
        switch (_role)
        {
            case Role.STUDENT:
            {
                    inputCourse.gameObject.SetActive(false);
                    showClassesButton.gameObject.SetActive(false);
                    role.text = "Estudiante";
                    message.text = "Únete a tu clase";
                    inputClass.placeholder.GetComponent<TextMeshProUGUI>().text = "Código de la clase";
                    classText.text = "Entrar";

                    break;
            }
            case Role.TEACHER:
            {
                    inputCourse.gameObject.SetActive(true);
                    showClassesButton.gameObject.SetActive(true);
                    role.text = "Docente";
                    inputClass.placeholder.GetComponent<TextMeshProUGUI>().text = "Nombre de la clase";
                    message.text = "Agrega una clase";
                    classText.text = "Agregar";
                    LoadClases();
                    showClassesButton.onClick.AddListener(ShowClases);
                    break;
            }
        }
        if (!panel.activeSelf)
        {
            panel.gameObject.SetActive(true);
        }
        panel.transform.DOScale(Vector3.one, 0.3f).SetUpdate(true);
    }

    private void ShowClases()
    {
        Classes.instance.ShowClasses(true);
    }

    private async void LoadClases()
    {
        ServiceApi serive = new();
        Models.ApiResponse<List<Models.ClassData>> response=await serive.GetClassesByUSerId(Profile.instance.User.id.ToString(), false);
        if(response.code == 200)
        {
            Profile.instance.Classes=response.data;
            Classes.instance.InstantiateClasses(response.data);
        }
    }


    public async void HandleButtonClick()
    {
        ServiceApi service = new();
        switch (_role)
        {
            case Role.STUDENT:
                {
                    try
                    {
                        var response = await service.RegisterInClass(Profile.instance.User.id.ToString(), inputClass.text);
                    }catch(Exception ex)
                    {

                    }

                    break;
                }
            case Role.TEACHER:
                {
                    try
                    {
                        var response = await service.CreateClass(Profile.instance.User.id.ToString(), inputClass.text,inputCourse.text);
                        if (response.code == 200)
                        {
                            inputClass.text = string.Empty;
                            inputCourse.text = string.Empty;
                            Models.ClassData _class=(Models.ClassData)response.data;
                            error.text = "Clase creada con éxito\nComparte este código con tus estudiantes\n" + _class.code;
                        }
                    }
                    catch (Exception ex)
                    {
                        error.text = "Se ha producido un error"+ex.Message;
                    }
                    break;
                }
        }
    }
    
}
