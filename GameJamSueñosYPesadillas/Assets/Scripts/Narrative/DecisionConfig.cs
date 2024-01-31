using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[CreateAssetMenu(fileName = "NewDecisionConfig", menuName = "Configs/Decision")]


public class DecisionConfig : ScriptableObject
{
    public string headDisplay;
    public string[] decisions;
    //Importante, las decisiones y el texto tienen que tener el mismo indice, si dos decisiones sacan el mismo texto se repite el texto y listo
    public List<DialogueConfig> nextDecisionsDialogues;
    public List<int> skipDialogues;
    public List<int> decisionFinalValue;

}
