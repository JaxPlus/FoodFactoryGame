using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BuildingB : MonoBehaviour
{
    public string Description => data.Description;
    public string Cost => data.Cost.ToString();
    public int buildingID;
    public string buildingGuid;
    [SerializeField] public List<GameObject> inputInventory = new();
    [SerializeField] protected int maxCapacity;
    public BuildingB output;
    private BuildingModel model;
    private BuildingData data;

    public void Setup(BuildingData data, float rotation, int ID)
    {
        this.data = data;
        buildingID = ID;
        buildingGuid = System.Guid.NewGuid().ToString();
        model = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        model.Rotate(rotation);

        if (inputInventory.Capacity != 0)
        {
            OutputPoint outputPoint = model.GetComponentInChildren<OutputPoint>();
            outputPoint.building = this;
            Destroy(outputPoint.GetComponent<SpriteRenderer>());

            maxCapacity = inputInventory.Capacity;
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

    public virtual bool AddToInventory(GameObject item)
    {
        if (inputInventory[0] != null) return false;

        inputInventory[0] = item;
        return true;
    }
}
