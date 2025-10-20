using UnityEngine;

public class BuildingB : MonoBehaviour
{
    // TUTAJ TRZEBA BĘDZIE ZROBIĆ AUTOMATYZACJĘ
    public string Description => data.Description;
    public string Cost => data.Cost.ToString();
    private BuildingModel model;
    private BuildingData data;

    public void Setup(BuildingData data, float rotation)
    {
        this.data = data;
        model = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        model.Rotate(rotation);
    }
}
