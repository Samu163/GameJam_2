using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public DialogueController dialogue;
    public List<DialogueConfig> dialogueTexts;
    public int indexText = 0;

    // Start is called before the first frame update
    void Start()
    {
        dialogue.Init(TriggerEvent);
        dialogue.AsignConfig(dialogueTexts[indexText]);
        dialogue.StartDialogue();
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
        }
        else if (dialogue.dialogueConfig.endsDay)
        {
            //Carga la escena del rpg
            dialogue.gameObject.SetActive(false);
        }
        else
        {
            indexText++;
            dialogue.AsignConfig(dialogueTexts[indexText]);
            dialogue.StartDialogue();
        }
    }
}
