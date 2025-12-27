using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Miner : BuildingB
{
    public LayerMask oreMask;
    [SerializeField] private float miningSpeed = 2f;
    public TileBase tileToMine;
    private InventoryController inventoryController;

    void Start()
    {
        gameObject.transform.Rotate(new Vector3(0, 0, model.Rotation));
        model.transform.Find("Wrapper").SetPositionAndRotation(transform.position, new Quaternion(0, 0, 0, 0));
        
        inventoryController = InventoryController.Instance;

        InvokeRepeating(nameof(Mine), 0, miningSpeed);
    }

    void Mine()
    {
        if (tileToMine == null || output == null) return;

        GameObject ore = inventoryController.FindOreByName(tileToMine.name);

        if (ore == null) return;

        output.AddToInventory(ore);
    }

    void FixedUpdate()
    {
        Vector2 position = transform.position;
        Vector2 size = GetComponent<BoxCollider2D>().size;

        Collider2D hit = Physics2D.OverlapBox(position, size, 0f, oreMask);

        if (hit != null)
        {
            Tilemap oreTilemap = hit.GetComponent<Tilemap>();

            Vector3Int cellPos = oreTilemap.WorldToCell(position);
            tileToMine = oreTilemap.GetTile(cellPos);
        }
    }
}
