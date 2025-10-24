using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarBuildingInfo : MonoBehaviour
{
    [SerializeField] private GameObject buildingIcon;
    [SerializeField] private TextMeshProUGUI buildingName;
    [SerializeField] private TextMeshProUGUI buildingDesc;
    [SerializeField] private GameObject buildingCost;
    
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
    }

    public void HideBuildingInfo()
    {
        gameObject.SetActive(false);
    }
}
