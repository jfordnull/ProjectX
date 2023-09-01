using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D<T>
{
    /*--------------------------------------------------*/

    T[] gridElements;
    public Vector2Int GridSize { get; private set; }
    public Vector2Int GridOffset { get; private set; }

    public Grid2D(Vector2Int size, Vector2Int offset)
    {
        GridSize = size;
        GridOffset = offset;
        gridElements = new T[size.x * size.y];
    }

    /*--------------------------------------------------*/
}
