using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] int maxNumItems;
    [SerializeField] ItemConfig[] menuItems;

    private ItemController item;

    private void Start()
    {
        for(int i = 0; i < maxNumItems; i++)
        {
            GameObject inventory = GameObject.Instantiate(itemPrefab, Vector2.zero, Quaternion.identity, GameObject.FindGameObjectWithTag("ItemMenu").transform);
            item = inventory.GetComponent<ItemController>();
            item.CreateItem(menuItems[i]);
        }
    }
}
