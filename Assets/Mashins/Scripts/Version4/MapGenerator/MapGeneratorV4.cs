using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorV4 : MonoBehaviour
{
    public List<RoomScrObjectV4> rooms = new List<RoomScrObjectV4>();
    public List<ConditionsV4> conditions = new List<ConditionsV4>();

    List<RoomSpecV4> existingRooms = new List<RoomSpecV4>();

    List<DoorsSpecV4> remainingNCD = new List<DoorsSpecV4>();
    float roomRow = 0;

    public RoomGenerator roomGenerator;

    public void GenerateWorld(int seed, int maxRooms)
    {
        Random.InitState(seed);
 
        CreateRows(maxRooms);

        for (int i = 0; i < existingRooms.Count; i++)
        {
            Debug.Log(rooms[existingRooms[i].roomTypeID].name);
        }

        roomGenerator = gameObject.GetComponent<RoomGenerator>();
        roomGenerator.GenerateRoom(ref existingRooms, rooms);
    }

    public void CreateRows(int maxRooms)
    {
        for (int i = 0; i <= roomRow; i++)
        {
            float roomNumber = 0;
            if (i == 0)
            {
                RoomSpecV4 newRoom = new RoomSpecV4();
                newRoom.roomIndex.x = i;
                newRoom.prevRoomIndex.x = -1;

                existingRooms.Add(newRoom);
                newRoom.roomTypeID = CheckConditions(maxRooms, existingRooms);
                newRoom.doors = CreateDoors(newRoom, ref roomNumber, maxRooms, 1, existingRooms);

                existingRooms[existingRooms.Count - 1] = newRoom;
            }
            else
            {
                int variable = remainingNCD.Count;
                for (int x = 0; x < variable; x++)
                {
                    RoomSpecV4 newRoom = new RoomSpecV4();
                    newRoom.roomIndex = remainingNCD[x].roomOutIndex;
                    newRoom.prevRoomIndex = remainingNCD[x].roomIndex;

                    existingRooms.Add(newRoom);
                    newRoom.roomTypeID = CheckConditions(maxRooms, existingRooms);
                    newRoom.doors = CreateDoors(newRoom, ref roomNumber, maxRooms, variable, existingRooms);

                    existingRooms[existingRooms.Count - 1] = newRoom;
                }
                remainingNCD.RemoveRange(0, variable);
            }

        }
        Debug.Log(existingRooms.Count);
    }

    public List<DoorsSpecV4> CreateDoors(RoomSpecV4 room, ref float roomNumber, int maxRooms, int roomsInRow, List<RoomSpecV4> list)
    {
        List<DoorsSpecV4> newListOfDoors = new List<DoorsSpecV4>();
        //Debug.Log(room.roomType.name + " " + room.roomType.isSpecialRoom);
        int value = rooms[room.roomTypeID].CalculateDoorAmount(list.Count + remainingNCD.Count, CalculateRemainingRooms(rooms, list), maxRooms);
        //Debug.Log("doorsAmout " + value);
        for (int i = 0; i < value; i++)
        {
            DoorsSpecV4 door = new DoorsSpecV4();
            door.roomIndex = room.roomIndex;
            if (i == 0 & room.roomIndex.x > 0)
            {
                door.roomOutIndex = room.prevRoomIndex;
            }
            else
            {
                roomRow = room.roomIndex.x + 1;
                door.roomOutIndex.x = roomRow;
                door.roomOutIndex.y = roomNumber;
                roomNumber++;
                remainingNCD.Add(door);
            }
            newListOfDoors.Add(door);
        }
        return newListOfDoors;
    }

    
    public int CheckConditions(int maxRooms, List<RoomSpecV4> list)
    {
        //RoomLimits[] limits = new RoomLimits[rooms.Count];
        List<RoomLimits> limits = new List<RoomLimits>();
        int sumOfWeight = 0;
        //Debug.Log(existingRooms.Count);
        for (int x = 0; x < rooms.Count; x++)
        {
            RoomLimits roomLimit = new RoomLimits();
            roomLimit.roomTypeID = x;
            roomLimit.limits.x = sumOfWeight;
            int weight = 0;
            for (int y = 0; y < conditions.Count; y++)
            {
                weight += conditions[y].Condition(x, list, maxRooms, rooms);

            }
            sumOfWeight += weight;
            roomLimit.limits.y = sumOfWeight;
            if (roomLimit.limits.y - roomLimit.limits.x > 0)
            {
                limits.Add(roomLimit);
            }
        }
        int result = Random.Range(0, sumOfWeight);

        for (int i = 0; i < limits.Count; i++)
        {
            if (i == limits.Count - 1)
            {
                if (result >= limits[i].limits.x && result <= limits[i].limits.y)
                {
                    return limits[i].roomTypeID;
                }
            }
            else if (result >= limits[i].limits.x && result < limits[i].limits.y)
            {
                return limits[i].roomTypeID;
            }
        }
        return 0;
    }
    public int CalculateRemainingRooms(List<RoomScrObjectV4> allTypes, List<RoomSpecV4> existingRooms)
    {
        int remainingRooms = 0;
        for (int i = 0; i < allTypes.Count; i++)
        {
            if (allTypes[i].isSpecialRoom)
            {
                remainingRooms += allTypes[i].roomsAmount;
            }
        }
        for (int i = 0; i < allTypes.Count; i++)
        {
            if (allTypes[i].isSpecialRoom)
            {
                int counter = 0;
                for (int x = 0; x < existingRooms.Count; x++)
                {
                    if (rooms[existingRooms[x].roomTypeID] == allTypes[i])
                        counter++;
                }
                remainingRooms -= counter;

            }
        }
        //Debug.Log(remainingRooms);
        return remainingRooms;
    }
    public class RoomLimits
    {
        public Vector2 limits;
        public int roomTypeID;
    }
}
