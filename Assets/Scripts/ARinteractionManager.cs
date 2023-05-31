using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARinteractionManager : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new();
    private bool isInitialPosition;

    private GameObject aRPointer;
    private GameObject itemModel;
   

    public GameObject ItemModel
    {
        set
        {
            itemModel = value;
            itemModel.transform.position = aRPointer.transform.position;
            itemModel.transform.parent = aRPointer.transform;
            isInitialPosition = true;
        }
    }
    void Start()
    {
        aRPointer = transform.GetChild(0).gameObject;
        arRaycastManager = FindAnyObjectByType<ARRaycastManager>();
        GameManager.Instance.OnMainMenu += SetItemPosition;
        GameManager.Instance.OnARPosition += OnArPosition;
        GameManager.Instance.OnGame += Instance_OnGame;
        aRPointer.SetActive(true);
    }

    private void Instance_OnGame()
    {
        aRPointer.GetComponent<Renderer>().enabled = false;

    }

    private void OnArPosition()
    {
        aRPointer.GetComponent<Renderer>().enabled = true;
        aRPointer?.SetActive(true);
    }

    private void SetItemPosition()
    {
        if (aRPointer != null)
        {
            aRPointer.SetActive(false);
            itemModel = null;
        }
    }

    public void OnDestroy()
    {
        itemModel.SetActive(false);
        aRPointer?.SetActive(false);
        GameManager.Instance.MainMenu();
    }
    // Update is called once per frame
    void Update()
    {
        if (isInitialPosition)
        {
            Vector2 middlePointScreen = new(Screen.width / 2, Screen.height / 2);
            arRaycastManager.Raycast(middlePointScreen, hits, TrackableType.Planes);
            if (hits.Count > 0)
            {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;
                aRPointer.SetActive(true);
                isInitialPosition = false;
            }
        }
    }
}
