using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARinteractionManager : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new();
    private bool isInitialPosition;
    private bool isOverUI;
    private bool isOver3DModel;
    private GameObject aRPointer;
    private GameObject itemModel;

    private Vector2 initialTouchPosition;
    bool isPlacing;
    public static ARinteractionManager Instance;
    public Camera Camera { get { return arCamera; } }

    private Vector2 modelInitialPosition;
    private IPiece selectedPiece;
    private bool isSelected;

    private float screenFactor = 0.001f;
    private float speedMovement = 2f;
    private bool isAxisX;
    public bool IsAxisX
    {
        get { return isAxisX; }
        set { isAxisX = value; }
    }
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
        Instance = this;    
        aRPointer = transform.GetChild(0).gameObject;
        arRaycastManager = FindAnyObjectByType<ARRaycastManager>();
        
        GameManager.Instance.OnMainMenu += SetItemPosition;
        GameManager.Instance.OnARPosition += OnArPosition;
        GameManager.Instance.OnGame += Instance_OnGame;
        aRPointer.SetActive(true);
    }

    private void Instance_OnGame()
    {
        isPlacing = false;
        aRPointer.GetComponent<BoxCollider>().enabled = false;
        itemModel.transform.parent = null;
        aRPointer.SetActive(false);
        arRaycastManager.SetTrackablesActive(false);
    }

    private void OnArPosition()
    {
        arRaycastManager.SetTrackablesActive(true);
        isPlacing = true;
        aRPointer?.SetActive(true);
        aRPointer.GetComponent<BoxCollider>().enabled = true;
    }

    private void SetItemPosition()
    {
        if (aRPointer != null)
        {
            aRPointer.SetActive(false);
        }

    }

    public void OnDestroy()
    {
        if (itemModel!=null) { 
            Destroy(itemModel.gameObject);
            itemModel = null;
        } 
        if (aRPointer != null)
        {
            aRPointer?.SetActive(false);
        }
        GameManager.Instance.MainMenu();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isPlacing)
        {
            return;
        }
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
        if (Input.touchCount > 0)
        {
            Touch touchOne = Input.GetTouch(0);
            if (touchOne.phase == TouchPhase.Began)
            {
                var touchPosition = touchOne.position;
                isOverUI = IsTapOverUI(touchPosition);
                isOver3DModel = IsTapOver3DModel(touchPosition);
            }

            if (touchOne.phase != TouchPhase.Moved)
            {
                if (arRaycastManager.Raycast(touchOne.position, hits, TrackableType.Planes))
                {
                    Pose pose = hits[0].pose;
                    if (!isOverUI && isOver3DModel)
                    {
                        transform.position = pose.position;
                    }
                }
            }
            if (Input.touchCount == 2)
            {
                Touch touchTwo=Input.GetTouch(1);
                if(touchOne.phase==TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
                {
                    initialTouchPosition = touchTwo.position-touchOne.position;
                }
                if(touchOne.phase==TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)
                {
                    Vector2 currentTouchPosition= touchTwo.position - touchOne.position;
                    float angle=Vector2.SignedAngle(initialTouchPosition, currentTouchPosition);
                    itemModel.transform.rotation = Quaternion.Euler(0, itemModel.transform.eulerAngles.y +angle, 0);
                    initialTouchPosition= currentTouchPosition;
                }
            }
        }
    }

    private bool IsTapOverUI(Vector2 touchPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touchPosition.x, touchPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    private bool IsTapOver3DModel(Vector2 touchPosition)
    {
        Ray ray=arCamera.ScreenPointToRay(touchPosition);
        if(Physics.Raycast(ray,out  RaycastHit hit))
        {
            if (hit.collider.CompareTag("ar")){
                return true;
            }
        }
        return false;
    }

    public IPiece Manage3DModelDrag<T>()
    {
        if (Input.touchCount ==0)
        {
            return null;
        }
        Touch touch = Input.GetTouch(0);
        if (Input.touchCount == 1)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    {
                        modelInitialPosition = touch.position;
                        isSelected = CheckTouchOnARObject<T>(modelInitialPosition);
                        if (isSelected)
                        {
                            AudioManager.Instance.SelectPlay(true);
                        }
                        break;
                    }
                case TouchPhase.Moved:
                    {
                        if (isSelected)
                        {
                            float multiplier = Configuration.instance.DragSpeed;
                            Vector2 diffPosition = (touch.position - modelInitialPosition) * screenFactor;
                            selectedPiece.Transform().position = selectedPiece.Transform().position +
                                new Vector3(diffPosition.x * speedMovement * multiplier*(!isAxisX?0:1), 
                                diffPosition.y * speedMovement * multiplier, 
                                diffPosition.x * speedMovement * multiplier*(!isAxisX?1:0));
                            modelInitialPosition = touch.position;
                        }
                        break;
                    }
                case TouchPhase.Ended:
                    {
                        if (selectedPiece != null)
                        {
                            selectedPiece.GameObject().GetComponent<Touchable>().MakeItGlow(false);
                            selectedPiece = null;
                        }
                        isSelected= false;
                        break;
                    }
            }
        }
        return selectedPiece;
    }

    private bool CheckTouchOnARObject<T>(Vector2 position)
    {
        Ray ray = arCamera.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag(Dragable.dragTag))
            {
                selectedPiece = (IPiece)hit.transform.gameObject.GetComponent<T>();
                selectedPiece.GameObject().GetComponent<Touchable>().MakeItGlow(true);
                return true;
            }
        }
        return false;
    }
}
