using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleARGame : ARGame
{
    PuzzlePiece[] pieces;
    PuzzlePiece selectedPiece;
    private int touchID = -1;
    private Vector3 offset;
    int gameLayer = 8;
    private bool isDragging = false;
    private Vector2 dragStartPosition;

    public override void EndGame()
    {
        throw new System.NotImplementedException();
    }

    public override void StartGame()
    {
        Debug.Log("Started Puzzle Game");
        foreach (PuzzlePiece piece in pieces)
        {
            piece.MakeItDrop(false);
        }
        TimerManager.Instance.StartTimer(timeLimit, false);

    }

    protected override void Start()
    {
        base.Start();
        pieces = GetComponentsInChildren<PuzzlePiece>();
        foreach (PuzzlePiece piece in pieces)
        {
            piece.MakeItDrop(true);
            piece.LayerMask = gameLayer;
        }
       
    }
    private void FixedUpdate()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        Touch touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                // Inicia el arrastre al tocar la pantalla
                Ray ray = _camera.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    PuzzlePiece puzzlePiece = hit.collider.GetComponent<PuzzlePiece>();

                    if (puzzlePiece != null)
                    {
                        isDragging = true;
                        selectedPiece = puzzlePiece;
                        dragStartPosition = touch.position;
                    }
                }
                break;


            case TouchPhase.Moved:
                // Mueve el objeto mientras arrastras el dedo
                if (isDragging && selectedPiece != null)
                {
                    Vector3 touchPosition = _camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, _camera.nearClipPlane));
                    selectedPiece.transform.position = touchPosition;
                }
                break;

            case TouchPhase.Ended:
                // Finaliza el arrastre al soltar el dedo
                isDragging = false;
                selectedPiece = null;
                break;
        }
    }

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
