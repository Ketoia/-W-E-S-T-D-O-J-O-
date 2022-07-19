using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodsForConditions
{
    public bool IsAllRooms(int roomID, List<RoomSpecV4> existingRooms, List<RoomScrObjectV4> rooms)
    {
        int value = 0;

        for (int i = 0; i < existingRooms.Count; i++)
        {
            if (existingRooms[i].roomTypeID == roomID)
                value++;
        }
        if (value >= rooms[roomID].roomsAmount)
        {
            if (roomID == 0)
            {
                Debug.Log("Istnieje::Tak " + rooms[roomID].name);
            }
            return true;
        }
        else
        {
            if (roomID == 0)
            {
                Debug.Log("Istnieje::Nie " + rooms[roomID].name);
            }
            return false;
        }
    }

    public bool IsSpecialRoomInParentRows(List<RoomSpecV4> existingRooms, int numberOfRowToCheck, List<RoomScrObjectV4> rooms)
    {
        List<RoomSpecV4> newlist = ListOfAllInBranch(existingRooms);
        for (int i = 1; i < numberOfRowToCheck; i++)
        {
            if (i < newlist.Count)
            {
                if (rooms[newlist[i].roomTypeID].isSpecialRoom || rooms[newlist[i].roomTypeID].isStartRoom)
                {

                    //Debug.Log(" metoda: " + newlist.Count + " " + newlist[0].roomIndex + " " + newlist[i].roomType.name + " " +newlist[i].roomType.isSpecialRoom);
                    return true;
                }
            }
            else
                return false;
        }
        return false;
    }

    public List<RoomSpecV4> ListOfAllInBranch(List<RoomSpecV4> existingRooms)
    {
        List<RoomSpecV4> newList = new List<RoomSpecV4>();
        newList.Add(existingRooms[existingRooms.Count - 1]);
        while (newList[newList.Count - 1].prevRoomIndex.x != -1)
        {
            for (int i = 0; i < existingRooms.Count; i++)
            {
                if (existingRooms[i].roomIndex == newList[newList.Count - 1].prevRoomIndex)
                {
                    newList.Add(existingRooms[i]);
                    break;
                }
            }
        }
        return newList;
    }
}
