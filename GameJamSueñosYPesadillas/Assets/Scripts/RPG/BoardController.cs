using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    public Image rpgBg;

    public void ChangeBg(Sprite newBg)
    {
        rpgBg.sprite = newBg;
    }




}
