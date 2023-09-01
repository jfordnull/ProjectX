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
    /*
     * (x,y+size.y) *----------------*
     *              -                -
     *              -                -
     *              -                -
     *              -                -
     *              -                -
     *              *----------------* (x+size.x,y)  
     */
    /*--------------------------------------------------*/

    public static bool IsIntersecting(Room a, Room b)
    {
        return 
            //Horizontal separation check
            !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) ||
            ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x) ||
            //Vertical separation check
            (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) ||
            ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
    }
}
