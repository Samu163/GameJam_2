using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemController : MonoBehaviour
{

    public Image imageObject;
    public Image selectedItemFrame;
    public TextMeshProUGUI nameObject;
    public int idObject;
    public bool allyTarget;
    public bool enemyTarget;
    public string description;

    public void Init(ItemConfig itemConfig)
    {
        imageObject.sprite = itemConfig.imageObject;
        nameObject.text = itemConfig.nameObject;
        idObject = itemConfig.idObject;
        allyTarget = itemConfig.targetAllies;
        enemyTarget = itemConfig.targetEnemies;
        description = itemConfig.description;
        ShowSelectedItem(false);
    }

    public void ShowSelectedItem(bool condition)
    {
        selectedItemFrame.gameObject.SetActive(condition);
    }


    public void OnItemClick()
    {

    }
}
