using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarBuildingInfo : MonoBehaviour
{
    [SerializeField] private GameObject buildingIcon;
    [SerializeField] private TextMeshProUGUI buildingName;
    [SerializeField] private TextMeshProUGUI buildingDesc;
    [SerializeField] private GameObject moreBuildingInfo;
    [SerializeField] private GameObject hotbar;
    private List<GameObject> buildingCostList;
    
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowBuildingInfo(BuildingData buildingData)
    {
        gameObject.SetActive(true);

        buildingName.text = buildingData.Name;
        buildingDesc.text = buildingData.Description;
        buildingIcon.GetComponent<Image>().sprite = buildingData.Model.gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
        buildingCostList = buildingData.Cost;
    }

    public void HideBuildingInfo()
    {
        gameObject.SetActive(false);
    }

    public void ShowMoreBuildingInfo()
    {
        moreBuildingInfo.GetComponent<MoreInfoUIController>().FillInInfo(buildingName.text, buildingDesc.text, buildingCostList);
        moreBuildingInfo.SetActive(true);
        hotbar.SetActive(false);
        gameObject.SetActive(false);
    }
}
