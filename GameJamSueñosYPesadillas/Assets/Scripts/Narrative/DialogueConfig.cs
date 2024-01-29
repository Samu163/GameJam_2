using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewDialogueConfig", menuName = "Configs/Dialogue")]

public class DialogueConfig : ScriptableObject
{
    public string[] lines;
    public bool hasDecision;
    public string nameChar;
    public Sprite characterImg;
    public DecisionConfig decision;
    public bool changeScenario;
    public bool endsDay;

}
