using DG.Tweening;
using System;
using System.Collections.Generic;
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

        string _classes = PlayerPrefs.GetString("clases");
        Debug.Log(_classes);
        if (_classes != null && _classes != "")
        {
            try
            {
                ClassWrapper classes = JsonUtility.FromJson<ClassWrapper>(_classes);
                Profile.instance.Classes=classes.classes;
                Classes.instance.InstantiateClasses(classes.classes);
                HandleShowClassForStudent(classes.classes);   
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }
    private void HandleShowClassForStudent(List<Models.ClassData> classes)
    {
        if (Profile.instance.User.role == Role.STUDENT.ToString() && classes.Count > 0)
        {
            inputClass.gameObject.SetActive(false);
            message.text = "Estás en la clase " + classes[0].className + "\n" + classes[0].grade;
            error.text = "Código: "+ classes[0].code;
            codeButton.gameObject.SetActive(false);
        }
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
                    LoadClases();
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
            PlayerPrefs.DeleteKey("clases");

            ClassWrapper _c = new()
            {
                classes = response.data
            };

            string _classes=JsonUtility.ToJson(_c);

            Debug.Log(_classes);
            PlayerPrefs.SetString("clases", _classes);
            HandleShowClassForStudent(response.data);
        }
    }
    private void ClearError()
    {
        error.text = "";
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
                        if (response.code == 200)
                        {
                            error.text = "Te has registrado correctamente";
                            Invoke("ClearError", 2);
                            LoadClases();
                        }
                    }
                    catch(Exception ex)
                    {
                        error.text = "Algo no ha salido bien";
                    }

                    break;
                }
            case Role.TEACHER:
                {
                    try
                    {
                        var response = await service.CreateClass(Profile.instance.User.id.ToString(), inputClass.text,inputCourse.text);
                        Debug.Log(response.message);
                        if (response.code == 200)
                        {
                            inputClass.text = string.Empty;
                            inputCourse.text = string.Empty;
                            Models.ClassData _class=(Models.ClassData)response.data;
                            error.text = "Clase creada con éxito\nComparte este código con tus estudiantes\n" + _class.code;
                            LoadClases();
                        }
                        else
                        {
                            throw new Exception(response.message);
                        }
                    }
                    catch (Exception ex)
                    {
                        error.text =ex.Message.Contains("already in class")? "No puedes formar parte de otra clase":"Se ha producido un error"+ex.Message;
                    }
                    break;
                }
        }
    }

    public class ClassWrapper
    {
        public List<Models.ClassData> classes;
    }

}
