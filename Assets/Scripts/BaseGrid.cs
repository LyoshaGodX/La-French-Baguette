using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGrid : MonoBehaviour
{
    // Grid dimensions
    public int width = 10;
    public int height = 10;
    public BaseCell[,] Cells;
    
    public GameObject PlaceItem(GameObject item_prefab, Vector2Int position)
    {
        GameObject item_gameobject = Instantiate(item_prefab, new Vector3(position.x, position.y, item_prefab.transform.position.z), Quaternion.identity);
        BaseItem item = item_gameobject.GetComponent<BaseItem>();
    
        if (position.x < 0 || position.y < 0 || position.x + item.Shape.GetLength(0) > width || position.y + item.Shape.GetLength(1) > height)
        {
            Debug.Log("Item is out of bounds");
            Destroy(item_gameobject);
            return null;
        }
    
        if (Cells[position.x, position.y].isOccupied)
        {
            Debug.Log("Cell is already occupied");
            Destroy(item_gameobject);
            return null;
        }
    
        for (int y = 0; y < item.Shape.GetLength(1); y++)
        {
            for (int x = 0; x < item.Shape.GetLength(0); x++)
            {
                Debug.Log("x: " + x + " y: " + y + " Shape: " + item.Shape[x, y]);
                if (item.Shape[x, y]) 
                {
                    if (Cells[position.x + x, position.y + y].isOccupied)
                    {
                        Debug.Log("Item is overlapping with another item"); 
                        Destroy(item_gameobject);
                        return null;
                    }
                }
            }
        }

        item.Position = position;
    
        for (int y = 0; y < item.Shape.GetLength(1); y++)
        {
            for (int x = 0; x < item.Shape.GetLength(0); x++)
            {
                if (item.Shape[x, y])
                {
                    Cells[position.x + x, position.y + y].isOccupied = true;
                    Cells[position.x + x, position.y + y].Occupant = item_gameobject;
                }
            }
        }

        return item_gameobject;
    }
}
