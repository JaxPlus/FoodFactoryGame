using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Recipe : MonoBehaviour, IPointerClickHandler
{
    public List<GameObject> ingredients;
    public GameObject result;
    private InventoryController inventoryController;
    private int ingredientsCount;

    void Start()
    {
        inventoryController = InventoryController.Instance;
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
