using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{

    public Image itemsBg;



    public void Init()
    {
        ShowItemsBg(false);
    }

    public void ShowItemsBg(bool isActive)
    {
        itemsBg.gameObject.SetActive(isActive);
    }
}
