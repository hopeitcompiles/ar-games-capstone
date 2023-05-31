using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SystemPiece : MonoBehaviour
{
    [SerializeField]
    private string pieceName;

    [TextArea]
    [SerializeField]
    private string description;

    [TextArea]
    [SerializeField]
    private List<string> easyFacts;

    [TextArea]
    [SerializeField]
    private List<string> mediumFacts;
    
    [TextArea]
    [SerializeField]
    private List<string> hardFacts;


    public string Name
    {
        get { return pieceName; }
    } 
    public string Description
    {
        get { return description; }
    } 
    public List<string> Facts
    {
        get { return easyFacts.Concat(mediumFacts).Concat(hardFacts).ToList(); }
    } 

}
