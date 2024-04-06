using UnityEngine;


public abstract class BaseCell : MonoBehaviour
{
    public bool isOccupied { get; set; }

    public GameObject Occupant { get; set; }
}