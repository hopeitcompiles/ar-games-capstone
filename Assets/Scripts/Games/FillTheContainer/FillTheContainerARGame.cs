using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FillTheContainerARGame : ARGame
{
    [SerializeField]
    private GameObject[] systems;
    [SerializeField]
    private int correctModel;
    private ContainerPiece[] pieces;

    public override void EndGame()
    {
        throw new System.NotImplementedException();
    }

    public override void StartGame()
    {
        foreach (ContainerPiece piece in pieces)
        {
            piece.MakeItDrop(false);
        }
        TimerManager.Instance.StartTimer(timeLimit, false);
    }

    void Start()
    {
        base.Start();
        ContainerPiece[] _pieces = new ContainerPiece[0];
        foreach (GameObject item in systems)
        {
            var system=Instantiate(item, transform);
            _pieces=_pieces.Concat(system.GetComponentsInChildren<ContainerPiece>()).ToArray();
            system.SetActive(false);
            Destroy(item,5);
        }
        List<ContainerPiece> temp = new();
        Vector3 offset = new Vector3(0f, 0f, -0.5f); // Ajusta los valores para el desplazamiento deseado
        foreach (ContainerPiece piece in _pieces)
        {
            piece.IsCorrect = true;
            piece.MakeItDrop(true);
            var pi = Instantiate(piece, transform.position + offset, transform.rotation);
            pi.transform.localScale *= 5f;
            pi.gameObject.SetActive(true);
            temp.Add(pi);
        }
        pieces=_pieces.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
