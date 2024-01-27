using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DialogueController dialogue;
    public List<DialogueConfig> dialogueTexts;
    public int indexText = 0;

    // Start is called before the first frame update
    void Start()
    {
        dialogue.AsignText(dialogueTexts[indexText].lines);
        dialogue.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
