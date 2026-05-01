using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


class Cell
{
    public Vector2 worldPosition;
    public Vector2 gridPosition;
    public float G = int.MaxValue;
    public float H = int.MaxValue;
    public float F = int.MaxValue;
    public Cell bestNeighbour;
    public enum TypeOfTile
    {
        NORMAL,
        DIFFICULT,
        IMPASSABLE,
        //add more as needed
    }

    public Cell(Vector2 pos, Vector2 wPos)
    {
        worldPosition = wPos;
        gridPosition = pos;
    }
}

public class Pathfinding : MonoBehaviour
{
    public int mapSizeX;
    public int mapSizeY;
    public Vector2 mapStartPos = Vector2.zero;
    List<Cell> cellsSearched;
    List<Cell> cellsToSearch;
    List<Cell> path;


    Vector2 startPos;
    Vector2 goalPos;
    Vector2 currentPos;
    Vector2[] neighbors = new Vector2[4] { Vector2.right, Vector2.left, Vector2.up, Vector2.down };

    List<Cell> cells = new List<Cell>();

    private void Start()
    {
        MakeGrid();
    }

    public void FindPath(Vector2 startPosP, Vector2 endPos)
    {
        //Get starting info
        cellsSearched = new List<Cell>();
        cellsToSearch = new List<Cell>();
        startPos = startPosP;
        currentPos = startPos;
        goalPos = endPos;

        //Give starting cell its costs-- G = distance from node starting node to neighbour, H = distance from neighbour to end node, F = G + H
        Cell startingCell = cells.Find(Cell => Cell.worldPosition == startPosP);
        startingCell.G = 0;
        startingCell.H = ManhattanDistance(startingCell.gridPosition, goalPos);
        startingCell.F = startingCell.G + startingCell.H;
        cellsToSearch.Add(startingCell);

        StartPathfinding();
    }

    void StartPathfinding()
    {
        while (cellsToSearch.Count > 0)
        {
            //sort by f cost and pop next lowest for cellsToSearch
            //List<Vector2> tempList = cellsToSearch.OrderBy(cells[] x => x.F).ToList();
            cellsToSearch.Sort((Cell a, Cell b) => a.F.CompareTo(b.F));
            Cell cellToSearch = cellsToSearch[0];
            cellsToSearch.Remove(cellToSearch);

            foreach (Vector2 neighbour in neighbors)
            {
                cellsToSearch.Remove(cellToSearch);
                cellsSearched.Add(cellToSearch);
                Vector2 neighbourPos = currentPos + neighbour;
                Cell potentialNeighbour = cells.Find(Cell => Cell.worldPosition == neighbourPos);
                if (potentialNeighbour.worldPosition == goalPos) ReconstructPath(potentialNeighbour); //Later will add neighbour to connection and make child then run Pathfinding
                if (cellsSearched.Contains(potentialNeighbour) || cellToSearch == null) continue;
                
                float tempG = Mathf.Round(1 + cellToSearch.G);
                if (!cellsToSearch.Contains(potentialNeighbour) || tempG < potentialNeighbour.G)
                {
                    potentialNeighbour.G = tempG; //Later this will change based on what type of tile the neighbour is
                    potentialNeighbour.H = ManhattanDistance(potentialNeighbour.worldPosition, goalPos);
                    potentialNeighbour.F = potentialNeighbour.G + potentialNeighbour.H;
                    potentialNeighbour.bestNeighbour = cellToSearch;

                    if(!cellsToSearch.Contains(potentialNeighbour)) cellsToSearch.Add(potentialNeighbour);
                }
            }
        }
    }

    void ReconstructPath(Cell currentCell)
    {
        path = new List<Cell>() { currentCell };

        while (currentCell.bestNeighbour != null)
        {
            currentCell = currentCell.bestNeighbour;
            path.Add(currentCell);
        }

        path.Reverse();
    }

    void MakeGrid()
    {
        Vector2 currentWorldPos = mapStartPos;
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                currentWorldPos = new Vector2(currentWorldPos.x + x, currentWorldPos.y + y);
                Cell thisCell = new Cell(new Vector2(x, y), currentWorldPos);
                cells.Add(thisCell);
            }
        }
    }

    float ManhattanDistance(Vector2 currentPos, Vector2 goalPos)
    {
        float h = Mathf.Abs((currentPos.x - goalPos.x)) + Mathf.Abs((currentPos.y - goalPos.y));
        Mathf.Round(h);
        return h;
    }
}
