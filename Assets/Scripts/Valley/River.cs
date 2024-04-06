using UnityEngine;

public class River : ValleyItem
{
    public River()
    {
        Shape = new bool[1, 1]
        {
            { true }
        };

        Chance = 1f;
    }
}