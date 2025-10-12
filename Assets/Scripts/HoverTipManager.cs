using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoverTipManager : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public Image ingredients;
    public RectTransform tipWindow;
    
    public static Action<string, List<GameObject>, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }
    
    void Start()
    {
        HideTip();
    }

    private void ShowTip(string text, List<GameObject> items, Vector2 mousePos)
    {
        itemName.text = text;
        
        foreach (GameObject item in items)
        {
            Instantiate(item.GetComponent<Image>(), ingredients.transform);
        }
        
        tipWindow.sizeDelta = new Vector2(
            itemName.preferredWidth + 120 > 300 ? 300 : itemName.preferredWidth + 120, 
            itemName.preferredHeight + 70
            );
        ingredients.GetComponent<RectTransform>().sizeDelta = tipWindow.sizeDelta;
        tipWindow.gameObject.SetActive(true);
        tipWindow.transform.position = new Vector2(mousePos.x + tipWindow.sizeDelta.x - 38, mousePos.y);
    }

    private void HideTip()
    {
        itemName.text = null;

        foreach (Transform child in ingredients.transform)
        {
            Destroy(child.gameObject);
        }
        
        tipWindow.gameObject.SetActive(false);
    }
}
