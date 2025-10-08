using UnityEngine;
using UnityEngine.InputSystem;

public class Building : MonoBehaviour
{
    public int ID;
    public string Name;
    public BuildingCategory Category;

    public void OnClick()
    {
        Debug.Log("trzeba zrobiæ budowanko");
    }
}
