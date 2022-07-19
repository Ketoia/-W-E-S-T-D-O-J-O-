using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomScrObjectV4))]
public class InspectorEditorOfRoomV4 : Editor
{
    MethodsForInspectorV4 methods = new MethodsForInspectorV4();
    private SerializedProperty _tilemaps;

    public void OnEnable()
    {
        _tilemaps = serializedObject.FindProperty("tilemaps");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        RoomScrObjectV4 roomScrObject = target as RoomScrObjectV4;
       
        if (roomScrObject.isSpecialRoom = EditorGUILayout.Toggle("Special Room", roomScrObject.isSpecialRoom))
        {
            roomScrObject.isStartRoom = false;
        }
        if (roomScrObject.isStartRoom = EditorGUILayout.Toggle("Start Room", roomScrObject.isStartRoom))
        {
            roomScrObject.isSpecialRoom = false;
        }
        //roomScrObject.randomAmountOfRooms = EditorGUILayout.Toggle("Special Room", roomScrObject.randomAmountOfRooms);
        if (!roomScrObject.isStartRoom )
        {
            methods.CheckIsRandomOfRooms(ref roomScrObject.randomAmountOfRooms, ref roomScrObject.roomsAmount, "rooms");
        }
        methods.CheckIsRandomOfDoors(ref roomScrObject.randomAmountOfDoors, ref roomScrObject.doorsAmount, ref roomScrObject.randomDirOfDoors, ref roomScrObject.doorsAmountDir, "doors");

        _tilemaps.arraySize = EditorGUILayout.IntField("Size", _tilemaps.arraySize);

        for (int i = 0; i < _tilemaps.arraySize; i++)
        {
            var tilemap = _tilemaps.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(tilemap, new GUIContent("Tilemap " + i), true);
        }

        serializedObject.ApplyModifiedProperties();
        //roomScrObject.OnInspectorGUI();
        //DrawPropertiesExcluding(roomScrObject.tilemaps, "Special Room");
        //DrawDefaultInspector();
        //EditorGUILayout.ObjectField(roomScrObject.tilemaps);
    }

    

}
