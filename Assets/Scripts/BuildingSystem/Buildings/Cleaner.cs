using System;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : BuildingB
{
    public float cleaningSpeed = 2f;
    private ItemDictionary itemDictionary;

    private List<Tuple<string, int, int>> ores = new();

    void Start()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();
        ores.Add(Tuple.Create("Flour", itemDictionary.GetItemID("Flour Ore Clump"), itemDictionary.GetItemID("Flour")));
        ores.Add(Tuple.Create("Butter", itemDictionary.GetItemID("Butter Ore Clump"), itemDictionary.GetItemID("Butter")));
        ores.Add(Tuple.Create("Salt", itemDictionary.GetItemID("Salt Ore Clump"), itemDictionary.GetItemID("Salt")));

        InvokeRepeating(nameof(Clean), 0, cleaningSpeed);
    }

    void Clean()
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
