using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BuildingB : MonoBehaviour
{
    // TUTAJ TRZEBA BĘDZIE ZROBIĆ AUTOMATYZACJĘ
    public string Description => data.Description;
    public string Cost => data.Cost.ToString();
    public int ID;
    [SerializeField] public List<Item> inputInventory = new(0);
    private BuildingB output;
    private BuildingModel model;
    private BuildingData data;

    public void Setup(BuildingData data, float rotation, int ID)
    {
        this.data = data;
        this.ID = ID;
        model = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        model.Rotate(rotation);

        // PIEC PRZEZ TO NIE MA OUTPUTA
        if (inputInventory.Capacity != 0)
        {
            OutputPoint outputPoint = model.GetComponentInChildren<OutputPoint>();
            outputPoint.building = this;
            Destroy(outputPoint.GetComponent<SpriteRenderer>());
        }
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
