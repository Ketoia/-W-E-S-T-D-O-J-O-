using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap roomTilemap;
    

    public int mainRooms;
    public int secondRooms;
    public int specialRooms;
    [Range(1000000,9999999)]
    public int seed;
    public bool random;

    List<Vector2> postionsList = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        if (random)
        {
            seed = Random.Range(1000000, 9999999);
        }
        Random.InitState(seed);
        for (int i = 0; i < mainRooms; i++)
        {
            Vector2 newRoomPos = CalcPosition(i);
            Tilemap actualRoom = Instantiate(roomTilemap, new Vector3(newRoomPos.x, newRoomPos.y, 0),Quaternion.identity, transform);
            actualRoom.name = "Room " + i;
            postionsList.Add(actualRoom.transform.position);
        }
        for (int i = 0; i < secondRooms; i++)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 CalcPosition(int offset)
    {
        int randomValue = Random.Range(0, 4);
        if (offset != 0)
        {
            Vector2 tRight = new Vector2(roomTilemap.size.x, 0);
            Vector2 tDown = new Vector2(0, -roomTilemap.size.y);
            Vector2 tLeft = new Vector2(-roomTilemap.size.x, 0);
            Vector2 tUp = new Vector2(0, roomTilemap.size.y);
            //0 = right
            Vector2 right = postionsList[offset - 1] + tRight;
            //1 = down
            Vector2 down = postionsList[offset - 1] + tDown;
            //2 = left
            Vector2 left = postionsList[offset - 1] + tLeft;
            //3 = up
            Vector2 up = postionsList[offset - 1] + tUp;

            Debug.Log(randomValue + " " + (randomValue == 0 && !isContains(postionsList, right, tUp, tDown)));
            if (randomValue == 0 && !isContains(postionsList,right,tUp,tDown))
                return right;
            else if (randomValue == 1 && !isContains(postionsList, down, tRight, tLeft))
                return down;
            else if (randomValue == 2 && !isContains(postionsList, left, tUp, tDown))
                return left;
            else if (randomValue == 3 && !isContains(postionsList, up, tRight, tLeft))
                return up;
            else if (!isContains(postionsList, right, tUp, tDown))
                return right;
            else if (!isContains(postionsList, left, tUp, tDown))
                return left;
            else if (!isContains(postionsList, down, tRight, tLeft))
                return down;
            else if (!isContains(postionsList, up, tRight, tLeft))
                return up;
        }

        return new Vector2(0, 0);
    }

    public bool isContains(List<Vector2> list, Vector2 position, Vector2 left, Vector2 right)
    {
        Vector2 tLeft = position + left;
        Vector2 tRight = position + right;
        if (list.Contains(position) || list.Contains(tLeft) || list.Contains(tRight))
            return true;
        else
            return false;
    }
}
