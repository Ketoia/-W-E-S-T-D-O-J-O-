using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Room", menuName = "Rooms")]
public class RoomScrObjectV4 : DoorsScrObject
{
    //public string roomName;
    public bool randomAmountOfRooms = true;
    public int roomsAmount;
    public bool isSpecialRoom;
    public bool isStartRoom;

    public List<Tilemap> tilemaps;
}
