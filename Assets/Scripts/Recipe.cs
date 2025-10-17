using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Recipe : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public List<GameObject> ingredients;
    [SerializeField] public GameObject result;
    [SerializeField] public int resultQuantity;
    [SerializeField] public float craftingTime = 0.5f;
    private Dictionary<int, int> ingredientsDict;
    private InventoryController inventoryController;
    private int ingredientsCount;
    private TMP_Text quantityText;

    void Start()
    {
        inventoryController = InventoryController.Instance;
        
        ingredientsDict = new Dictionary<int, int>();
        quantityText = GetComponentInChildren<TMP_Text>();
        result.GetComponent<Item>().quantity = resultQuantity;
        quantityText.text = resultQuantity.ToString();

        SetItemDictionary();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        
        var invItems = inventoryController.GetInventorySaveItems();

        for (int i = 0; i < invItems.Count; i++)
        {
            if (ingredientsDict.ContainsKey(invItems[i].itemID))
            {
                ingredientsDict[invItems[i].itemID] -= invItems[i].quantity;
            }
        }

        foreach (var i in ingredientsDict)
        {
            if (i.Value <= 0)
            {
                ingredientsCount--;
            }
        }
            
        if (ingredientsCount == 0)
        {
            inventoryController.AddItem(result);
            Debug.Log("Recipe finished.");
            SetItemDictionary();

            foreach (GameObject ingredient in ingredients)
            {
                inventoryController.RemoveItem(ingredient);
            }
        }
        else
        {
            Debug.Log("Not enough items.");
        }
    }
    
    private void SetItemDictionary()
    {
        ingredientsDict.Clear();
        foreach (GameObject ingredient in ingredients)
        {
            var currItem = ingredient.GetComponent<Item>();
            
            if (!ingredientsDict.TryAdd(currItem.ID, 1))
            {
                ingredientsDict[currItem.ID]++;
            }
        }
        
        ingredientsCount = ingredientsDict.Count;
    }
}
