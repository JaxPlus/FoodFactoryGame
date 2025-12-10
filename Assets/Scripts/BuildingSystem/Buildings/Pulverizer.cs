using System;
using System.Collections.Generic;
using UnityEngine;

public class Pulverizer : BuildingB
{
    [SerializeField] private float pulvarizeSpeed = 2f;
    private ItemDictionary itemDictionary;

    // NAZWA, FROM, TO
    private List<Tuple<string, int, int>> ores = new(); 

    void Start()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();
        ores.Add(Tuple.Create("Flour Ore", itemDictionary.GetItemID("Flour Ore"), itemDictionary.GetItemID("Flour Ore Clump")));
        ores.Add(Tuple.Create("Butter Ore", itemDictionary.GetItemID("Butter Ore"), itemDictionary.GetItemID("Butter Ore Clump")));
        ores.Add(Tuple.Create("Salt Ore", itemDictionary.GetItemID("Salt Ore"), itemDictionary.GetItemID("Salt Ore Clump")));

        InvokeRepeating(nameof(Pulvarize), 0, pulvarizeSpeed);
    }

    void Pulvarize()
    {
        if (inputInventory[0] == null) return;

        Item oreToProcess = inputInventory[0].GetComponent<Item>();

        foreach (var ore in ores)
        {
            if (oreToProcess.ID == ore.Item2)
            {
                inputInventory[0] = itemDictionary.GetItemPrefab(ore.Item3);
            }
        }

        if (output == null) return;

        var outputResult = output.AddToInventory(inputInventory[0]);
        
        if (outputResult)
        {
            inputInventory[0] = null;
        }
    }
}
