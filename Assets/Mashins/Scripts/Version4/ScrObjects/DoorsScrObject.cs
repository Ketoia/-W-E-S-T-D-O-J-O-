using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsScrObject : ScriptableObject
{
    public bool randomAmountOfDoors = true;
    public int doorsAmount;
    public bool randomDirOfDoors = true;
    public Vector4 doorsAmountDir;
    

    public int CalculateDoorAmount(int existingRooms, int remainingSpecificRooms, int roomsCount)
    {
        //Debug.Log("remaining0: " + remainingSpecificRooms);
        if (!randomAmountOfDoors && !randomDirOfDoors)
        {
            //Debug.Log("coœ");
            return (int)(doorsAmountDir.x + doorsAmountDir.y + doorsAmountDir.z + doorsAmountDir.w);
        }
        else if (!randomAmountOfDoors)
        {
            //Debug.Log("coœ");
            return doorsAmount;
        }
        else if (existingRooms > roomsCount & remainingSpecificRooms <= 0)
        {
            //Debug.Log("WTF??: " + remainingSpecificRooms);
            return 1;
        }
        else
        {
            float a = roomsCount;
            float b = existingRooms;
            int maxRange = Mathf.RoundToInt(Mathf.Log(a/b*50));
            int minRange = 1;
            if (existingRooms <= roomsCount)
                minRange = 2;
            if (maxRange < 1)
                maxRange = 1;
            maxRange++;
            //Debug.Log("Dzia³a1");
            int result = Random.Range(minRange, maxRange);
            Debug.Log(minRange + " " + result + " " + maxRange);
            return result;
        }
    }


}
