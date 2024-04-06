using UnityEngine;

public class ValleyGrid : BaseGrid
{
    public GameObject cellPrefab;
    public GameObject cowPrefab;
    public GameObject riverPrefab;

    void Start()
    {
        CreateGrid();
    }
    
    public void CreateGrid()
    {
        Cells = new ValleyCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newCellInstance = Instantiate(cellPrefab, new Vector3(x, y, cellPrefab.transform.position.z), Quaternion.identity);
                ValleyCell newCell = newCellInstance.AddComponent<ValleyCell>();

                newCell.isOccupied = false;
                newCell.Occupant = null;

                Cells[x, y] = newCell;
            }
        }
    }
    
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2Int cellPosition = new Vector2Int(Mathf.FloorToInt(mousePosition.x), Mathf.CeilToInt(mousePosition.y));
        //    Debug.Log("Cell position: " + cellPosition);
        //    GameObject newObject = Instantiate(cowPrefab, new Vector3(cellPosition.x, cellPosition.y, cowPrefab.transform.position.z), Quaternion.identity);
        //    PlaceItem(newObject, cellPosition);
        //}
    }
}