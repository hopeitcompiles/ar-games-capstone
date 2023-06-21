using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassItem : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI className;    
    [SerializeField] 
    private TextMeshProUGUI classCode;
    [SerializeField]
    private TextMeshProUGUI classCourse;
    [SerializeField]
    private Image classIcon; 
    
    public string ClassName
    {
        set { className.text = value; }

    }
    public string ClassCode
    {
        set { classCode.text = value; }
    }
    public string ClassCourse
    {
        set { classCourse.text = value; }
    }
    public Sprite ClassIcon
    {
        set { classIcon.sprite = value; }
    }
}
