using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapScrObject))]
public class InspectorEditorOfMap : Editor
{
    MethodsForInspector methods = new MethodsForInspector();
    public override void OnInspectorGUI()
    {
        MapScrObject mapScrObject = target as MapScrObject;

        methods.CheckIsRandomOfRooms(ref mapScrObject.isRandomRoomAmount, ref mapScrObject.roomAmount, "rooms");
        methods.CheckIsRandomOfDoors(ref mapScrObject.randomAmountOfDoors, ref mapScrObject.doorsAmount, ref mapScrObject.randomDirOfDoors, ref mapScrObject.doorsAmountDir, "doors");
    }
}
