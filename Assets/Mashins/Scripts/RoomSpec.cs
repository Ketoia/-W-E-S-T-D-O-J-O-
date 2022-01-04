using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Room", menuName = "Rooms")]
public class RoomSpec : ScriptableObject
{
    public string roomName;
    public Tilemap roomPrefab;
    public Tile roomTile;
    

    public int roomDistant;
    public int roomAmount;

    public TileBase[] tilesBase()
    {
        roomPrefab.CompressBounds();
        BoundsInt bounds = roomPrefab.cellBounds;
        TileBase[] allTIles = roomPrefab.GetTilesBlock(bounds);

        return allTIles;
    }
    public Vector3Int[] tilesPostion()
    {
        roomPrefab.CompressBounds();
        BoundsInt bounds = roomPrefab.cellBounds;
        Vector3Int[] allPos = new Vector3Int[bounds.size.x*bounds.size.y];
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                allPos[x + y * bounds.size.x] = new Vector3Int(x, y, 0);
            }
        }
        return allPos;
    }


}
