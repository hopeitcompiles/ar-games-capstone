using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class FillTheContainerARGame : ARGame
{
    [SerializeField]
    private GameObject correctSystem;
    [SerializeField]
    private GameObject wrongSystem;

    private List<ContainerPiece> correctPieces=new();
    private List<ContainerPiece> wrongPieces = new();


    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new();
    private bool isOverUI;
    private bool isOver3DModel;
    private ContainerPiece selectedPiece;

    readonly float raycastDownwardDistance = 1f;
    readonly float distanceFromContainer = 0.0005f;
    protected override void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        raycastManager.SetTrackablesActive(true);
        base.Start();
    }
    

    public override void StartGame()
    {
        List<ContainerPiece> _correctPieces=correctSystem.GetComponentsInChildren<ContainerPiece>().ToList();
        List<ContainerPiece> _wrongPieces = wrongSystem.GetComponentsInChildren<ContainerPiece>().ToList();

        Vector3 containerPosition = base.Model.transform.position;
        Quaternion containerRotation = base.Model.transform.rotation;
        float angleStep = 360f / _correctPieces.Count;

        for (int i = 0; i < _correctPieces.Count; i++)
        {
            // Calcular la posición relativa del elemento en base al ángulo
            Quaternion elementRotation = containerRotation * Quaternion.Euler(0f, angleStep * i, 0f);
            Vector3 elementPosition = containerPosition + (elementRotation * Vector3.forward * distanceFromContainer);

            // Buscar una posición válida utilizando raycasting
            if (FindValidPosition(ref elementPosition))
            {
                ContainerPiece element = Instantiate(_correctPieces[i], elementPosition, elementRotation);
                element.SetUp(1);
                correctPieces.Add(element);
            }
        }
        TimerManager.Instance.StartTimer(timeLimit, false);
        hasStarted = true;
    }
    private bool FindValidPosition(ref Vector3 position)
    {
        return true;
        // Lanzar un rayo desde la posición hacia abajo para detectar el plano
        Ray ray = new Ray(position + Vector3.up * raycastDownwardDistance, Vector3.down);
        if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
        {
            // Verificar si el rayo intersecta con un plano
            position = hits[0].pose.position; // Actualizar la posición con la posición del raycast hit
            return true;
        }
        return false;
    }
    public override void EndGame()
    {
        ResultsManager.Instance.Activate(true, Result.OK, metric);
    }
    private void Update()
    {
        if (!hasStarted)
        {
            return;
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
                //if (raycastManager.Raycast(touchOne.position, hits, TrackableType.Planes))
                //{
                    Pose pose = hits[0].pose;
                    if (!isOverUI && isOver3DModel)
                    {
                        if(selectedPiece != null)
                        {
                            selectedPiece.transform.position = pose.position;
                        }
                    }
                //}
            }
            if(touchOne.phase == TouchPhase.Ended)
            {
                DeselectPiece();
            }
        }
    }
    public void SelectPiece(ContainerPiece piece)
    {
        // Deseleccionar la pieza anteriormente seleccionada, si hay alguna
        DeselectPiece();

        // Seleccionar la nueva pieza
        selectedPiece = piece;
        selectedPiece.GetComponent<Touchable>().MakeItGlow(true);
    }

    public void DeselectPiece()
    {
        // Deseleccionar la pieza actual
        if (selectedPiece != null)
        {
            selectedPiece.GetComponent<Touchable>().MakeItGlow(false);
            selectedPiece = null;
        }
    }
    private bool IsTapOverUI(Vector2 touchPosition)
    {
        PointerEventData eventData = new(EventSystem.current)
        {
            position = new Vector2(touchPosition.x, touchPosition.y)
        };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    private bool IsTapOver3DModel(Vector2 touchPosition)
    {
        Ray ray = ARinteractionManager.Instance.Camera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out ContainerPiece piece))
            {
                if (piece != null)
                {
                    SelectPiece(piece);

                    return true;
                }
            }
        }
        DeselectPiece();
        return false;
    }
}
