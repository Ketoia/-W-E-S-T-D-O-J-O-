using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class RoomGenerator : MonoBehaviour
{
    public Tile door;
    public Tile wall;
    public Grid grid;
    public Camera cam;



    public void GenerateRoom(ref List<RoomSpecV4> existingRooms, List<RoomScrObjectV4> rooms)
    {
        Debug.Log("Width: " + Screen.width / wall.sprite.rect.size.x);
        for (int i = 0; i < existingRooms.Count; i++)
        {

            existingRooms[i].tilemapGameObject = new GameObject("Room " + i);
            existingRooms[i].tilemapGameObject.SetActive(false);
            existingRooms[i].tilemap = CreateTilemap(existingRooms[i].tilemapGameObject);
            CreateWall(wall, existingRooms[i].tilemap, cam);


        }
        CreateDoors(ref existingRooms, cam);
    }

    private void CreateDoors(ref List<RoomSpecV4> existingRooms, Camera cameraX)
    {

        for (int i = 0; i < existingRooms.Count; i++)
        {

            for (int x = 0; x < existingRooms[i].doors.Count; x++)
            {
                if (existingRooms[i].doors[x].positionOut == Vector3Int.zero)
                {
                    //random pos of doors
                    existingRooms[i].doors[x].position = Position(existingRooms[i].doors, cameraX);
                    //doorpos
                    //neighbourroom door outpos = doorpos
                }
            }
        }
    }

    private Vector3Int Position(List<DoorsSpecV4> doors, Camera cameraX)
    {
        List<Vector3Int> leftWallPos = new List<Vector3Int>();
        List<Vector3Int> rightWallPos = new List<Vector3Int>();
        List<Vector3Int> upWallPos = new List<Vector3Int>();
        List<Vector3Int> downWallPos = new List<Vector3Int>();
        int size = (int)cameraX.orthographicSize * 2;
        Vector3Int[] positions = GetAllPosInWall((int)cameraX.orthographicSize);
        for (int i = 0; i < positions.Length; i++)
        {
            if (i < size - 2)
            {
                leftWallPos.Add(positions[i]);
                //tilemap.SetTile(positions[i], door);
            }
            if (i < 2 * (size - 1) - 1 && i > size - 2)
            {
                rightWallPos.Add(positions[i]);

                // tilemap.SetTile(positions[i], door);
            }
            if (i < positions.Length - (positions.Length - 2 * (size - 1)) / 2 - 2 && i > 2 * (size - 1) - 1)
            {
                upWallPos.Add(positions[i]);

                //tilemap.SetTile(positions[i], door);
            }
            if (i < positions.Length - 1 && i > positions.Length - (positions.Length - 2 * (size - 1)) / 2)
            {
                downWallPos.Add(positions[i]);

                //tilemap.SetTile(positions[i], door);
            }
        }

        int[] newArray = new int[4];
        for (int i = 0; i < doors.Count; i++)
        {
            if (doors[i].position != Vector3Int.zero)
            {
                if (leftWallPos.Contains(doors[i].position))
                {
                    leftWallPos.Remove(doors[i].position);
                    newArray[0]++;
                }
                else if (rightWallPos.Contains(doors[i].position))
                {
                    rightWallPos.Remove(doors[i].position);
                    newArray[1]++;
                }
                else if (upWallPos.Contains(doors[i].position))
                {
                    upWallPos.Remove(doors[i].position);
                    newArray[2]++;
                }
                else if (downWallPos.Contains(doors[i].position))
                {
                    downWallPos.Remove(doors[i].position);
                    newArray[3]++;
                }
            }
        }

        //0 = left
        //1 = right
        //2 = up
        //3 = down
        
        int max = newArray.Max();
        int min = newArray.Min();

        int value;
        if (max > min)
        {
            List<int> newList = new List<int>();
            for (int i = 0; i < newArray.Length; i++)
            {
                if (newArray[i] == max)
                {
                    newList.Add(newArray[i]);
                }
            }
            value = newList[Random.Range(0, newList.Count - 1)];
        }
        else
        {
            value = Random.Range(0, newArray.Length - 1);
            //Debug.Log(value);
        }
        Vector3Int newPos;
        switch (value)
        {
            case 0:
                newPos = leftWallPos[Random.Range(0, leftWallPos.Count - 1)];
                return newPos;
            case 1:
                newPos = rightWallPos[Random.Range(0, rightWallPos.Count - 1)];
                return newPos;
            case 2:
                newPos = upWallPos[Random.Range(0, upWallPos.Count - 1)];
                return newPos;
            default:
                newPos = downWallPos[Random.Range(0, downWallPos.Count - 1)];
                return newPos;
                
        }
    }

    private void CreateWall(Tile tile, Tilemap tilemap, Camera cameraX)
    {
        Vector3Int[] positions = GetAllPosInWall((int)cameraX.orthographicSize);
        TileBase[] tiles = new TileBase[positions.Length];
        
        for (int i = 0; i < positions.Length; i++)
        {
            tiles[i] = tile;
        }
        tilemap.SetTiles(positions, tiles);

        
    }
    private Tilemap CreateTilemap(GameObject tilemap)
    {
       // var go = new GameObject(tilemapName);
        Tilemap tm = tilemap.AddComponent<Tilemap>();
        tilemap.AddComponent<TilemapRenderer>();
        
        tm.transform.SetParent(grid.transform);
        return tm;
    }
    private Vector3Int[] GetAllPosInWall(int size)
    {
        
        int height = size * 2;
        int fullWidth = size * Screen.width * 2 / Screen.height;
        int width = fullWidth;
        int newSize = height * 2 + width * 2 - 4;
        Vector3Int[] newArray = new Vector3Int[newSize];
        int dynamicHeight = -height / 2;
        int dynamicWidth = -width / 2;
        for (int i = 0; i < newSize; i++)
        {
            if (i < height - 1)
            {
                newArray[i] = new Vector3Int(-width / 2, ++dynamicHeight);

            }
            else if (i < 2* (height - 1))
            {
                newArray[i] = new Vector3Int(width / 2 - 1, --dynamicHeight);
            }
            else if (i < 2* height + width - 3)
            {
                newArray[i] = new Vector3Int(++dynamicWidth, height  /2 - 1);
            }
            else if (i < 2 * height + 2 * (width - 2))
            {
                newArray[i] = new Vector3Int(--dynamicWidth, -height / 2);
            }
        }
        return newArray;
    }
}
