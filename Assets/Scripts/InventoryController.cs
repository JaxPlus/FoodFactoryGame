using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount = 77;
    //public GameObject[] itemPrefabs;
    Dictionary<int, int> itemCountCache = new();
    public event Action OnInventoryChanged;
    
    public static InventoryController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();
        RebuildItemCounts();
        
        //for (int i = 0; i < slotCount; i++)
        //{
        //    Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();

        //    if (i < itemPrefabs.Length)
        //    {
        //        GameObject item = Instantiate(itemPrefabs[i], slot.transform);
        //        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //        slot.currentItem = item;
        //    }
        //}
    }

    public void RebuildItemCounts()
    {
        itemCountCache.Clear();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();

                if (item != null)
                {
                    itemCountCache[item.ID] = itemCountCache.GetValueOrDefault(item.ID, 0) + item.quantity;
                }
            }
        }
        
        OnInventoryChanged?.Invoke();
    }
    
    public Dictionary<int, int> GetItemCounts() => itemCountCache;

    public GameObject FindOreByName(string oreName)
    {
        foreach (Item item in itemDictionary.itemPrefabs)
        {
            if (item.GetComponent<Image>().sprite.name == oreName)
            {
                return item.gameObject;
            }
        }

        return null;
    }

    public bool AddItem(GameObject itemPrefab)
    {
        Item itemToAdd = itemPrefab.GetComponent<Item>();
        if (itemToAdd == null) return false;

        // Dodawanie do stacka
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();

                if (slotItem != null && slotItem.ID == itemToAdd.ID)
                {
                    slotItem.AddToStack(itemToAdd.quantity);
                    RebuildItemCounts();
                    return true;
                }
            }
        }

        // Dodawanie gdzie jest miejsce
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                RebuildItemCounts();
                return true;
            }
        }

        Debug.Log("Inventory is full");
        return false;
    }
    
    public bool RemoveItem(GameObject itemPrefab)
    {
        Item itemToRemove = itemPrefab.GetComponent<Item>();
        if (itemToRemove == null) return false;

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();

                if (slotItem != null && slotItem.ID == itemToRemove.ID)
                {
                    slotItem.RemoveItem();
                    RebuildItemCounts();
                    return true;
                }
            }
        }

        return false;
    }

    public void RemoveItemsFromInventory(int itemID, int amountToRemove)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            if (amountToRemove <= 0) break;
            
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot?.currentItem?.GetComponent<Item>() is Item item && item.ID == itemID)
            {
                int removed = Mathf.Min(amountToRemove, item.quantity);
                item.RemoveFromStack(removed);
                amountToRemove -= removed;

                if (item.quantity == 0)
                {
                    Destroy(slot.currentItem);
                    slot.currentItem = null;
                }
            }
        }
        
        RebuildItemCounts();
    }

    public List<InventorySaveData> GetInventorySaveItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                invData.Add(new InventorySaveData
                {
                    itemID = item.ID,
                    slotIndex = slotTransform.GetSiblingIndex(),
                    quantity = item.quantity,
                });
            }
        }

        return invData;
    }

    public void SetInventoryData(List<InventorySaveData> invData)
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        foreach (InventorySaveData data in invData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);

                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    Item itemComponent = item.GetComponent<Item>();

                    if (itemComponent != null && data.quantity > 1)
                    {
                        itemComponent.quantity = data.quantity;
                        itemComponent.UpdateQuantityDisplay();
                    }

                    slot.currentItem = item;
                }
            }
        }
        
        RebuildItemCounts();
    }
}
