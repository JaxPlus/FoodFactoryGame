using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public enum BuildingPreviewState
    {
        POSITIVE,
        NEGATIVE,
    }
    
    public BuildingPreviewState State { get; private set; } = BuildingPreviewState.NEGATIVE;
    public BuildingData Data { get; private set; }
    public BuildingModel BuildingModel { get; private set; }
    private List<SpriteRenderer> renderers = new();
    private List<Collider> colliders = new();

    public void Setup(BuildingData data)
    {
        Data = data;
        BuildingModel = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        renderers.AddRange(BuildingModel.GetComponentsInChildren<SpriteRenderer>());
        colliders.AddRange(BuildingModel.GetComponentsInChildren<Collider>());

        foreach (var col in colliders)
        {
            col.enabled = false;
        }
        
        SetPreviewState(State);
    }

    public void ChangeState(BuildingPreviewState newState)
    {
        if (State == newState) return;
        State = newState;
        SetPreviewState(State);
    }

    public void Rotate(int rotationStep)
    {
        BuildingModel.Rotate(rotationStep);
    }
    
    private void SetPreviewState(BuildingPreviewState newState)
    {
        Color previewColor = newState == BuildingPreviewState.POSITIVE ? new Color(255, 255, 255, 80) : new Color(50, 50, 50, 200);
        
        foreach (var renderer in renderers)
        {
            renderer.color = previewColor;
        }
    }
}
