using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapScrObjectV4))]
public class InspectorEditorOfMapV4 : Editor
{
    MethodsForInspectorV4 methods = new MethodsForInspectorV4();
    public override void OnInspectorGUI()
    {
       // base.OnInspectorGUI();
        MapScrObjectV4 mapScrObject = target as MapScrObjectV4;

       // methods.CheckIsRandomOfRooms(ref mapScrObject.isRandomRoomAmount, ref mapScrObject.roomAmount, "rooms");
        methods.CheckIsRandomOfDoors(ref mapScrObject.randomAmountOfDoors, ref mapScrObject.doorsAmount, ref mapScrObject.randomDirOfDoors, ref mapScrObject.doorsAmountDir, "doors");
    }

}
