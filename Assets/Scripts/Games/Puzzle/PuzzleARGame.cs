using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleARGame : ARGame
{
    PuzzlePiece[] pieces;
    readonly int layer = 8;
    PuzzlePiece selectedPiece;

    public override void EndGame()
    {
        metric.isGameCompleted = false;
        metric.timeElapsed = TimerManager.Instance.StopTimer();
        ResultsManager.Instance.Activate(true, Result.OK, metric);

    }

    public override void StartGame()
    {
        Debug.Log("Started Puzzle Game");
        TimerManager.Instance.StartTimer(timeLimit, false);
    }

    protected override void Start()
    {
        base.Start();
        pieces = GetComponentsInChildren<PuzzlePiece>();
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].SetUp(layer);
            pieces[i].gameObject.AddComponent<Dragable>();
        }

    }
    private void Update()
    {
        selectedPiece=(PuzzlePiece)ARinteractionManager.Instance.Manage3DModelDrag<PuzzlePiece>();
    }
        //if (Input.touchCount == 0)
        //{
        //    return;
        //}
        //Touch touch = Input.GetTouch(0);
        //if(Input.touchCount == 1)
        //{
        //    switch (touch.phase)
        //    {
        //        case TouchPhase.Began:
        //            {
        //                initialPosition=touch.position;
        //                isSelected = CheckTouchOnARObject(initialPosition);
        //                break;
        //            }
        //            case TouchPhase.Moved:
        //            {
        //                if(isSelected)
        //                {
        //                    float multiplier = Configuration.instance.DragSpeed;
        //                    Vector2 diffPosition=(touch.position - initialPosition)*screenFactor;
        //                    selectedPiece.transform.position=selectedPiece.transform.position + 
        //                        new Vector3(diffPosition.x*speedMovement* multiplier, diffPosition.y*speedMovement* multiplier, 0);
        //                    initialPosition=touch.position;
        //                }
        //                break;
        //            }
        //    }
        //}
        //switch (touch.phase)
        //{
        //    case TouchPhase.Began:
        //        // Inicia el arrastre al tocar la pantalla
        //        Ray ray = _camera.ScreenPointToRay(touch.position);
        //        RaycastHit hit;

        //        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        //        {
                    
        //            if (hit.collider.TryGetComponent<PuzzlePiece>(out var puzzlePiece))
        //            {
        //                isDragging = true;
        //                selectedPiece = puzzlePiece;
        //            }
        //        }
        //        break;


        //    case TouchPhase.Moved:
        //        // Mueve el objeto mientras arrastras el dedo
        //        if (isDragging && selectedPiece != null)
        //        {
        //            Vector3 touchPosition = _camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, _camera.nearClipPlane));
        //            selectedPiece.transform.position = touchPosition;
        //        }
        //        break;

        //    case TouchPhase.Ended:
        //        // Finaliza el arrastre al soltar el dedo
        //        isDragging = false;
        //        selectedPiece = null;
        //        break;
        //}

    //void FixedUpdate()
    //{
    //    if (Input.touchCount == 0)
    //    {
    //        return;
    //    }
    //    Touch touch = Input.touches[0];
    //    Vector3 position = touch.position;

    //    if (touch.phase == TouchPhase.Began)
    //    {
    //        // Verifica si el toque inicial colisiona con la pieza
    //        RaycastHit hit;
    //        Ray ray = _camera.ScreenPointToRay(position);
    //        if (Physics.Raycast(ray, out hit, Mathf.Infinity,
    //            LayerMask.GetMask(LayerMask.LayerToName(gameLayer))))
    //        {
    //            selectedPiece=hit.collider.gameObject.GetComponent<PuzzlePiece>();
    //            if (selectedPiece != null)
    //            {
    //                touchID = touch.fingerId;
    //                offset = transform.position - GetTouchWorldPosition(touch.position);
    //                selectedPiece.Touchable.MakeItGlow(true);
    //                Debug.Log("tocado" + hit.collider.gameObject.name);
    //            }

    //        }
    //        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
    //        {
    //            // Verifica si el toque actual pertenece al ID del toque inicial
    //            if (touch.fingerId == touchID)
    //            {
    //                if(selectedPiece!=null)
    //                {
    //                    selectedPiece.gameObject.transform.position = GetTouchWorldPosition(touch.position) + offset;

    //                }
    //                // Actualiza la posición de la pieza según el movimiento del toque
    //            }
    //        }
    //        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
    //        {
    //            // Reinicia el ID del toque cuando se levanta el dedo de la pantalla
    //            if (touch.fingerId == touchID)
    //            {
    //                touchID = -1;
    //                if(selectedPiece!=null)
    //                {
    //                    selectedPiece.Touchable.MakeItGlow(true);
    //                }
    //                selectedPiece = null;   
    //            }
    //        }
    //    }

    //}
    //private Vector3 GetTouchWorldPosition(Vector2 touchPosition)
    //{
    //    // Obtiene la posición del toque en el mundo en coordenadas 3D
    //    Vector3 touchPosition3D = touchPosition;
    //    touchPosition3D.z = _camera.nearClipPlane;
    //    return _camera.ScreenToWorldPoint(touchPosition3D);
    //}
}
