using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private Transform originalParent;
    private Canvas canvas;
    private InventoryPanel inventoryPanel;
    public BaseGrid Grid;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        inventoryPanel = GetComponentInParent<InventoryPanel>();
        Grid = FindObjectOfType<BaseGrid>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = originalPosition;
        transform.SetParent(originalParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        
        // Get slot index based on the parent's Image index in the slots list
        int slotIndex = inventoryPanel.slots.IndexOf(originalParent.GetComponent<Image>());
        
        // Get the item from the inventory panel
        GameObject itemPrefab = inventoryPanel.items[slotIndex];

        if (itemPrefab != null)
        {
            // Convert the mouse position to world coordinates
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // Convert the position to grid coordinates
            Vector2Int gridPosition = new Vector2Int(Mathf.FloorToInt(mousePosition.x), Mathf.CeilToInt(mousePosition.y));
            
            // Try to place the item on the grid
            GameObject item = Grid.PlaceItem(itemPrefab, gridPosition);

            // If the item was successfully placed (it should exist and not be destroyed by PlaceItem)
            if (item != null)
            {
                // Reset the slot's child to an empty state
                inventoryPanel.ClearSlot(slotIndex);

                // Set the item in the items list to null
                inventoryPanel.items[slotIndex] = null;
                
                Debug.Log("Item placed successfully at " + gridPosition);
            }
            else
            {
                Debug.Log("Item placement failed at " + gridPosition);
            }
        }
    }
}