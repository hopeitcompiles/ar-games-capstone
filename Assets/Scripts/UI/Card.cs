using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private List<MicroGame.SystemData> systems;
    private GameObject model;
    private string title;
    private string description;
    private Sprite image;
    
    public GameObject Model { set { model = value; } }
    public List<MicroGame.SystemData> Systems { set { systems = value; } }
    public string Title { set { title = value; } }
    public string Description { set { description = value; } }
    public Sprite Image { set { image = value; } }

    void Start()
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
        if (transform.GetChild(1).childCount>1)
        {
            transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = image;
            transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
        }
        else
        {
            transform.GetChild(1).GetComponent<Image>().sprite = image;
        }
        var button = GetComponent<Button>();
        button.onClick.AddListener(StartAR);
    }
    public void StartAR()
    {
        ContentLister.instance.StartAR(systems,model,title);
    }
}
