using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueController: MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public int index;
    public float textSpeed = 0.1f;
    public DialogueConfig dialogueConfig;
    UnityAction _onDialogueEnd;

    public void Init(UnityAction onDialogueEnd)
    {
        _onDialogueEnd = onDialogueEnd;
    }

    //assign the config
    public void AsignConfig(DialogueConfig dialogue)
    {
        dialogueConfig = dialogue;
    }


    // Start is called before the first frame update
    void Start()
    {
        displayText.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(displayText.text == dialogueConfig.lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                displayText.text = dialogueConfig.lines[index];

            }
        }
    }

    public void StartDialogue()
    {
        index = 0;

        StartCoroutine(WriteLine());
    }

    IEnumerator WriteLine()
    {
        foreach (var letter in dialogueConfig.lines[index].ToCharArray())
        {
            displayText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if(index < dialogueConfig.lines.Length-1)
        {
            index++;
            displayText.text = string.Empty;
            StartCoroutine(WriteLine());

        }
        else
        {
            //Acaban las lineas, se desactiva de momento 
            // gameObject.SetActive(false);
            displayText.text = string.Empty;
            _onDialogueEnd.Invoke();

        }
    }
}
