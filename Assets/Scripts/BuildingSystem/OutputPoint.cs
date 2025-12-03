using UnityEngine;

public class OutputPoint : MonoBehaviour
{
    [SerializeField] public BuildingB building;

    private void Update()
    {
        Vector2 position = transform.position;
        Vector2 size = GetComponent<BoxCollider2D>().size;

        if (!building)
            return;
        
        Collider2D hit = Physics2D.OverlapBox(position, size, 0f);
        if (hit != null && hit.CompareTag("Building") && hit.gameObject.GetComponentInParent<BuildingB>() != null)
        {
            BuildingB output = hit.gameObject.GetComponentInParent<BuildingB>();

            if (output.inputInventory.Count < 0)
                return;

            building.SetOutput(output);
        }
    }
}
