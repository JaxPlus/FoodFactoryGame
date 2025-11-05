using System.Collections.Generic;
using UnityEngine;

public class BuildingDictionary : MonoBehaviour
{
    public List<Building> buildingPrefabs;
    public List<BuildingData> buildingDataList;
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

    public GameObject GetBuildingPrefab(int buildingID)
    {
        buildingDictionary.TryGetValue(buildingID, out GameObject prefab);

        if (prefab == null)
        {
            Debug.LogWarning($"Building with ID {buildingID} doesn't exists");
        }

        return prefab;
    }

    public BuildingData GetBuildingData(int buildingID)
    {
        buildingDictionary.TryGetValue(buildingID, out GameObject prefab);

        if (prefab == null)
        {
            Debug.LogWarning($"Building model with ID {buildingID} doesn't exists");
        }

        foreach (BuildingData data in buildingDataList)
        {
            if (data.Name == prefab.GetComponent<Building>().Name)
            {
                return data;
            }
        }

        return null;
    }
}
