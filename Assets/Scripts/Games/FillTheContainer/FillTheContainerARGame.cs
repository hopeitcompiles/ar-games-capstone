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
    readonly int layer=9;
    public override void EndGame()
    {
        ResultsManager.Instance.Description = "Este juego aún no tiene estadísticas asignadas";
        ResultsManager.Instance.Activate(true);
    }

    public override void StartGame()
    {
        foreach (ContainerPiece piece in pieces)
        {
            piece.MakeItDrop(false);
        }
        TimerManager.Instance.StartTimer(timeLimit, false);
    }

    protected override void Start()
    {
        base.Start();
        ContainerPiece[] _pieces = new ContainerPiece[0];
        foreach (GameObject item in systems)
        {
            var system=Instantiate(item, transform);
            _pieces=_pieces.Concat(system.GetComponentsInChildren<ContainerPiece>()).ToArray();
            system.SetActive(false);
        }
        List<ContainerPiece> temp = new();
        Vector3 offset = new(0f, 0f, -0.5f); // Ajusta los valores para el desplazamiento deseado
        foreach (ContainerPiece piece in _pieces)
        {
            piece.SetUp(layer);
            piece.IsCorrect = true;
            piece.MakeItDrop(true);
            var pi = Instantiate(piece, transform.position + offset, transform.rotation);
            pi.transform.localScale *= 5f;
            pi.gameObject.SetActive(true);
            temp.Add(pi);
        }
        pieces=_pieces.ToArray();
    }
    private void OnDisable()
    {
        foreach(ContainerPiece piece in pieces)
        {
            piece.gameObject.SetActive(false);
        }
    }
}
