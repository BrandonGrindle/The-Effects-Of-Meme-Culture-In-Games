using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Items> items = new List<Items>();

    public Transform ItemContent;
    public GameObject ItemContainer;

    public ItemController[] ItemController;
    private void Awake()
    {
        Instance = this; 
    }

    public void AddItem(Items item)
    {
        items.Add(item);
    }

    public void RemoveItem(Items item) {  
        items.Remove(item); 
    }

    public void listItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in items) 
        {
            GameObject obj = Instantiate(ItemContainer, ItemContent);
            var ItemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var ItemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            

            ItemName.text = item.ItemName;
            ItemIcon.sprite = item.ItemSprite;
        }

        SetinvItems();
    }

    public void SetinvItems()
    {
        ItemController = ItemContent.GetComponentsInChildren<ItemController>();

        for (int i = 0; i < items.Count;  i++)
        {
            ItemController[i].addItem(items[i]);
        }
    }
}
