using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Image playerImage;
    public int health;
    public int damage;

    public GameObject DispararButton;

    public bool isFighting = false;

    public void ShowFightMenu()
    {
        isFighting = !isFighting;

        DispararButton.gameObject.SetActive(isFighting);
    }
}
