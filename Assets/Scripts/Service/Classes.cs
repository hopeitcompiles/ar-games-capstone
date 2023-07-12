using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Classes : MonoBehaviour
{
    [SerializeField]
    private GameObject container;

    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private ClassItem classPrefab;
    [SerializeField]
    private TextMeshProUGUI subtitle;

    public static Classes instance;
    void Awake()
    {
        instance = this;
        if(panel.activeSelf)
        {
            panel.transform.localScale = Vector3.zero;
        }
    }

    public void ShowClasses(bool show)
    {
        if(show && !panel.activeSelf)
        {
            panel.SetActive(show);
        }
        Vector3 scale = show ? Vector3.one : Vector3.zero;
        panel.transform.DOScale(scale,0.3f).SetEase(show?Ease.InOutBounce:Ease.Linear);
    }

    public void InstantiateClasses(List<Models.ClassData> classes)
    {
        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }
        if (classes.Count == 0)
        {
            subtitle.text = "Aún no has registrado ninguna clase";
            return;
        }
        subtitle.text = "Comparte el código de la clase con tus estudiantes";
        foreach (Models.ClassData obj in classes)
        {
            ClassItem newItem;
            newItem = Instantiate(classPrefab, container.transform);
            newItem.ClassName=obj.className;
            newItem.ClassCode=obj.code;
            newItem.ClassCourse=obj.grade;
        }
    }
}
