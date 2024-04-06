using UnityEngine;

public class Cow : ValleyItem
{
    public Cow()
    {
        Shape = new bool[2, 1];
        Shape[0, 0] = true;
        Shape[1, 0] = true;
        
        // Set chance to 0.5f
        Chance = 0.5f;
    }
}