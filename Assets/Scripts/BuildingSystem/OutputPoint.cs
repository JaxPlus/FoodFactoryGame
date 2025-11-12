using UnityEngine;

public class OutputPoint : MonoBehaviour
{
    // Gdzie to jest na gridzie
    public int x;
    public int y;

    private void Update()
    {
        Vector2 position = transform.position;
        Vector2 size = GetComponent<BoxCollider2D>().size;

        Collider2D hit = Physics2D.OverlapBox(position, size, 0f);
        if (hit != null && hit.tag == "Model" && hit.gameObject.GetComponentInParent<BuildingB>() != null)
        {
            BuildingB building = hit.gameObject.GetComponentInParent<BuildingB>();

            if (building.inputInventory.Count < 0)
                return;

            Debug.Log(building.ID);
        }
    }
}
