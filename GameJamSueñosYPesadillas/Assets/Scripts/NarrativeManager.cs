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
    public GameObject filtroAzul;
    public Image characterTalking;
    public Text nameChar;
    public Image background;
    public Animator transitionAnimator;
    public Animator sceneAnimator;
    public Animator dreamAnimator;
    public Animator blackDreamAnimator;
    public Animator characterAnimator;
    public Animator characterAnimator2;
    public Animator itemAnimator;
    public AudioSource musicPlayer;
    public AudioClip ambientSound;
    public AudioClip ambientSound2;
    public AudioClip decisionSound;
    public AudioClip flashbackSound;
    public List<DialogueConfig> dialogueTexts;
    public List<DecisionConfig> decisionsTexts;
    public List<DialogueConfig> dialogueTextsEnglish;
    public List<DecisionConfig> decisionsTextsEnglish;
    public List<int> decisions;
    public int indexText = 0;
    public int indexDecisions = 0;
    public string language;
    public bool isEndingDay = false;
    


    //Cargar datos del game manager
    public void Awake()
    {
        indexText = GameManager.instance.lastTextIndex;
        indexDecisions = GameManager.instance.lastDecisionIndex;
        language = GameManager.instance.language;
    }

    //Notas: no pongais en la lista de dialogos los dialogos de la recompensa de decisiones

    // Start is called before the first frame update
    void Start()
    {
        dialogue.Init(TriggerEvent);

        if (language == "Español")
        {
            dialogue.AsignConfig(dialogueTexts[indexText]);
        } 
        else if (language == "English")
        {
            dialogue.AsignConfig(dialogueTextsEnglish[indexText]);
        }
        

        dialogue.StartDialogue();

        dialogue.transitionAnimator = transitionAnimator;
        dialogue.characterAnimator = characterAnimator;
        dialogue.characterAnimator2 = characterAnimator2;
        dialogue.sceneAnimator = sceneAnimator;
        dialogue.itemAnimator = itemAnimator;
        dialogue.blackDreamAnimator = blackDreamAnimator;
        characterAnimator2.SetInteger("IdAnim", 13);
        characterAnimator.SetInteger("IdAnim", 0);
        background.sprite = dialogue.dialogueConfig.backgroundScene;
        nameChar.text = dialogue.dialogueConfig.nameChar;
        decision.gameObject.SetActive(false);
        musicPlayer.clip = ambientSound;
        musicPlayer.Play();
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
            musicPlayer.clip = decisionSound;
            musicPlayer.Play();
            dialogue.gameObject.SetActive(false);
            decision.gameObject.SetActive(true);

            if (language == "Español")
            {
                decision.Init(decisionsTexts[indexDecisions], DecisionResult);
            }
            else if (language == "English")
            {
                decision.Init(decisionsTextsEnglish[indexDecisions], DecisionResult);
            }
            


            indexDecisions++;
        }
        else if (dialogue.dialogueConfig.endsDay)
        {

            dreamAnimator.gameObject.SetActive(true);
            transitionAnimator.gameObject.SetActive(true);
            blackDreamAnimator.gameObject.SetActive(true);

            Black();
            Dream();

            GameManager.instance.SaveNarrative(decisions, indexText, indexDecisions);

            Invoke("BlackDream", 10);
            Invoke("LoadRPG", 13);

        }
        else
        {
            indexText++;

            if(language == "Español")
            {
                dialogue.AsignConfig(dialogueTexts[indexText]);
            }
            else if (language == "English")
            {
                dialogue.AsignConfig(dialogueTextsEnglish[indexText]);
            }
            


            dialogue.StartDialogue();
            nameChar.text = dialogue.dialogueConfig.nameChar;
            background.sprite = dialogue.dialogueConfig.backgroundScene;
            
            
        }

        if(dialogue.dialogueConfig.endDecision)
        {
            musicPlayer.clip = ambientSound;
            musicPlayer.Play();
        } 
        else if (dialogue.dialogueConfig.endDecision && dialogue.dialogueConfig.isDay2)
        {
            musicPlayer.clip = ambientSound2;
            musicPlayer.Play();
        }
        if (dialogue.dialogueConfig.isDay2)
        {
            musicPlayer.clip = ambientSound2;
            musicPlayer.Play();
        }
        if (dialogue.dialogueConfig.startFlashback)
        {
            filtroAzul.SetActive(true);
            musicPlayer.clip = flashbackSound;
            musicPlayer.Play();
        }
        if (dialogue.dialogueConfig.endFlashback)
        {
            filtroAzul.SetActive(false);
            musicPlayer.clip = ambientSound;
            musicPlayer.Play();
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
    public void Dream()
    {
        dreamAnimator.Play("DreamAnim");
    }

    public void Black()
    {
        transitionAnimator.Play("InBlack");
    }

    public void BlackDream()
    {
        blackDreamAnimator.Play("FadeInOut");
    }

    public void LoadRPG()
    {

        //Carga la escena del rpg
        SceneManager.LoadScene("RPG");
        isEndingDay = false;

    }
}
