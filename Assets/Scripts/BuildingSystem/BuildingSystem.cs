using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingSystem : MonoBehaviour
{
    public const float CellSize = 1f;
    [SerializeField] private BuildingPreview previewPrefab;
    [SerializeField] private BuildingB buildingPrefab;
    [SerializeField] private BuildingGrid grid;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject hotbarBuildingInfo;
    public int currentBuildingID { get; private set; } = -1;
    private BuildingPreview preview;
    private int UILayer;
    private Dictionary<int, int> ingredientsDict;
    private int ingredientsCount;

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

    private void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
        ingredientsDict = new Dictionary<int, int>();
    }

    // @TODO Zrobić niszczenie budynków
    private void Update()
    {
        Vector3 mousePos = GetMousePosition();

        if (preview != null)
        {
            HandlePreview(mousePos);
        }
    }

    public void SetPreview(BuildingData data, int buildingID)
    {
        Vector3 mousePos = GetMousePosition();
        hotbarBuildingInfo.GetComponent<HotbarBuildingInfo>().ShowBuildingInfo(data);
        preview = CreatePreview(data, mousePos);
        currentBuildingID = buildingID;
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

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
            {
                if (HasEnoughResources(preview.Data))
                {
                    PlaceBuilding(buildPositions);
                    hotbarBuildingInfo.GetComponent<HotbarBuildingInfo>().HideBuildingInfo();
                    Debug.Log("Building complete");
                }
                else
                {
                    Debug.Log("Not enough resources");
                }
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
    private bool HasEnoughResources(BuildingData data)
    {
        SetItemDictionary(data);
        var invItems = InventoryController.Instance.GetInventorySaveItems();

        for (int i = 0; i < invItems.Count; i++)
        {
            if (ingredientsDict.ContainsKey(invItems[i].itemID))
            {
                ingredientsDict[invItems[i].itemID] -= invItems[i].quantity;
            }
        }

        ingredientsCount = ingredientsDict.Count;

        foreach (var i in ingredientsDict)
        {
            if (i.Value <= 0)
            {
                ingredientsCount--;
            }
        }

        if (ingredientsCount == 0)
        {
            foreach (GameObject ingredient in data.Cost)
            {
                InventoryController.Instance.RemoveItem(ingredient);
            }

            return true;
        }
        
        return false;
    }

    private void SetItemDictionary(BuildingData buildingData)
    {
        ingredientsDict.Clear();
        foreach (GameObject items in buildingData.Cost)
        {
            var currItem = items.GetComponent<Item>();

            if (!ingredientsDict.TryAdd(currItem.ID, 1))
            {
                ingredientsDict[currItem.ID]++;
            }
        }

        ingredientsCount = ingredientsDict.Count;
    }

    public void StopPreview()
    {
        Destroy(preview.gameObject);
        currentBuildingID = -1;
        preview = null;
        hotbarBuildingInfo.GetComponent<HotbarBuildingInfo>().HideBuildingInfo();
    }

    private void PlaceBuilding(List<Vector3> buildPositions)
    {
        BuildingB building = Instantiate(preview.Data.Prefab != null ? preview.Data.Prefab : buildingPrefab, preview.transform.position, Quaternion.identity);
        building.Setup(preview.Data, preview.BuildingModel.Rotation, currentBuildingID);
        grid.SetBuilding(building, buildPositions);
        Destroy(preview.gameObject);
        preview = null;
        currentBuildingID = -1;
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
    
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaycastResults)
    {
        return eventSystemRaycastResults.Any(curRaycastResult => curRaycastResult.gameObject.layer == UILayer);
    }

    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }
}