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
    public Animator characterAnimator;
    public Animator characterAnimator2;
    public Animator itemAnimator;
    public Animator sceneAnimator;
    public Animator blackDreamAnimator;
    public GameObject panelTransition;
    public AudioSource soundEffectPlayer;
    
    public int index;
    public bool canClick;
    public float textSpeed = 0.1f;
    public DialogueConfig dialogueConfig;
    UnityAction _onDialogueEnd;

    private bool hasAnimated = false;
    public bool endingDay = false;

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
        canClick = true;
        textSpeed = GameManager.instance.textSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if(dialogueConfig.isOnlyRight)
        {
            characterAnimator2.gameObject.SetActive(false);
        }
        else
        {
            characterAnimator2.gameObject.SetActive(true);
        }

        if (dialogueConfig.isLeft)
        {
           
             characterAnimator2.SetInteger("IdAnim", dialogueConfig.idAnim);
            
        }
        else
        {
             characterAnimator.SetInteger("IdAnim", dialogueConfig.idAnim);
        }
       
        if(dialogueConfig.isColgante)
        {
            itemAnimator.SetBool("isColgante", true);
        }
        else
        {
            itemAnimator.SetBool("isColgante", false);
        }

        if (dialogueConfig.isGun)
        {
            itemAnimator.SetBool("isGun", true);
        }
        else
        {
            itemAnimator.SetBool("isGun", false);
        }

        if (dialogueConfig.isIdCard)
        {
            itemAnimator.SetBool("isIdCard", true);
        }
        else
        {
            itemAnimator.SetBool("isIdCard", false);
        }



        if (Input.GetMouseButtonDown(0) && canClick)
        {

            if(dialogueConfig.policeSound)
            {
                soundEffectPlayer.clip = dialogueConfig.soundEffect;
                soundEffectPlayer.Play();
            }

            if (displayText.text == dialogueConfig.lines[index])
            {
                
                if (dialogueConfig.fadeInOut && index >= dialogueConfig.lines.Length - 1)
                {
                    panelTransition.SetActive(true);
                    sceneAnimator.gameObject.SetActive(true);
                    if(dialogueConfig.isFlashback)
                    {
                        
                        Flashback();
                        Invoke("NextLine", 5);
                        Invoke("SetCanClick", 6);

                    }
                    else if (dialogueConfig.isPunch)
                    {
                        
                        Punch();
                        Invoke("NextLine", 5);
                        Invoke("SetCanClick", 6);
                    }
                    else if (dialogueConfig.isRoomScene)
                    {
                        Room();
                        Invoke("NextLine", 2);
                        Invoke("SetCanClick", 6);
                    }
                    else if (dialogueConfig.endsDay && !endingDay)
                    {
                        blackDreamAnimator.gameObject.SetActive(true);
                        blackDreamAnimator.Play("FadeInOut");
                        Invoke("NextLine", 3);
                        Invoke("SetCanClick", 18);
                        endingDay = true;
                    }
                    else
                    {
                        sceneAnimator.gameObject.SetActive(false);
                        FadeInOut();
                        Invoke("NextLine", 3);
                        Invoke("SetCanClick", 6);
                    }
                    
                    canClick = false;
                    soundEffectPlayer.clip = dialogueConfig.soundEffect;
                    soundEffectPlayer.Play();

                    
                    
                }
                else
                {

                    if (!dialogueConfig.endsDay)
                    {
                        transitionAnimator.gameObject.SetActive(false);
                    }
                    
                    sceneAnimator.Play("Idle");
                    sceneAnimator.gameObject.SetActive(false);
                    panelTransition.SetActive(false);
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
        hasAnimated = false;
        if (index < dialogueConfig.lines.Length-1)
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

    public void FadeIn()
    {
        transitionAnimator.Play("FadeIn");

    }

    public void FadeOut()
    {
        transitionAnimator.Play("FadeOut");

    }

    public void Flashback()
    {
        sceneAnimator.Play("FlashbackAnim");

    }

    

    public void Punch()
    {
        sceneAnimator.Play("PunchAnim");

    }

    public void Room()
    {
        sceneAnimator.Play("RoomAnim");

    }

    public void SetCanClick()
    {
        canClick = true;
        if (!dialogueConfig.endsDay)
        {
            transitionAnimator.gameObject.SetActive(false);
        }
    }

}
