using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    /*--------------------------------------------------*/
    public RectInt bounds;
    public Room(Vector2Int gridLocation, Vector2Int size)
    {
        bounds = new RectInt(gridLocation, size);
    }
    /*--------------------------------------------------*/
}
