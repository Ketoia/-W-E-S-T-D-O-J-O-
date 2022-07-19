using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MethodsForInspector
{
    public void CheckIsRandomOfRooms(ref bool condition, ref int amount, string name)
    {
        condition = EditorGUILayout.Toggle("isRandom amount of " + name, condition);

        if (!condition)
        {
            amount = EditorGUILayout.IntField(name + " amount", amount);

        }
    }

    public void CheckIsRandomOfDoors(ref bool condition, ref int amount, ref bool conditionPerWall, ref Vector4 amountPerWall, string name)
    {
        condition = EditorGUILayout.Toggle("isRandom amount of " + name, condition);

        if (!condition)
        {
            if (conditionPerWall)
            {
                amount = EditorGUILayout.IntField(name + " amount", amount);
            }
            else
            {
                amountPerWall = EditorGUILayout.Vector4Field(name + " amount dir", amountPerWall);
            }

            conditionPerWall = EditorGUILayout.Toggle("isRandom per wall", conditionPerWall);
        }
    }
}
