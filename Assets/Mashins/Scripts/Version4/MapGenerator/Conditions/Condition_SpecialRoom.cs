using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Conditions/SpecialRoom")]
public class Condition_SpecialRoom : ConditionsV4
{
    MethodsForConditions methods = new MethodsForConditions();
    public override int Condition(int roomID, List<RoomSpecV4> existingRooms, int maxRooms, List<RoomScrObjectV4> rooms)
    {
        int numberOfRowToCheck = 2;
        if (rooms[roomID].isSpecialRoom)
        {
            switch (existingRooms.Count)
            {
                case 1:
                    return 0;
                default:
                    if (methods.IsSpecialRoomInParentRows(existingRooms, numberOfRowToCheck, rooms))
                    {

                        Debug.Log("Sepcial1");
                        return 0;
                    }
                    if (methods.IsAllRooms(roomID, existingRooms, rooms))
                    {

                        Debug.Log("Sepcial2");
                        return 0;
                    }
                    //Debug.Log("Sepcial" + existingRooms.Count / maxRooms * 100);
                    return existingRooms.Count/ maxRooms * 100;
            }
        }
        else return 0;
    }

   
}
