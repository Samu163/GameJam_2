using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Image itemsBg;
    public TextRPGInfo textDisplay;


    public void Init()
    {
        ShowItemsBg(false);
        ShowTextDisplay(false);
    }

    public void ShowItemsBg(bool isActive)
    {
        itemsBg.gameObject.SetActive(isActive);
    }

    public void ShowTextDisplay(bool condition)
    {
        textDisplay.gameObject.SetActive(condition);
    }
}
