using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private Tilemap oreTilemap;
    [SerializeField] private float mineRange = 2f;
    [SerializeField] private float mineInterval = 1f;
    private Coroutine miningCoroutine;
    private Vector3Int? currentOreCell;

    [SerializeField] private Camera playerCamera;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = moveInput * speed;
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        
        if (currentOreCell.HasValue)
        {
            Vector3 worldPos = oreTilemap.GetCellCenterWorld(currentOreCell.Value);
            float dist = Vector2.Distance(transform.position, worldPos);

            if (dist > mineRange)
            {
                StopMining();
                Debug.Log("Odszedłeś od rudy – przerywam kopanie.");
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
                Debug.Log("Przestałem kopać rudę.");
                return;
            }

            if (currentOreCell.HasValue)
            {
                StopMining();
            }

            currentOreCell = cellPos;
            miningCoroutine = StartCoroutine(MineLoop(cellPos, tile));
            Debug.Log("Zaczynam kopać rudę: " + tile.name);
        }
    }
    
    private System.Collections.IEnumerator MineLoop(Vector3Int cellPos, TileBase tile)
    {
        while (true)
        {
            // Inventory.Instance.AddItem(tile.name, 1);
            Debug.Log("Wydobyto rudę: " + tile.name);

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
