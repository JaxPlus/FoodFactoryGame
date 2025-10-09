using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public string Name;
    public int quantity = 1;

    private TMP_Text quantityText;

    private void Awake()
    {
        quantityText = GetComponentInChildren<TMP_Text>();
        UpdateQuantityDisplay();
    }

    public void AddToStack(int amount = 1)
    {
        quantity += amount;
        UpdateQuantityDisplay();
    }

    public int RemoveFromStack(int amount = 1)
    {
        int removed = Mathf.Min(amount, quantity);
        quantity -= removed;
        UpdateQuantityDisplay();
        return removed;
    }

    public void RemoveItem(int amount = 1)
    {
        if (quantity - amount <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            quantity -= Mathf.Min(amount, quantity);
            UpdateQuantityDisplay();
        }
    }

    public GameObject CloneItem(int newQuantity)
    {
        GameObject clone = Instantiate(gameObject);
        Item cloneItem = clone.GetComponent<Item>();
        cloneItem.quantity = newQuantity;
        cloneItem.UpdateQuantityDisplay();
        return clone;
    }

    public void UpdateQuantityDisplay()
    {
        if (quantityText != null) 
        { 
            quantityText.text = quantity > 1 ? quantity.ToString() : "";
        }
    }
}
