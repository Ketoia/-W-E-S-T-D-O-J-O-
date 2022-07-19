using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorCalculationV2
{
    public Vector2 RandomPosition(int seed, int maxDistance, int distanceFromOthers, int distanceFromSameTypes, List<Vector2> allRoomPositions, List<Vector2> allRoomPositionsOfSameType)
    {
        Random.InitState(seed);
        int xPosition = 0;
        int yPosition = 0;
        bool allright = true;
        while (allright)
        {
            allright = false;
            xPosition = Random.Range(-maxDistance, maxDistance);
            yPosition = Random.Range(-maxDistance, maxDistance);
            for (int x = -distanceFromOthers; x <= distanceFromOthers; x++)
            {
                for (int y = -distanceFromOthers; y <= distanceFromOthers; y++)
                {
                    for (int z = -distanceFromSameTypes; z <= distanceFromSameTypes; z++)
                    {
                        for (int g = -distanceFromSameTypes; g <= distanceFromSameTypes; g++)
                        {
                            if (allRoomPositions.Contains(new Vector2(xPosition + z, yPosition + g)))
                            {
                                if (allRoomPositionsOfSameType.Contains(new Vector2(xPosition + x, yPosition + y)))
                                {
                                    allright = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return new Vector2(xPosition, yPosition);
    }

    public List<Vector2> GridCalc(List<Vector2> allPositions)
    {
        List<Vector2> grid = new List<Vector2>();
        int minValueX = 0;
        int maxValueX = 0;

        int minValueY = 0;
        int maxValueY = 0;
        for (int i = 0; i < allPositions.Count; i++)
        {
            if (i == 0)
            {
                minValueX = (int)allPositions[i].x;
                maxValueX = (int)allPositions[i].x;

                minValueY = (int)allPositions[i].y;
                maxValueY = (int)allPositions[i].y;
            }
            else
            {
                if (minValueX > (int)allPositions[i].x)
                    minValueX = (int)allPositions[i].x;

                if (minValueY > (int)allPositions[i].y)
                    minValueY = (int)allPositions[i].y;

                if (maxValueX < (int)allPositions[i].x)
                    maxValueX = (int)allPositions[i].x;

                if (maxValueY < (int)allPositions[i].y)
                    maxValueY = (int)allPositions[i].y;
            }
        }
        for (int x = minValueX; x <= maxValueX; x++)
        {
            for (int y = minValueY; y <= maxValueY; y++)
            {
                grid.Add(new Vector2(x, y));
            }
        }
        return grid;
    }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    public List<Vector2> FindCorrectPath(Vector2 startPosition, Vector2 endPosition, List<Vector2> grid, List<Vector2> unWakableNodes)
    {
        int gridSize = grid.Count;
        //Vector2 startPosition = new Vector2(startPosition3Int.x, startPosition3Int.y);
        //Debug.Log(startPosition);
        //Vector2 endPosition = new Vector2(endPosition3Int.x, endPosition3Int.y);
        //Debug.Log();

        PathNode[] pathNodeArray = new PathNode[gridSize];

        for (int i = 0; i < gridSize; i++)
        {
            PathNode pathNode = new PathNode();
            pathNode.x = (int)grid[i].x;
            pathNode.y = (int)grid[i].y;
            pathNode.index = i;

            pathNode.gCost = int.MaxValue;
            pathNode.hCost = CalculateDistanceCost(new Vector2(pathNode.x, pathNode.y), endPosition);
            pathNode.CalculateFCost();

            pathNode.isWakable = true;
            pathNode.cameFromIndex = -1;

            pathNodeArray[pathNode.index] = pathNode;
        }

        for (int i = 0; i < unWakableNodes.Count; i++)
        {
            if (unWakableNodes[i] != endPosition)
            {
                PathNode walkablePathNode = pathNodeArray[grid.IndexOf(unWakableNodes[i])];
                walkablePathNode.SetIsWalkable(false);
                pathNodeArray[grid.IndexOf(unWakableNodes[i])] = walkablePathNode;
            }
        }

        //NativeList<int2> neighbourOffsetList = ScanRange(5);
        List<Vector2> neighbourOffsetList = new List<Vector2>();
        neighbourOffsetList.Add(Vector2.right);
        neighbourOffsetList.Add(Vector2.left);
        neighbourOffsetList.Add(Vector2.down);
        neighbourOffsetList.Add(Vector2.up);

        int endNodeIndex = grid.IndexOf(endPosition);
        PathNode startNode = pathNodeArray[grid.IndexOf(startPosition)];

        startNode.gCost = 0;
        startNode.CalculateFCost();
        pathNodeArray[0] = startNode;

        //NativeList<int> openList = new NativeList<int>(Allocator.Temp);
        //NativeList<int> closedList = new NativeList<int>(Allocator.Temp);
        List<int> openList = new List<int>();
        List<int> closedList = new List<int>();

        openList.Add(startNode.index);

        while (openList.Count > 0)
        {
            int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
            PathNode currentNode = pathNodeArray[currentNodeIndex];

            if (currentNodeIndex == endNodeIndex)
            {
                break;
            }

            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i] == currentNodeIndex)
                {
                    openList.RemoveAt(i);
                    break;
                }
            }

            closedList.Add(currentNodeIndex);

            for (int i = 0; i < neighbourOffsetList.Count; i++)
            {
                Vector2 neighbourOffset = neighbourOffsetList[i];
                Vector2 neighbourPosition = new Vector2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                if (!IsPositionInsideGrid(neighbourPosition, grid))
                {
                    continue;
                }

                int neighbourNodeIndex = grid.IndexOf(neighbourPosition);

                if (closedList.Contains(neighbourNodeIndex))
                {
                    continue;
                }

                PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
                if (!neighbourNode.isWakable)
                {
                    continue;
                }

                Vector2 currentNodePosition = new Vector2(currentNode.x, currentNode.y);

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);

                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromIndex = currentNodeIndex;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.CalculateFCost();
                    pathNodeArray[neighbourNodeIndex] = neighbourNode;

                    if (!openList.Contains(neighbourNode.index))
                    {
                        openList.Add(neighbourNode.index);
                    }
                }
            }


        }

        PathNode endNode = pathNodeArray[endNodeIndex];

        if (endNode.cameFromIndex == -1)
        {
            //Nie znaleziono path
            //Debug.Log("niee znaleziono drogi, ale dzia³a");
            List<Vector2> path = new List<Vector2>();
            return path;
        }
        else
        {
            //Znaleziona path
            List<Vector2> path = CalculatePath(pathNodeArray, endNode);

            //Debug.Log("Znaleziono droge");
            return path;
            //PathFinded(path, materialIndex, unWakableNumber);

            //path.Dispose();
        }

        //pathNodeArray.Dispose();
        //neighbourOffsetList.Dispose();
        //openList.Dispose();
        //closedList.Dispose();
    }

    private List<Vector2> CalculatePath(PathNode[] pathNodeArray, PathNode endNode)
    {
        if (endNode.cameFromIndex == -1)
        {
            //nope
            return new List<Vector2>();
        }
        else
        {
            //yep
            List<Vector2> path = new List<Vector2>();
            path.Add(new Vector2(endNode.x, endNode.y));

            PathNode currentNode = endNode;
            while (currentNode.cameFromIndex != -1)
            {
                PathNode cameFromNode = pathNodeArray[currentNode.cameFromIndex];
                path.Add(new Vector2(cameFromNode.x, cameFromNode.y));
                currentNode = cameFromNode;
            }

            return path;
        }
    }
    private int GetLowestCostFNodeIndex(List<int> openList, PathNode[] pathNodeArray)
    {
        PathNode lowestFCostPathNode = pathNodeArray[openList[0]];
        for (int i = 1; i < openList.Count; i++)
        {
            PathNode testPathNode = pathNodeArray[openList[i]];
            if (testPathNode.fCost < lowestFCostPathNode.fCost)
            {
                lowestFCostPathNode = testPathNode;
            }
        }
        return lowestFCostPathNode.index;
    }
    private bool IsPositionInsideGrid(Vector2 gridPosition, List<Vector2> grid)
    {
        return grid.Contains(gridPosition);
    }

    private int CalculateDistanceCost(Vector2 aPosition, Vector2 bPosition)
    {
        int xDistance = (int)Mathf.Abs(aPosition.x - bPosition.x);
        int yDistance = (int)Mathf.Abs(aPosition.y - bPosition.y);
        int remaining = (int)Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;

    }
    private struct PathNode
    {
        public int x;
        public int y;

        public int index;

        public int gCost;
        public int hCost;
        public int fCost;
        public bool isWakable;


        public int cameFromIndex;

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

        public void SetIsWalkable(bool isWalkable)
        {
            this.isWakable = isWalkable;
        }
    }
}
