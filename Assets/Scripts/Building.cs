using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Building : MonoBehaviour, IPointerClickHandler
{
    public int ID;
    public string Name;
    public BuildingCategory Category;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("trzeba zrobiæ budowanko");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Otworzyæ UI je¿eli jest postawione");
        }
    }
}
