using System;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : BuildingB
{
    public float bakingSpeed = 4f;
    private ItemDictionary itemDictionary;
    private List<Tuple<int, bool>> ingredients = new();

    void Start()
    {
        gameObject.transform.Rotate(new Vector3(0, 0, model.Rotation));
        model.transform.Find("Wrapper").SetPositionAndRotation(transform.position, new Quaternion(0, 0, 0, 0));
        
        itemDictionary = FindFirstObjectByType<ItemDictionary>();
        
        ingredients.Add(Tuple.Create(itemDictionary.GetItemID("Flour"), false));
        ingredients.Add(Tuple.Create(itemDictionary.GetItemID("Butter"), false));
        ingredients.Add(Tuple.Create(itemDictionary.GetItemID("Salt"), false));
        
        InvokeRepeating(nameof(Bake), 0, bakingSpeed);
    }

    void Bake()
    {
        if (output == null) return;
        var outputResult = output.AddToInventory(inputInventory[0]);
        if (inputInventory[0].GetComponent<Item>().ID == itemDictionary.GetItemID("Cake Matcha"))
        {
            if (outputResult)
            {
                inputInventory[0] = null;
            }
        }
        
        bool isFull = true;
        foreach (var item in inputInventory)
        {
            if (item == null)
            {
                isFull = false;
            }
        }
        
        if (!isFull) return;
        
        foreach (var itemObj in inputInventory)
        {
            Item item = itemObj.GetComponent<Item>();

            for (int i = 0; i < ingredients.Count; i++)
            {
                if (ingredients[i].Item1 == item.ID)
                {
                    ingredients[i] = Tuple.Create(ingredients[i].Item1, true);
                }
            }
        }

        bool areAllIngredientsPresent = true;
        foreach (var ingredient in ingredients)
        {
            if (!ingredient.Item2)
            {
                areAllIngredientsPresent = false;
            }
        }
        
        if (!areAllIngredientsPresent) return;

        for (int i = 0; i < ingredients.Count; i++)
        {
            ingredients[i] = Tuple.Create(ingredients[i].Item1, false);
            inputInventory[i] = null;
        }

        inputInventory[0] = itemDictionary.GetItemPrefab(itemDictionary.GetItemID("Cake Matcha"));
        
        outputResult = output.AddToInventory(inputInventory[0]);

        if (outputResult)
        {
            inputInventory[0] = null;
        }
    }

    public override bool AddToInventory(GameObject newItem)
    {
        bool isItemIn = false;

        if (inputInventory.TrueForAll(i => i != null)) return false;
        
        foreach (var itemIn in inputInventory)
        {
            if (itemIn == null) continue;
            
            if (itemIn.GetComponent<Item>().ID == newItem.GetComponent<Item>().ID)
            {
                isItemIn = true;
            }
        }

        if (isItemIn) return false;

        for (int i = 0; i < inputInventory.Count; i++)
        {
            if (inputInventory[i] == null)
            {
                inputInventory[i] = newItem;
                break;
            }
        }

        return true;
    }
}
