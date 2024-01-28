using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Image playerImage;
    public PlayerConfig config;

    public GameObject DispararButton;

    public bool isFighting = false;

    public void Init(PlayerConfig config)
    {
        this.config = config;
    }


    public void ShowFightMenu()
    {
        isFighting = !isFighting;

        DispararButton.gameObject.SetActive(isFighting);
    }
}
