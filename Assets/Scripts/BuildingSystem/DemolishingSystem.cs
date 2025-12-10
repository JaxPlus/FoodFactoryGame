using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DemolishingSystem : MonoBehaviour
{
    public bool DemolishingMode = false;
    [SerializeField] private GameObject demolishingModePanel;
    [SerializeField] private GameObject hotbar;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject buildingGridObject;
    [SerializeField] private GameObject buildingGrid;
    public static DemolishingSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            DemolishingMode = !DemolishingMode;
            demolishingModePanel.SetActive(DemolishingMode);
            hotbar.SetActive(!DemolishingMode);
        }

        if (DemolishingMode && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Transform objectHit = hit.collider.transform;

                if (!objectHit.CompareTag("Building"))
                    return;

                string buildingGuid = objectHit.GetComponent<BuildingB>().buildingGuid;
                List<GameObject> returnItems = objectHit.GetComponent<BuildingB>().GetData().Cost;
                foreach (var item in returnItems)
                {
                    item.GetComponent<Item>().quantity = 1;
                }
                for (int i = 0; i < returnItems.Count; i++)
                {
                    InventoryController.Instance.AddItem(returnItems[i]);
                }

                foreach (var building in buildingGridObject.GetComponentsInChildren<BuildingB>())
                {
                    if (building.buildingGuid == buildingGuid)
                    {
                        Destroy(building.gameObject);
                    }
                }

                buildingGrid.GetComponent<BuildingGrid>().DestroyBuilding(buildingGuid);

                Debug.Log("Destroying complete");
            }
        }
    }
}
