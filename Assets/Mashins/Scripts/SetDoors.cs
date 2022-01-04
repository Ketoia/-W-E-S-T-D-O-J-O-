using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SetDoors 
{
    // Start is called before the first frame update
    public List<RoomSetting> GetRooms(List<Vector2> grid, Tilemap tilemapBase, List<Tile> tiles)
    {
        List<Vector2> neighbourOffsetList = new List<Vector2>();
        neighbourOffsetList.Add(Vector2.down);
        neighbourOffsetList.Add(Vector2.up);
        neighbourOffsetList.Add(Vector2.right);
        neighbourOffsetList.Add(Vector2.left);

        List<RoomSetting> getRooms = new List<RoomSetting>();
        
        List<Vector2> correctGrid = CorrectGrid(grid, tilemapBase);
        for (int i = 0; i < correctGrid.Count; i++)
        {
            RoomSetting room = new RoomSetting();

            room.x = (int)correctGrid[i].x;
            room.y = (int)correctGrid[i].y;

            for (int x = 0; x < tiles.Count ; x++)
            {
                if (tilemapBase.GetTile(new Vector3Int((int)correctGrid[i].x, (int)correctGrid[i].y, 0)) == tiles[x])
                    room.tile = tiles[x];
            }
            room.index = i;
            for (int x = 0; x < neighbourOffsetList.Count; x++)
            {
                Vector2 isNeighbour = new Vector2(room.x + neighbourOffsetList[x].x, room.y + neighbourOffsetList[x].y);
                if (correctGrid.Contains(isNeighbour))
                {
                     room.isNeighbour(x, correctGrid.IndexOf(isNeighbour));
                }
            }
            getRooms.Add(room);
        }
        return getRooms;
    }

    public List<Vector2> CorrectGrid(List<Vector2> grid, Tilemap tilemapBase)
    {
        List<Vector2> correctGrid = new List<Vector2>();
        for (int i = 0; i < grid.Count; i++)
        {
            if (tilemapBase.GetTile(new Vector3Int((int)grid[i].x, (int)grid[i].y, 0)))
            {
                correctGrid.Add(grid[i]);
            }
        }
        return correctGrid;
    }


}
public struct RoomSetting
{
    public int x;
    public int y;

    public int index;

    //public int cameFromIndex;

    public Tile tile;
    //public List<int> indexOfNeighbours;
    public int leftNeighbour;
    public int rightNeighbour;
    public int upNeighbour;
    public int downNeighbour;

    public void isNeighbour(int dir, int index)
    {
        if (dir == 0)
            downNeighbour = index;
        if (dir == 1)
            upNeighbour = index;
        if (dir == 2)
            rightNeighbour = index;
        if (dir == 3)
            leftNeighbour = index;
    }
}