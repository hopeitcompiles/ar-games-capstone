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

    int points = 0;
    ImpostorPiece[] impostors;

    ImpostorPiece impostor1;
    ImpostorPiece impostor2;
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
    void Start()
    {
        points = 0;
        base.Start();
        impostors = GetComponentsInChildren<ImpostorPiece>();

        System.Random random = new();
        int[] array = Enumerable.Range(0, impostors.Length).ToArray();
        int[] mix=new int[array.Length];

        int thresshold=random.Next(1,mix.Length-1);

        for (int i = 0; i < mix.Length; i++)
        {
            int index = (i + thresshold) >= mix.Length ? i + thresshold -mix.Length : i + thresshold;
            mix[i] = array[index];
        }


        foreach (int value in mix)
        {
            Debug.Log(value);
        }
        for (int i=0; i<impostors.Length; i++) {
            impostors[i].Id = i;
            impostors[i].LayerMask = gameLayer;
            
        } 
        for (int i=0; i<impostors.Length; i++) {
            if (impostors[i].Faked == null)
            {
                impostors[i].Faked = impostors[mix[i]];
                impostors[mix[i]].Faked = impostors[i];
            }
            impostors[i].ChangePosition(impostors[i].Faked.OriginalPosition);

        }
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
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 
                LayerMask.GetMask(LayerMask.LayerToName(gameLayer))))
            {
                Debug.Log("touched");
                if(impostor1 == null)
                {
                    impostor1 = hit.collider.gameObject.GetComponent<ImpostorPiece>();
                    impostor1.Touchable.MakeItGlow(true);
                    return;
                }
                else {
                    ImpostorPiece impostorPiece = hit.collider.gameObject.GetComponent<ImpostorPiece>();
                    if (impostorPiece.Id != impostor1.Id)
                    {
                        impostor2 = impostorPiece;
                        impostor1.Touchable.MakeItGlow(false);
                        impostor2.Touchable.MakeItGlow(false);
                        if (impostor1.Id == impostor2.Faked.Id)
                        {
                            MakePoint(true);
                            impostor1.gameObject.SetActive(false);
                            impostor2.gameObject.SetActive(false);
                        }
                        else
                        {
                            MakePoint(false);
                        }
                        impostor1 = null;
                        impostor2 = null;
                    }
                }
            }
        }
    }
}
