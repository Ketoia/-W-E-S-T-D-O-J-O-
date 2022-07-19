using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpecV2 : ScriptableObject
{
    public int maxDistance;
    public int distanceFromOthers;
    public int distanceFromSameTypes;

    [HideInInspector]
    public Vector2 roomPosition;
    [HideInInspector]
    public int roomIndex;

    [HideInInspector]
    List<Vector2> neighboursPos;
    [HideInInspector]
    List<int> neighbourIndex;
}
