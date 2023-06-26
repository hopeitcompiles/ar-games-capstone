using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleSelector : MonoBehaviour
{
    [SerializeField]
    GameObject container;
    [SerializeField]
    Button studentButton;
    [SerializeField]
    Button teacherButton;
    Image studentCheck;
    Image teachertCheck;
    Role role;

    public Role Role
    {
        get { return role; }
    }
    public static RoleSelector instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if(container.activeSelf)
        {
            container.SetActive(false);
        }
        studentCheck =studentButton.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
        teachertCheck = teacherButton.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
        role = Role.STUDENT;
        teacherButton.onClick.AddListener(HandleRoleChangedToTeacher);
        studentButton.onClick.AddListener(HandleRoleChangedToStudent);
        teachertCheck.transform.localScale = Vector3.zero;
    }
    public void HandleRoleChangedToTeacher()
    {

        if(role == Role.STUDENT)
        {
            role = Role.TEACHER;
            teachertCheck.transform.DOScale(Vector3.one,0.3f);
            studentCheck.transform.DOScale(Vector3.zero, 0.3f);
        }
        
    }
    void HandleRoleChangedToStudent()
    {

        if(role == Role.TEACHER)
        {
            role = Role.STUDENT;
            teachertCheck.transform.DOScale(Vector3.zero, 0.3f);
            studentCheck.transform.DOScale(Vector3.one, 0.3f);
        }
       
    }
}
