using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Conditions/DefaultRoom")]
public class Condition_DefaultRoom : ConditionsV4
{
    public override int Condition(int roomID, List<RoomSpecV4> existingRooms, int maxRooms, List<RoomScrObjectV4> rooms)
    {
        int offset = 10;
        if (!rooms[roomID].isStartRoom & !rooms[roomID].isSpecialRoom)
        {
            switch (existingRooms.Count)
            {
                case 1:
                    return 0;
                default:
                    if (existingRooms.Count <= maxRooms)
                        return maxRooms / existingRooms.Count * offset;
                    else
                        return offset;
            }
        }
        else return 0;
    }
}
