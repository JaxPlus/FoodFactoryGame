using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class BuildingSystem : MonoBehaviour
{
    public const float CellSize = 1f;
    [SerializeField] private BuildingData buildingData1;
    [SerializeField] private BuildingData buildingData2;
    [SerializeField] private BuildingPreview previewPrefab;
    [SerializeField] private BuildingB buildingPrefab;
    [SerializeField] private BuildingGrid grid;
    [SerializeField] private Camera playerCamera;
    private BuildingPreview preview;

    public static BuildingSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        Vector3 mousePos = GetMousePosition();

        if (preview != null)
        {
            HandlePreview(mousePos);
        }
    }

    public void SetPreview(BuildingData data)
    {
        Vector3 mousePos = GetMousePosition();
        preview = CreatePreview(data, mousePos);
    }

    private void HandlePreview(Vector3 mouseWorldPosition)
    {
        preview.transform.position = mouseWorldPosition;
        List<Vector3> buildPositions = preview.BuildingModel.GetAllBuildingPositions();
        bool canBuild = grid.CanBuild(buildPositions);

        if (canBuild)
        {
            preview.transform.position = GetSnappedCenterPosition(buildPositions);
            preview.ChangeState(BuildingPreview.BuildingPreviewState.POSITIVE);

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding(buildPositions);
            }
        }
        else
        {
            preview.ChangeState(BuildingPreview.BuildingPreviewState.NEGATIVE);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            preview.Rotate(90);
        }
    }

    private void PlaceBuilding(List<Vector3> buildPositions)
    {
        BuildingB building = Instantiate(buildingPrefab, preview.transform.position, Quaternion.identity);
        building.Setup(preview.Data, preview.BuildingModel.Rotation);
        grid.SetBuilding(building, buildPositions);
        Destroy(preview.gameObject);
        preview = null;
    }

    private Vector3 GetSnappedCenterPosition(List<Vector3> allBuildingPositions)
    {
        List<int> xs = allBuildingPositions.Select(p => Mathf.FloorToInt(p.x)).ToList();
        List<int> ys = allBuildingPositions.Select(p => Mathf.FloorToInt(p.y)).ToList();
        float centerX = (xs.Min() + xs.Max()) / 2f + CellSize / 2f;
        float centerY = (ys.Min() + ys.Max()) / 2f + CellSize / 2f;
        
        return new Vector3(centerX, centerY);
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mouseWorldPos = playerCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        return mouseWorldPos;
    }

    private BuildingPreview CreatePreview(BuildingData data, Vector3 position)
    {
        BuildingPreview buildingPreview = Instantiate(previewPrefab, position, Quaternion.identity);
        buildingPreview.Setup(data);
        return buildingPreview;
    }
}
