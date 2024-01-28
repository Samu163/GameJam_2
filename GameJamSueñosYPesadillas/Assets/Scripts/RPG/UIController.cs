using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public ItemManager itemInventoryPrefabRef;
    public bool isMenuActive = false;
    public bool isMenuCreated = false;

    private ItemManager inventory;

    public void itemButton()
    {
        
        if(!isMenuCreated)
        {
            inventory = Instantiate(itemInventoryPrefabRef, transform);
            inventory.transform.position = new Vector3(200, 50, 0);
            isMenuCreated = true;
        }

        if(!isMenuActive)
        {
            inventory.gameObject.SetActive(true);
        } 
        else if (isMenuActive)
        {
            inventory.gameObject.SetActive(false);
        }
        isMenuActive = !isMenuActive;
        
    }
}
