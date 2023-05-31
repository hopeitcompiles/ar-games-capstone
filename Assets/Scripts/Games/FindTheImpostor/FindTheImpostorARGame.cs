using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using UnityEngine;

public class FindTheImpostorARGame : ARGame
{
    [SerializeField]
    TextMeshProUGUI UIPoints; 
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    GameObject impostorsModel;

    int points = 0;


    int gameLayer=6;
    public override void EndGame()
    {
        throw new System.NotImplementedException();
    }

    public override void StartGame()
    {
        Debug.Log("game has started"); 
        TimerManager.Instance.StartTimer(timeLimit, false);
        canvas.SetActive(true);
        canvas.transform.DOScale(Vector3.one, .3f);

    }

    // Start is called before the first frame update
    protected override void Start()
    {
        points = 0;
        base.Start();
        canvas.transform.localScale = Vector3.zero;
        canvas.SetActive(false);
    }
    private void MakePoint(bool marked)
    {
        points += marked ? 1 : -1;
        if (UIPoints != null)
        {
            UIPoints.text=points.ToString();
        }
    }
    void FixedUpdate()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        Touch touch = Input.touches[0];
        Vector3 position = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("touched");
                
                Touchable touchable= hit.collider.gameObject.GetComponent<Touchable>();
                if (touchable != null)
                {
                    touchable.MakeItGlow(true);
                }
                ImpostorPiece impostorPiece = hit.collider.gameObject.GetComponent<ImpostorPiece>();
                if (impostorPiece != null)
                {
                    MakePoint(true);
                    impostorPiece.gameObject.SetActive(false);
                }
                else
                {
                    MakePoint(false);
                }
                
            }
        }
    }
}
