using System;
using UnityEngine;

public class OutputPoint : MonoBehaviour
{
    [SerializeField] public BuildingB building;
    public LayerMask buildingMask;

    private void FixedUpdate()
    {
        if (building == null) return;
        
        Vector2 position = transform.position;
        Vector2 size = GetComponent<BoxCollider2D>().size;
        
        Collider2D hit = Physics2D.OverlapBox(position, size, 0f, buildingMask);

        if (hit != null && hit.GetComponentInParent<BuildingB>().CompareTag("Building") && hit.GetComponentInParent<BuildingB>() != null)
        {
            BuildingB output = hit.gameObject.GetComponentInParent<BuildingB>();

            if (output.inputInventory.Count < 0)
                return;

            building.SetOutput(output);
        }
    }
}
