using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewMapGenerator : MonoBehaviour
{
    public Tilemap tilemapBase;
    public List<RoomSpec> rooms;
    public RoomSpec defaultRoom;
    public FindPath findPath;
    public SetDoors setDoors;
    public RoomSetting roomSettings;
    

    [Range(1000000, 9999999)]
    public int seed;
    public bool random;

    //List<Vector2> postionsList = new List<Vector2>();

    void Start()
    {
        List<Vector2> allRoomsPos = new List<Vector2>();
        findPath = new FindPath();
        setDoors = new SetDoors();
        roomSettings = new RoomSetting();
        

        //tilemapBase = GetComponent<Tilemap>();
        if (random)
            seed = Random.Range(1000000, 9999999);

        Random.InitState(seed);
        //Debug.Log(rooms.Count);
        for (int i = 0; i < rooms.Count; i++)
        {
            for (int x = 0; x < rooms[i].roomAmount; x++)
            {
                Vector2 newRoomPos = roomPosition(i);
                allRoomsPos.Add(newRoomPos);
                Vector3Int[] actualPos = setRoomPos(rooms[i].tilesPostion(), newRoomPos);
                tilemapBase.SetTiles(actualPos, rooms[i].tilesBase());
            }
        }
        List<Vector2> grid = new List<Vector2>();
        for (int y = tilemapBase.cellBounds.xMin; y < tilemapBase.cellBounds.xMax; y++)
        {
            for (int z = tilemapBase.cellBounds.yMin; z < tilemapBase.cellBounds.yMax; z++)
            {
                grid.Add(new Vector2(y, z));
            }
        }
        for (int i = 0; i < allRoomsPos.Count; i++)
        {
            List<Vector2> findedPath = findPath.FindCorrectPath(Vector3.zero, allRoomsPos[i], grid, allRoomsPos);
            for (int y = 1; y < findedPath.Count; y++)
            {
                if (!tilemapBase.GetTile(new Vector3Int((int)findedPath[y].x, (int)findedPath[y].y, 0)))
                {
                    Vector3Int[] roadPos = setRoomPos(defaultRoom.tilesPostion(), findedPath[y]);
                    tilemapBase.SetTiles(roadPos, defaultRoom.tilesBase());
                }
            }
        }
        List<Tile> allRoomTiles = new List<Tile>();
        allRoomTiles.Add(defaultRoom.roomTile);
        for (int i = 0; i < rooms.Count; i++)
        {
            allRoomTiles.Add(rooms[i].roomTile);
        }
        List<RoomSetting> test = setDoors.GetRooms(grid, tilemapBase, allRoomTiles);
        //test = setDoors.GetRooms(grid, tilemapBase, allRoomTiles);
        //Debug.Log(test.Count);
        for (int i = 0; i < test.Count; i++)
        {
            Debug.Log(test[i].leftNeighbour + " " + test[i].rightNeighbour + " " + test[i].upNeighbour + " " + test[i].downNeighbour);
            //Debug.Log(test[i].x + " " + test[i].y);
        }
        //Debug.Log(roomSettings.GetRooms(grid, tilemapBase, allRoomTiles).Count);
        //List<Vector2> test = roomSettings.CorrectGrid(grid, tilemapBase);
    }

    public Vector2 roomPosition(int i)
    {
        int xPosition;
        int yPosition;

        do
        {
            int xRandomPositionSquare = Random.Range(-(int)(Mathf.Pow(rooms[i].roomDistant, 2)), (int)(Mathf.Pow(rooms[i].roomDistant, 2)) + 1);
            bool isPossitive = Random.Range(0, 2) > 0;

            //int xPosition;
            if (xRandomPositionSquare >= 0)
                xPosition = (int)Mathf.Sqrt(xRandomPositionSquare);
            else
                xPosition = -(int)(Mathf.Sqrt(-xRandomPositionSquare));

            //int yPosition;
            if (isPossitive)
                yPosition = (int)(Mathf.Sqrt(Mathf.Pow(rooms[i].roomDistant, 2) - Mathf.Abs(xRandomPositionSquare)));
            else
                yPosition = -(int)(Mathf.Sqrt(Mathf.Pow(rooms[i].roomDistant, 2) - Mathf.Abs(xRandomPositionSquare)));
            //Debug.Log((!tilemapBase.GetTile(new Vector3Int(xPosition, yPosition, 0))));
        } while (tilemapBase.GetTile(new Vector3Int(xPosition, yPosition, 0)));

        return new Vector2(xPosition, yPosition);
    }

    public Vector3Int[] setRoomPos(Vector3Int[] roomTilesPos, Vector2 newRoomPos)
    {
        Vector3Int[] actualPos = new Vector3Int[roomTilesPos.Length];
        //Debug.Log(newRoomPos);
        for (int y = 0; y < roomTilesPos.Length; y++)
        {
            actualPos[y] = new Vector3Int(roomTilesPos[y].x + (int)(newRoomPos.x), roomTilesPos[y].y + (int)(newRoomPos.y), roomTilesPos[y].z);
        }
        return actualPos;
    }
}
