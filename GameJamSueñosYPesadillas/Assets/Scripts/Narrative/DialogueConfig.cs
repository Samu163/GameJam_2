using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[CreateAssetMenu(fileName = "NewDialogueConfig", menuName = "Configs/Dialogue")]

public class DialogueConfig : ScriptableObject
{
    public string[] lines;
    public bool hasDecision;
    public string nameChar;
    public Sprite characterImg;
    public Sprite backgroundScene;
    public DecisionConfig decision;
    public bool changeScenario;
    public bool endsDay;

}
