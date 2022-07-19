using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Conditions : ScriptableObject
{
    public abstract bool Condition(RoomScrObject room, List<RoomSpecV3> roomList);
}
