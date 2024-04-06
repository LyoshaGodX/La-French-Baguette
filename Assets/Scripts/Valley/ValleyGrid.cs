using UnityEngine;

public class ValleyGrid : BaseGrid
{
    public GameObject cellPrefab;
    public GameObject cowPrefab;
    public GameObject riverPrefab;
    public ValleyCell[,] ValleyCells;
    
    void Start()
    {
        CreateGrid();
        PrintOccupied();
    }
    
    public void CreateGrid()
    {
        ValleyCells = new ValleyCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newCellInstance = Instantiate(cellPrefab, new Vector3(x, y, cellPrefab.transform.position.z), Quaternion.identity);
                ValleyCell newCell = new ValleyCell();
                
                newCell.isOccupied = false;
                newCell.Occupant = null;
                
                ValleyCells[x, y] = newCell;
            }
        }
    }
    
    public void PrintOccupied()
    {
        string output = "";
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                output += ValleyCells[x, y].isOccupied;
            }
            output += "\n";
        }
        Debug.Log(output);
    }
    

    public void PlaceItem(GameObject item_gameobject, Vector2Int position)
    {
        ValleyItem item = item_gameobject.GetComponent<ValleyItem>();
        
        if (position.x < 0 || position.y < 0 || position.x + item.Shape.GetLength(0) > width || position.y + item.Shape.GetLength(1) > height)
        {
            Debug.Log("Item is out of bounds");
            Destroy(item_gameobject); // TODO: Убрать, временное решение для появляющихся из воздуха предметов, а не берущихся из инвентаря
            return;
        }
        
        if (ValleyCells[position.x, position.y].isOccupied)
        {
            Debug.Log("Cell is already occupied");
            Destroy(item_gameobject); // TODO: Убрать, временное решение для появляющихся из воздуха предметов, а не берущихся из инвентаря
            return;
        }
        
        for (int y = 0; y < item.Shape.GetLength(1); y++)
        {
            for (int x = 0; x < item.Shape.GetLength(0); x++)
            {
                Debug.Log("x: " + x + " y: " + y + " Shape: " + item.Shape[x, y]);
                if (item.Shape[x, y]) 
                {
                    if (ValleyCells[position.x + x, position.y + y].isOccupied)
                    {
                        Debug.Log("Item is overlapping with another item"); 
                        Destroy(item_gameobject);
                        return;
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
                    ValleyCells[position.x + x, position.y + y].isOccupied = true;
                    ValleyCells[position.x + x, position.y + y].Occupant = item_gameobject;
                }
            }
        }
    }
    
    

    
    // Create an update loop to check for input and place cow on mouse click at the place of the click
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int cellPosition = new Vector2Int(Mathf.FloorToInt(mousePosition.x), Mathf.CeilToInt(mousePosition.y));
            Debug.Log("Cell position: " + cellPosition);
            GameObject newObject = Instantiate(cowPrefab, new Vector3(cellPosition.x, cellPosition.y, cowPrefab.transform.position.z), Quaternion.identity);
            PlaceItem(newObject, cellPosition);
            PrintOccupied();
        }
    }
}