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
    private InventoryController inventoryController;
    private int ingredientsCount;
    private TMP_Text quantityText;

    void Start()
    {
        inventoryController = InventoryController.Instance;
        
        quantityText = GetComponentInChildren<TMP_Text>();
        result.GetComponent<Item>().quantity = resultQuantity;
        quantityText.text = resultQuantity.ToString();
        ingredientsCount = ingredients.Count;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var invItems = inventoryController.GetInventorySaveItems();

            for (int i = 0; i < invItems.Count; i++)
            {
                foreach (GameObject ingredient in ingredients)
                {
                    if (invItems[i].itemID == ingredient.GetComponent<Item>().ID)
                    {
                        ingredientsCount--;
                    }
                }
            }

            if (ingredientsCount == 0)
            {
                inventoryController.AddItem(result);
                Debug.Log("Recipe finished.");
                Debug.Log("Result quantity: " + result.GetComponent<Item>().quantity);
                Debug.Log("Result quantity 2: " + resultQuantity);
                ingredientsCount = ingredients.Count;

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
    }
}
