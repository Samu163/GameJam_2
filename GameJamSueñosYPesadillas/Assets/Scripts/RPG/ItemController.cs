using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemController : MonoBehaviour
{

    [SerializeField] Image imageObject;
    [SerializeField] TextMeshProUGUI nameObject;
    public int idObject;

    public void CreateItem(ItemConfig itemConfig)
    {
        imageObject.sprite = itemConfig.imageObject;
        nameObject.text = itemConfig.nameObject;
        idObject = itemConfig.idObject;
    }
}
