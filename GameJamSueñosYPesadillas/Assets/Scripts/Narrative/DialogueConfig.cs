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
    public AudioClip soundEffect;
    public DecisionConfig decision;
    public bool fadeInOut;
    public bool changeScenario;
    public bool endsDay;
    public int idAnim;
    public bool isLeft;
    public bool isOnlyRight;
    public bool isPunch;
    public bool isFlashback;
    public bool startFlashback;
    public bool endFlashback;
    public bool endDecision;
    public bool isDay2;

}
