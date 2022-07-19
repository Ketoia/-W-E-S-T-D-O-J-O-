using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomSpecV4
{
    //public int index;
    //x = row; y = roomNumber;
    public Vector2 roomIndex;
    public Vector2 prevRoomIndex;
   // public int roomRow;
    public List<DoorsSpecV4> doors;

    public int roomTypeID = -1;
    public RoomSpecV4()
    {
        doors = new();
        
    }

    public GameObject tilemapGameObject;
    public Tilemap tilemap;
}
