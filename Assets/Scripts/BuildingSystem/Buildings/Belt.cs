using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Belt : BuildingB
{
    private List<SpriteRenderer> itemDisplay;
    public float beltSpeed = 2f;

    void Start()
    {
        InvokeRepeating(nameof(Transport), beltSpeed, beltSpeed);
    }

    void Transport()
    {
        if (inputInventory[0] != null && inputInventory[1] == null)
        {
            inputInventory[1] = inputInventory[0];
            inputInventory[0] = null;
            itemDisplay[1].sprite = inputInventory[1].GetComponent<Image>().sprite;
            itemDisplay[0].sprite = null;
            return;
        }

        if (output == null) return;
        
        if (inputInventory[1] != null)
        {
            if (!output.AddToInventory(inputInventory[1])) return;
            inputInventory[1] = null;
            itemDisplay[1].sprite = null;
        }
    }

    public void SetItemDisplay(GameObject display)
    {
        itemDisplay = new List<SpriteRenderer>();
        var itemDisplaySlots = display.GetComponentsInChildren<SpriteRenderer>();
        foreach (var itemSlot in itemDisplaySlots)
        {
            itemSlot.sprite = null;
            itemDisplay.Add(itemSlot);
        }
    }

    public bool IsItemDisplaySet()
    {
        return itemDisplay != null && itemDisplay.Count > 0;
    }

    public override bool AddToInventory(GameObject item)
    {
        if (inputInventory[0] != null) return false;

        inputInventory[0] = item;
        itemDisplay[0].sprite = item.GetComponent<Image>().sprite;
        return true;
    }
}
