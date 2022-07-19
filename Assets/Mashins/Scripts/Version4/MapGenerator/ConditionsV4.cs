using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionsV4 : ScriptableObject
{
    public abstract int Condition(int roomID, List<RoomSpecV4> existingRooms, int roomCount, List<RoomScrObjectV4> rooms);
}
