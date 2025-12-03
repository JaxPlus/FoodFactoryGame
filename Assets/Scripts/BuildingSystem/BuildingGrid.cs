using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BuildingGrid : MonoBehaviour
{
    [SerializeField] public BuildingB buildingPrefab;

    [SerializeField] private int height;
    [SerializeField] private int width;
    private BuildingGridCell[,] grid;
    private BuildingDictionary dictionary;

    private void Start()
    {
        dictionary = FindFirstObjectByType<BuildingDictionary>();
        grid = new BuildingGridCell[height, width];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = new BuildingGridCell();
            }
        }
    }

    public List<BuildingGridCellData> GetBuildingGridCells()
    {
        List<BuildingGridCellData> buildingGridCellData = new List<BuildingGridCellData>(height * width);
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                buildingGridCellData.Add(new BuildingGridCellData
                {
                    buildingID = grid[x, y].GetBuildingID(),
                    buildingPosition = grid[x, y].GetBuildingPosition(),
                    rotation = grid[x, y].GetBuildingRotation(),
                });
            }
        }

        return buildingGridCellData;
    }

    public void SetBuildingGridCells(List<BuildingGridCellData> buildingGridCellsData)
    {
        if (buildingGridCellsData.Count == 0) 
            return;

        int i = 0;
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (buildingGridCellsData[i].buildingID == -1)
                    continue;

                BuildingData data = dictionary.GetBuildingData(buildingGridCellsData[i].buildingID);
                BuildingB building = Instantiate(buildingPrefab, buildingGridCellsData[i].buildingPosition, Quaternion.identity);
                building.Setup(data, buildingGridCellsData[i].rotation, buildingGridCellsData[i].buildingID);
                SetBuilding(building, building.GetData().Model.GetAllBuildingPositions());
                i++;
            }
        }
    }

    public void DestroyBuilding(string guid)
    {
        foreach (BuildingGridCell buildingGridCell in grid)
        {
            buildingGridCell.DeleteBuilding(guid);
        }
    }

    public void SetBuilding(BuildingB building, List<Vector3> allBuildingPositions)
    {
        foreach (var position in allBuildingPositions)
        {
            (int x, int y) = WorldToGridPosition(position);
            grid[x, y].SetBuilding(building);
        }
    }

    public bool CanBuild(List<Vector3> allBuildingPositions)
    {
        foreach (var position in allBuildingPositions)
        {
            (int x, int y) = WorldToGridPosition(position);
            
            if (x < 0 || x >= width || y < 0 || y >= height) return false;
            if (!grid[x, y].IsEmpty()) return false;
        }
        
        return true;
    }

    private (int x, int y) WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - transform.position).x / BuildingSystem.CellSize);
        int y = Mathf.FloorToInt((worldPosition - transform.position).y / BuildingSystem.CellSize);
        return (x, y);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        if (BuildingSystem.CellSize <= 0 || width <= 0 || height <= 0) return;

        Vector3 origin = transform.position;

        for (int y = 0; y <= height; y++)
        {
            Vector3 start = origin + new Vector3(0, y * BuildingSystem.CellSize, 0);
            Vector3 end = origin + new Vector3(width * BuildingSystem.CellSize, y * BuildingSystem.CellSize, 0);
            Gizmos.DrawLine(start, end);
        }

        for (int x = 0; x <= width; x++)
        {
            Vector3 start = origin + new Vector3(x * BuildingSystem.CellSize, 0, 0);
            Vector3 end = origin + new Vector3(x * BuildingSystem.CellSize, height * BuildingSystem.CellSize, 0);
            Gizmos.DrawLine(start, end);
        }
    }
}

public class BuildingGridCell
{
    private BuildingB building;

    public void SetBuilding(BuildingB building)
    {
        this.building = building;
    }

    public int GetBuildingID()
    {
        if (building == null) return -1;
        return building.buildingID;
    }

    public void DeleteBuilding(string guid)
    {
        if (building == null) return;

        if (building.buildingGuid == guid)
        {
            building = null;
        }
    }

    public Vector3 GetBuildingPosition()
    {
        if (building == null) return Vector3.zero;
        return building.transform.position;
    }

    public float GetBuildingRotation()
    {
        if (building == null) return 0;
        return building.GetData().Model.Rotation;
    }

    public bool IsEmpty()
    {
        return building == null;
    }
}
