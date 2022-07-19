using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New MapSet", menuName = "MapSetting")]
public class MapScrObject : ScriptableObject
{
    public bool isRandomRoomAmount = true;
    public int roomAmount;
    public bool randomAmountOfDoors = true;
    public int doorsAmount;
    public bool randomDirOfDoors = true;
    public Vector4 doorsAmountDir;
}
