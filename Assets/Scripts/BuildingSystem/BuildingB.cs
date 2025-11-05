using UnityEngine;

public class BuildingB : MonoBehaviour
{
    // TUTAJ TRZEBA BĘDZIE ZROBIĆ AUTOMATYZACJĘ
    public string Description => data.Description;
    public string Cost => data.Cost.ToString();
    public int ID;
    private BuildingModel model;
    private BuildingData data;

    public void Setup(BuildingData data, float rotation, int ID)
    {
        this.data = data;
        this.ID = ID;
        model = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        model.Rotate(rotation);
    }

    public BuildingData GetData()
    {
        return data;
    }
}
