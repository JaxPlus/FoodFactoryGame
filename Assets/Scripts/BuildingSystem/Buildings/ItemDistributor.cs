using UnityEngine;

public class ItemDistributor : BuildingB
{
    public GameObject itemToDistribute;
    void Start()
    {
        InvokeRepeating(nameof(DistributeItem), 0, 1f);
    }

    void DistributeItem()
    {
        if (output == null) return;

        output.AddToInventory(itemToDistribute);
    }
}
