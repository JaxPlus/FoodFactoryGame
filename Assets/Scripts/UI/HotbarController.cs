using UnityEngine;

public class HotbarController : MonoBehaviour
{
    public GameObject[] panels;
    public int[] slotCounts = {2, 1};
    private int currentPanelActive = 0;

    private BuildingDictionary buildingDictionary;

    private void Awake()
    {
        buildingDictionary = FindFirstObjectByType<BuildingDictionary>();
    }

    void Start()
    {
        ActivateHotbarPanel(0);

        int currentBuilding = 0;
        for (int i = 0; i < panels.Length; i++)
        {
            for (int j = 0; j < slotCounts[i]; j++)
            {
                //Slot slot = Instantiate(slotPrefab, panels[i].transform).GetComponent<Slot>();
                
                //GameObject building = Instantiate(buildingDictionary.GetComponent<BuildingDictionary>().buildingPrefabs[currentBuilding].gameObject, slot.transform);
                //building.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                //slot.currentItem = building;
                Instantiate(buildingDictionary.GetComponent<BuildingDictionary>().buildingPrefabs[currentBuilding].gameObject, panels[i].transform);
                currentBuilding++;
            }
        }
    }

    void StartBuildingWith(int index)
    {
        Slot slot = panels[currentPanelActive].transform.GetChild(index).GetComponent<Slot>();

        if (slot.currentItem != null)
        {
            Building building = slot.currentItem.GetComponent<Building>();
            // start building
        }
    }

    public void ActivateHotbarPanel(int hotbarPanelNum)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        panels[hotbarPanelNum].SetActive(true);
        currentPanelActive = hotbarPanelNum;
    }
}
