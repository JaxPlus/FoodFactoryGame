using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Belt : BuildingB
{
    private List<SpriteRenderer> itemDisplay;

    void Start()
    {
        InvokeRepeating(nameof(Transport), 0, 2f);
    }

    void Transport()
    {
        if (output == null) return;

        output.AddToInventory(inputInventory[0]);
        inputInventory[0] = inputInventory[1];
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

    public override void AddToInventory(GameObject item)
    {
        for (int i = 0; i < inputInventory.Count; i++)
        {
            if (inputInventory[i] == null)
            {
                inputInventory[i] = item;
                itemDisplay[i].sprite = item.GetComponent<Image>().sprite;
            }
            else
            {
                continue;
            }
        }

        return;
    }
}
