using System.Collections.Generic;
using UnityEngine;

public class BuildingDictionary : MonoBehaviour
{
    public List<Building> buildingPrefabs;
    private Dictionary<int, GameObject> buildingDictionary;

    private void Awake()
    {
        buildingDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < buildingPrefabs.Count; i++)
        {
            if (buildingPrefabs[i] != null)
            {
                buildingPrefabs[i].ID = i + 1;
            }
        }

        foreach (Building building in buildingPrefabs)
        {
            buildingDictionary[building.ID] = building.gameObject;
        }   
    }

    public GameObject GetItemPrefab(int buildingID)
    {
        buildingDictionary.TryGetValue(buildingID, out GameObject prefab);

        if (prefab == null)
        {
            Debug.LogWarning($"Building with ID {buildingID} doesn't exists");
        }

        return prefab;
    }
}
