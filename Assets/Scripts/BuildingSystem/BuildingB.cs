using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BuildingB : MonoBehaviour
{
    // TUTAJ TRZEBA BĘDZIE ZROBIĆ AUTOMATYZACJĘ
    public string Description => data.Description;
    public string Cost => data.Cost.ToString();
    public int ID;
    [SerializeField] public List<Item> inputInventory = new List<Item>(0);
    public BuildingB output;
    private BuildingModel model;
    private BuildingData data;

    public void Setup(BuildingData data, float rotation, int ID)
    {
        this.data = data;
        this.ID = ID;
        model = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        model.Rotate(rotation);
        output = null;
    }

    public BuildingData GetData()
    {
        return data;
    }

    public void SetOutput(BuildingB outputBuilding)
    {
        output = outputBuilding;
    }
}
