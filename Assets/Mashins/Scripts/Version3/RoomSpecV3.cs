using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpecV3
{
    public bool complete;
    public int roomIndex;
    public List<DoorsSpec> doors;
    public RoomScrObject roomType;

    public RoomSpecV3()
    {
        doors = new();
    }

}
