using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Random = System.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int levelGridSize, maxRoomSize;
    [SerializeField] int roomCount, randomSeed = 0;
    [SerializeField] GameObject floorTile, wallLeft, wallRight, wallUp, wallDown;
    [SerializeField] Material floorMaterial, wallMaterial;
    enum CellType { None, Room, Hallway }

    Random random;
    Grid2D<CellType> levelGrid;
    List<Room> rooms;
    //Add Bowyer-Watson class instance here
    //Add HashSet of edges here

    void Start()
    {
        Generate();
    }

    /*--------------------------------------------------*/

    void Generate()
    {
        random = new Random(randomSeed);
        levelGrid = new Grid2D<CellType>(levelGridSize, Vector2Int.zero);
        rooms = new List<Room>();

        PlaceRooms();
    }

    void PlaceRooms()
    {
        for (int i = 0; i < roomCount; i++)
        {
            bool flag = false;

            Vector2Int location = new Vector2Int(
                random.Next(0,levelGridSize.x),
                random.Next(0,levelGridSize.y));
            Vector2Int roomSize = new Vector2Int(
                random.Next(1, maxRoomSize.x),
                random.Next(1, maxRoomSize.y));

            Room newRoom = new Room(location, roomSize);

            //Check buffer for intersection against other rooms
            Room buffer = new Room(location + new Vector2Int(-1, -1),
                roomSize + new Vector2Int(2, 2));
            foreach (Room room in rooms)
            {
                if (Room.IsIntersecting(room, buffer))
                {
                    Debug.Log("Failed to add room. Placement conflict");
                    flag = true; break;
                }
            }

            //Check that new room is in bounds
            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= levelGridSize.x || 
                newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= levelGridSize.y)
            {
                Debug.Log("Failed to add room. Out of bounds");
                flag = true;
            }

            if (!flag)
            {
                rooms.Add(newRoom);
                DrawRoom(newRoom.bounds.position, newRoom.bounds.size);
                Debug.Log("Added room" + rooms.Count + newRoom.bounds.position + "size: " + newRoom.bounds.size);
            }

            if (flag)
            {
                i--;
            }
        }
    }

    /*--------------------------------------------------*/

    void DrawRoom(Vector2Int location, Vector2Int size)
    {
        for(int i = 0; i < size.x; i++)
        {
            /*              -------------              */
            /*              -           -              */
            /*              -           -              */
            /*              -           -              */
            /*              *************              */
            /*                                         */
            PlaceWallTile(location + new Vector2Int(i, 0), wallDown);
            /*              *************              */
            /*              -           -              */
            /*              -           -              */
            /*              -           -              */
            /*              -------------              */
            /*                                         */
            PlaceWallTile(location + new Vector2Int(i, size.y), wallUp);

            for (int j = 0; j < size.y; j++)
            {
                /*              *------------              */
                /*              *           -              */
                /*              *           -              */
                /*              *           -              */
                /*              *------------              */
                /*                                         */
                PlaceWallTile(location + new Vector2Int(0, j), wallLeft);
                /*              ------------*              */
                /*              -           *              */
                /*              -           *              */
                /*              -           *              */
                /*              ------------*              */
                /*                                         */
                PlaceWallTile(location + new Vector2Int(size.x, j), wallRight);

                PlaceFloorTile(location + new Vector2Int(i, j));
            }
        }
    }

    void DrawHallway()
    {

    }

    /*--------------------------------------------------*/

    void PlaceFloorTile(Vector2Int location)
    {
        GameObject floor = Instantiate(floorTile, new Vector3(location.x, 0, location.y), Quaternion.identity);
    }

    void PlaceWallTile(Vector2Int location, GameObject wall)
    {
        GameObject wall1 = Instantiate(wall, new Vector3(location.x, 0, location.y), Quaternion.identity);
        GameObject wall2 = Instantiate(wall, new Vector3(location.x, 1, location.y), Quaternion.identity);
    }
}
