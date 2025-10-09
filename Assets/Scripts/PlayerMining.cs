using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMining : MonoBehaviour
{
    [SerializeField] private Tilemap oreTilemap;
    [SerializeField] private float mineRange = 2f;
    [SerializeField] private float mineInterval = 1f;
    [SerializeField] private Camera playerCamera;
    private Coroutine miningCoroutine;
    private Vector3Int? currentOreCell;
    private InventoryController inventoryController;

    private void Start()
    {
        inventoryController = FindFirstObjectByType<InventoryController>(); ;
    }

    private void FixedUpdate()
    {
        if (currentOreCell.HasValue)
        {
            Vector3 worldPos = oreTilemap.GetCellCenterWorld(currentOreCell.Value);
            float dist = Vector2.Distance(transform.position, worldPos);
            
            if (dist > mineRange)
            {
                StopMining();
                Debug.Log("To far away from ore.");
            }
        }
    }

    public void Mine(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector3 mouseWorldPos = playerCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector3Int cellPos = oreTilemap.WorldToCell(mouseWorldPos);
        TileBase tile = oreTilemap.GetTile(cellPos);
        
        if (tile != null)
        {
            if (currentOreCell.HasValue && currentOreCell.Value == cellPos)
            {
                StopMining();
                Debug.Log("Stop mining ore.");
                return;
            }

            if (currentOreCell.HasValue)
            {
                StopMining();
            }

            currentOreCell = cellPos;
            
            Vector3 worldPos = oreTilemap.GetCellCenterWorld(currentOreCell.Value);
            float dist = Vector2.Distance(transform.position, worldPos);
            
            if (dist > mineRange)
            {
                StopMining();
                Debug.Log("To far away from ore.");
                return;
            }
            
            Debug.Log("Mining ore: " + tile.name);
            miningCoroutine = StartCoroutine(MineLoop(cellPos, tile));
        }
    }

    private System.Collections.IEnumerator MineLoop(Vector3Int cellPos, TileBase tile)
    {
        while (true)
        {
            Debug.Log("Got ore: " + tile.name);
            GameObject item = inventoryController.FindOreByName(tile.name);
            if (item != null)
            {
                inventoryController.AddItem(item);
            }

            yield return new WaitForSeconds(mineInterval);
        }
    }

    private void StopMining()
    {
        if (miningCoroutine != null)
        {
            StopCoroutine(miningCoroutine);
            miningCoroutine = null;
        }
        currentOreCell = null;
    }
}
