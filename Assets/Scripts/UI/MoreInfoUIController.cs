using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoreInfoUIController : MonoBehaviour
{
    public GameObject BuildingName;
    public GameObject BuildingDescription;
    public GameObject BuildingCost;
    public GameObject Hotbar;
    public GameObject HotbarBuildingInfo;
    public GameObject CostInfoPrefab;

    void Start()
    {
        gameObject.SetActive(false);
    }
    
    public void CloseMoreInfo()
    {
        foreach (Transform obj in BuildingCost.transform)
        {
            Destroy(obj.gameObject);
        }
        
        Hotbar.SetActive(true);
        HotbarBuildingInfo.SetActive(true);
        gameObject.SetActive(false);
    }

    public void FillInInfo(string buildingName, string buildingDescription, List<GameObject> buildingCost)
    {
        BuildingName.GetComponent<TMP_Text>().text = buildingName;
        BuildingDescription.GetComponent<TMP_Text>().text = buildingDescription;

        foreach (GameObject obj in buildingCost)
        {
            Item item = obj.GetComponent<Item>();
            CostInfoPrefab.GetComponent<Image>().sprite = item.GetComponent<Image>().sprite;
            Instantiate(CostInfoPrefab, BuildingCost.transform);
        }
    }
}
