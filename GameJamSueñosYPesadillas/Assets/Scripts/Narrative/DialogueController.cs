using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class DialogueController: MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public Animator transitionAnimator;
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

    public void AsignAnimator(Animator animator)
    {
        transitionAnimator = animator;
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
            

            if (displayText.text == dialogueConfig.lines[index])
            {
                
                if (dialogueConfig.fadeInOut && index >= dialogueConfig.lines.Length - 1)
                {
                    FadeInOut();
                    Invoke("NextLine", 3);
                }
                else
                {
                    transitionAnimator.Play("Idle");
                    NextLine();
                }
                
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
            //Acaban las lineas
            displayText.text = string.Empty;
            _onDialogueEnd.Invoke();
        }
    }

    public void FadeInOut()
    {
        transitionAnimator.Play("FadeInOut");
    }


}
