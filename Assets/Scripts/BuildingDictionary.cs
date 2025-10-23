using System.Collections.Generic;
using UnityEngine;

public class BuildingDictionary : MonoBehaviour
{
    public List<Building> buildingPrefabs;
    public List<BuildingModel> buildingModelPrefabs;
    private Dictionary<int, GameObject> buildingDictionary;
    private Dictionary<int, GameObject> buildingModelDictionary;

    private void Awake()
    {
        buildingDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < buildingPrefabs.Count; i++)
        {
            if (buildingPrefabs[i] != null)
            {
                buildingPrefabs[i].ID = i + 1;
            }

            //if (buildingModelPrefabs[i] != null)
            //{
            //    //buildingModelPrefabs[i].ID = i + 1;
            //}
        }

        foreach (Building building in buildingPrefabs)
        {
            buildingDictionary[building.ID] = building.gameObject;
        }

        //foreach (Building building in buildingModelPrefabs)
        //{
        //    buildingModelDictionary[building.ID] = building.gameObject;
        //}
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

    public GameObject GetBuildingModelPrefab(int buildingID)
    {
        buildingModelDictionary.TryGetValue(buildingID, out GameObject prefab);

        if (prefab == null)
        {
            Debug.LogWarning($"Building model with ID {buildingID} doesn't exists");
        }

        return prefab;
    }
}
