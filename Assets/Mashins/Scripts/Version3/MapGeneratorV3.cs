using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorV3 : MonoBehaviour
{
    public List<RoomScrObject> roomsList;
    public MapScrObject startRoom;
    int test = 3;
    public List<RoomSpecV3> rooms = new List<RoomSpecV3>();

    public int seed;
    int roomNumber = 1;
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(seed);

        int maxRoomAmount = GetRoomMap(startRoom);
        CreateRoom(0, GetDoorAmount(startRoom), ref roomNumber);
        for (int i = 1; i < rooms.Count; i++)
        {
            //Debug.Log(" dzia³a tutaj");
            CreateRoom(i, test, ref roomNumber);
        }

        //for (int i = 0; i < rooms.Count; i++)
        //{
        //    Debug.Log(rooms[i].roomIndex + " " + rooms.Count + " " + rooms[i].doors.Count);
        //}
    }

    //Create or overwrite room
    public void CreateRoom(int roomIndex, int doorsCount, ref int roomOut)
    {
        RoomSpecV3 room = new RoomSpecV3();
        room.roomIndex = roomIndex;
        int testCount = 0;
        test--;
        if (rooms.Count > 0)
        {
            testCount = rooms[roomIndex].doors.Count;
        }
        //Check: are in this room doors? Add previous doors to new room
        for (int i = 0; i < testCount; i++)
        {
            room.doors.Add(rooms[roomIndex].doors[i]);
        }
        // Add new doors
        for (int i = 0; i < doorsCount - testCount; i++, roomOut++)
        {
            DoorsSpec door = new DoorsSpec();
            door.roomOutIndex = roomOut;
            room.doors.Add(door);
        }
        //Set room
        if (roomIndex < rooms.Count)
        {
            rooms[roomIndex] = room;
        }
        else
            rooms.Add(room);

        //Add neighbour rooms and add door to previous room
        for (int i = 1; i < room.doors.Count; i++)
        {
            RoomSpecV3 neighbourRoom = new RoomSpecV3();
            neighbourRoom.roomIndex = room.doors[i].roomOutIndex;
            DoorsSpec door = new DoorsSpec();
            door.roomOutIndex = roomIndex;
            neighbourRoom.doors.Add(door);
            rooms.Add(neighbourRoom);
        }
    }

    public int GetRoomMap(MapScrObject mapSet)
    {
        if (mapSet.isRandomRoomAmount)
        {
            return Random.Range(1, 100);
        }
        else
            return mapSet.roomAmount;
    }

    public int GetDoorAmount(MapScrObject mapset)
    {
        if (mapset.randomAmountOfDoors)
        {
            return Random.Range(1, 10);
        }
        else if (mapset.randomDirOfDoors)
        {
            return mapset.doorsAmount;
        }
        else return (int)(mapset.doorsAmountDir.x + mapset.doorsAmountDir.y + mapset.doorsAmountDir.z + mapset.doorsAmountDir.w);
    }

    public int indexOfScrRoom(List<RoomScrObject> list, List<RoomSpecV3> rooms)
    {
        List<RoomScrObject> newList = new List<RoomScrObject>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].specialRoom)
            {
                newList.Add(list[i]);
            }
        }
        return 0;
    }

    public RoomScrObject ChooseRoom(List<RoomScrObject> list, List<RoomSpecV3> listOfAllRooms, List<Conditions> conditions, int roomIndex, int maxRoomAmount)
    {
        List<RoomScrObject> newlist = new List<RoomScrObject>();
        for (int i = 0; i < list.Count; i++)
        {
            bool checkAll = true;
            for (int x = 0; x < conditions.Count; x++)
            {
                if (!conditions[x].Condition(list[i], listOfAllRooms))
                {
                    checkAll = false;
                }
            }
            if (checkAll)
            {
                newlist.Add(list[i]);
            }
        }
        return null;

        //Dictionary<RoomSpecV3, RoomScrObject> testx2 = new Dictionary<RoomSpecV3, RoomScrObject>();
    }
}
