using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;




public class DecisionController : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public TMP_FontAsset fontDecision;
    public int resultIndex;
    public int indexForHead;
    public float textSpeed = 0.1f;
    public DecisionConfig decisionConfig;
    public List<ButtonDecisionController> buttonDecisions;
    public ButtonDecisionController buttonDecisionPrefabRef;
    UnityAction<int, DialogueConfig, int> _onButtonClick;

    bool isHeadTextDisplayed = false;

    

    public void Init(DecisionConfig decisionConfig, UnityAction<int, DialogueConfig, int> onButtonClick)
    {
        this.decisionConfig = decisionConfig;
        _onButtonClick = onButtonClick;
        StartDialogue();
        isHeadTextDisplayed = false;
        
    }

    public void InitOptions()
    {
        buttonDecisions = new List<ButtonDecisionController>();
        
        for (int i = 0; i < decisionConfig.decisions.Length; i++)
        {
            var button = Instantiate(buttonDecisionPrefabRef, transform);
            button.decisionText.font = fontDecision;
            button.transform.position = new Vector3(button.transform.position.x-300, button.transform.position.y-50*i, button.transform.position.z);
            button.Init(i, OnButtonClick, decisionConfig.decisions[i]);
            buttonDecisions.Add(button);
            
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isHeadTextDisplayed)
        {

            StopAllCoroutines();
            displayText.text = decisionConfig.headDisplay;
            InitOptions();
            isHeadTextDisplayed = true;
        }
    }

    public void StartDialogue()
    {
        displayText.text = string.Empty;

        indexForHead = 0;

        StartCoroutine(WriteLine());
    }

    IEnumerator WriteLine()
    {
        foreach (var letter in decisionConfig.headDisplay.ToCharArray())
        {
            displayText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }


    public void OnButtonClick(int index)
    {
        resultIndex = index;
        _onButtonClick.Invoke(resultIndex, decisionConfig.nextDecisionsDialogues[resultIndex], decisionConfig.skipDialogues[resultIndex]);
        for (int i = 0; i < buttonDecisions.Count; i++)
        {
            Destroy(buttonDecisions[i].gameObject);
        }
    }
}

