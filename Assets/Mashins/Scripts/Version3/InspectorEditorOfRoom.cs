using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomScrObject))]
public class InspectorEditorOfRoom : Editor
{
    MethodsForInspector methods = new MethodsForInspector();
    public override void OnInspectorGUI()
    {
        RoomScrObject roomScrObject = target as RoomScrObject;

        roomScrObject.specialRoom = EditorGUILayout.Toggle("Special Room", roomScrObject.specialRoom);
        //roomScrObject.randomAmountOfRooms = EditorGUILayout.Toggle("Special Room", roomScrObject.randomAmountOfRooms);

        methods.CheckIsRandomOfRooms(ref roomScrObject.randomAmountOfRooms, ref roomScrObject.roomsAmount, "rooms");
        methods.CheckIsRandomOfDoors(ref roomScrObject.randomAmountOfDoors, ref roomScrObject.doorsAmount, ref roomScrObject.randomDirOfDoors, ref roomScrObject.doorsAmountDir, "doors");
    }


}
