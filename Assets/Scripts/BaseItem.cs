using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    public Vector2Int Position { get; set; }
    public bool[,] Shape { get; set; }
    public int Rotation { get; set; }
    public float Chance { get; set; }
    public Sprite UIImage;
}