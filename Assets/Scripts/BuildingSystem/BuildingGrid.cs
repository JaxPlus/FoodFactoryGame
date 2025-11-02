using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingGrid : MonoBehaviour
{
    [SerializeField] private int height;
    [SerializeField] private int width;
    private BuildingGridCell[,] grid;

    private void Start()
    {
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
                    buildingID = grid[x, y].GetBuildingID()
                });
            }
        }

        return buildingGridCellData;
    }

    public void SetBuildingGridCells(BuildingGridCell[,] gridCells)
    {
        grid = gridCells;
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
        return building.ID;
    }

    public bool IsEmpty()
    {
        return building == null;
    }
}
