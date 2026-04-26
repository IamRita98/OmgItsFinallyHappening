using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


class Cell
{
    public Vector2 gridPosition;
    public float G = int.MaxValue;
    public float H = int.MaxValue;
    public float F = int.MaxValue;
    public Vector2 bestNeighbour;
    public enum TypeOfTile
    {
        NORMAL,
        DIFFICULT,
        IMPASSABLE,
        //add more as needed
    }

    public Cell(Vector2 pos)
    {
        gridPosition = pos;
    }
}

public class Pathfinding : MonoBehaviour
{
    public int mapSizeX;
    public int mapSizeY;
    public Vector2 mapStartPos = Vector2.zero;
    List<Vector2> cellsSearched;
    List<Vector2> cellsToSearch;

    Vector2 startPos;
    Vector2 goalPos;
    Vector2 currentPos;
    Vector2[] neighbors = new Vector2[4] { Vector2.right, Vector2.left, Vector2.up, Vector2.down };

    Dictionary<Vector2, Cell> cells = new Dictionary<Vector2, Cell>();

    private void Start()
    {
        MakeGrid();
    }

    public void FindPath(Vector2 startPosP, Vector2 endPos)
    {
        //Get starting info
        cellsSearched = new List<Vector2>();
        cellsToSearch = new List<Vector2>();
        startPos = startPosP;
        currentPos = startPos;
        goalPos = endPos;

        //Give starting cell its costs-- G = distance from node starting node to neighbour, H = distance from neighbour to end node, F = G + H
        Cell startingCell = cells[startPos];
        startingCell.G = 0;
        startingCell.H = Vector2.Distance(startPos, endPos);
        startingCell.F = startingCell.G + startingCell.H;
        cellsToSearch.Add(currentPos);

        StartPathfinding();
    }

    void StartPathfinding()
    {
        //This is after the list has been filled
        while (cellsToSearch.Count > 0)
        {
            foreach (Vector2 neighbour in neighbors)
            {
                Vector2 neighbourPos = currentPos + neighbour;
                if (cellsSearched.Contains(neighbourPos)) continue;
                cellsToSearch.Add(neighbourPos);
            }
            
            Vector2 cellToSearch = cellsSearched[0];
        }
    }

    void MakeGrid()
    {
        Vector2 currentWorldPos = mapStartPos;
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                currentWorldPos = new Vector2(currentWorldPos.x + x, currentWorldPos.y + y);
                Cell thisCell = new Cell(new Vector2(x, y));
                cells.Add(currentWorldPos, thisCell);
                //Make Dict entry passing this grid pos-- World position should be i,j (WE HAVE TO MAKE EVERY MAP STARTING FROM 0,0)
            }
        }
    }
}
