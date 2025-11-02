using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private BuildingGrid buildingGrid;

    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindFirstObjectByType<InventoryController>();
        buildingGrid = FindFirstObjectByType<BuildingGrid>();

        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            inventorySaveData = inventoryController.GetInventorySaveItems(),
            buildingGridData = buildingGrid.GetBuildingGridCells(),
        };

        Debug.Log(JsonUtility.ToJson(saveData.buildingGridData));

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void SaveAndExit()
    {
        SaveGame();
        // @TODO Zamieniæ na zmianê sceny na menu jak ju¿ je mo¿e kiedyœ bêdê mia³
        Application.Quit();
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            inventoryController.SetInventoryData(saveData.inventorySaveData);
            //buildingGrid.SetBuildingGridCells(saveData.buildingGridData);
        }
        else
        {
            SaveGame();
        }
    }
}
