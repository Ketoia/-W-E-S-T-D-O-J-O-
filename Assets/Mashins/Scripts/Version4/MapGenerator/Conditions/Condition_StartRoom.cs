using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Conditions/StartRoom")]
public class Condition_StartRoom : ConditionsV4
{
    public override int Condition(int roomID, List<RoomSpecV4> existingRooms, int maxRooms, List<RoomScrObjectV4> rooms)
    {
        if (rooms[roomID].isStartRoom)
        {
            switch (existingRooms.Count)
            {
                case 1:
                    return 100;
                default:
                    return 0;
            }
        }
        else return 0;
    }
}
