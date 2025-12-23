using System.Collections.Generic;
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
            questProgressData = QuestController.Instance.activeQuests,
            handingQuestIDs = QuestController.Instance.handingQuestIDs,
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void SaveAndExit()
    {
        SaveGame();
        // @TODO Zamieni� na zmian� sceny na menu jak ju� je mo�e kiedy� b�d� mia�
        Application.Quit();
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            Debug.Log("Loading Game");
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            inventoryController.SetInventoryData(saveData.inventorySaveData);
            buildingGrid.SetBuildingGridCells(saveData.buildingGridData);
            
            QuestController.Instance.LoadQuestProgress(saveData.questProgressData);
            QuestController.Instance.handingQuestIDs = saveData.handingQuestIDs;
        }
        else
        {
            SaveGame();
            
            inventoryController.SetInventoryData(new List<InventorySaveData>());
        }
    }
}
