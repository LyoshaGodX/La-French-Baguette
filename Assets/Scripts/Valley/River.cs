using UnityEngine;

public class River : ValleyItem
{
    public River()
    {
        // Define the shape of the river
        Shape = new bool[1, 1]
        {
            { true }
        };
        
        Chance = 1f;
    }
}