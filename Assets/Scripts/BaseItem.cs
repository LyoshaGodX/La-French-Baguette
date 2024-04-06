using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    public Vector2Int Position { get; set; }
    public bool[,] Shape { get; set; }
    // Rotation of the shape 1 = 90 degrees, 2 = 180 degrees, 3 = 270 degrees
    public int Rotation { get; set; }
    public float Chance { get; set; }
}