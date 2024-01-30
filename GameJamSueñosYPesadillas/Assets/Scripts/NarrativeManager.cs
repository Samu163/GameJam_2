using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NarrativeManager : MonoBehaviour
{
    public DialogueController dialogue;
    public DecisionController decision;
    public Image characterTalking;
    public Text nameChar;
    public Image background;
    public Animator transitionAnimator;
    public List<DialogueConfig> dialogueTexts;
    public List<DecisionConfig> decisionsTexts;
    public List<int> decisions;
    public int indexText = 0;
    public int indexDecisions = 0;


    //Cargar datos del game manager
    public void Awake()
    {
        indexText = GameManager.instance.lastTextIndex;
        indexDecisions = GameManager.instance.lastDecisionIndex;
    }

    //Notas: no pongais en la lista de dialogos los dialogos de la recompensa de decisiones

    // Start is called before the first frame update
    void Start()
    {
        dialogue.Init(TriggerEvent);
        dialogue.AsignConfig(dialogueTexts[indexText]);
        dialogue.StartDialogue();
        characterTalking.sprite = dialogue.dialogueConfig.characterImg;
        background.sprite = dialogue.dialogueConfig.backgroundScene;
        nameChar.text = dialogue.dialogueConfig.nameChar;
        decision.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerEvent()
    {
        
        if (dialogue.dialogueConfig.hasDecision)
        {
            //Carga la decision y cuando la decision se ejecute carga el siguiente dialogo (pero eso en otra parte)
            dialogue.gameObject.SetActive(false);
            decision.gameObject.SetActive(true);
            decision.Init(decisionsTexts[indexDecisions], DecisionResult);
            indexDecisions++;
        }
        else if (dialogue.dialogueConfig.endsDay)
        {
            GameManager.instance.SaveNarrative(decisions, indexText, indexDecisions);
            //Carga la escena del rpg
            SceneManager.LoadScene("RPG");
            //dialogue.gameObject.SetActive(false);
        }
        else
        {
            indexText++;
            dialogue.AsignConfig(dialogueTexts[indexText]);
            dialogue.transitionAnimator = transitionAnimator;
            dialogue.StartDialogue();
            characterTalking.sprite = dialogue.dialogueConfig.characterImg;
            nameChar.text = dialogue.dialogueConfig.nameChar;
            background.sprite = dialogue.dialogueConfig.backgroundScene;
            
        }

       
        
    }

    public void DecisionResult(int result, DialogueConfig nextDialogue, int dialoguesToSkip)
    {
        decisions.Add(result);
        indexText += dialoguesToSkip;
        decision.gameObject.SetActive(false);
        dialogue.gameObject.SetActive(true);
        dialogue.AsignConfig(nextDialogue);
        dialogue.StartDialogue();
        characterTalking.sprite = dialogue.dialogueConfig.characterImg;
        background.sprite = dialogue.dialogueConfig.backgroundScene;
        nameChar.text = dialogue.dialogueConfig.nameChar;
    }
}
