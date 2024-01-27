using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class ButtonDecisionController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI decisionText;
    public Image selectionIcon;
    public int indexOfDecision;
    UnityAction<int> onButtonClick;


    public void Init(int indexOfDecision, UnityAction<int> onButtonClick, string decisionText)
    {
        this.indexOfDecision = indexOfDecision;
        this.onButtonClick = onButtonClick;
        this.decisionText.text = decisionText;
        selectionIcon.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectionIcon.gameObject.SetActive(true);
        decisionText.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectionIcon.gameObject.SetActive(false);
        decisionText.color = Color.black;
    }

public void Clicked()
    {
        onButtonClick.Invoke(indexOfDecision);
    }
}
