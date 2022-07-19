using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Room", menuName = "Rooms")]
public class RoomScrObject : ScriptableObject
{
    //public string roomName;
    public bool randomAmountOfRooms = true;
    public int roomsAmount;
    public bool specialRoom;
    public bool randomAmountOfDoors = true;
    public int doorsAmount;
    public bool randomDirOfDoors = true;
    public Vector4 doorsAmountDir;

}
