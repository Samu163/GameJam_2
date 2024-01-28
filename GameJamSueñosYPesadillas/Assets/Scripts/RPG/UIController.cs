using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public ItemManager itemInventoryPrefabRef;

    public void itemButton()
    {
        var inventory = Instantiate(itemInventoryPrefabRef, transform);
        inventory.transform.position = new Vector3(200, 50, 0);

    }
}
