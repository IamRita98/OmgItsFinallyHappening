using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;



public class Cell
{
    public Vector2 worldPosition;
    public Vector2 gridPosition;
    public float G = int.MaxValue;
    public float H = int.MaxValue;
    public float F = int.MaxValue;
    public Cell bestNeighbour;
    TileType tileType;
    public float cost = 0; 

    
    TileBehaviour.TypeOfTile typeOfTile;

    public Cell(Vector2 pos, Vector2 wPos, float Costp, TileBehaviour.TypeOfTile typeOftilep)
    {
        worldPosition = wPos;
        gridPosition = pos;
        cost = Costp;
        typeOfTile = typeOftilep;
    }
}

public class Pathfinding : MonoBehaviour
{

    public int mapSizeX;
    public int mapSizeY;
    public Vector2 mapStartPos = Vector2.zero;
    List<Cell> cellsSearched;
    List<Cell> cellsToSearch;
    public List<Cell> path;


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
        ResetCells();
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
            cellsSearched.Add(cellToSearch);
            foreach (Vector2 neighbour in neighbors)
            {
                Vector2 neighbourPos = cellToSearch.worldPosition + neighbour;
                if (neighbourPos.x < mapStartPos.x || neighbourPos.y < mapStartPos.y || neighbourPos.x > (mapStartPos.x + mapSizeX) || neighbourPos.y > (mapStartPos.y + mapSizeY))
                {
                    continue;
                }
                Cell potentialNeighbour = cells.Find(Cell => Cell.worldPosition == neighbourPos);
                if (potentialNeighbour.worldPosition == goalPos)
                {
                    potentialNeighbour.bestNeighbour = cellToSearch;
                    ReconstructPath(potentialNeighbour); //Later will add neighbour to connection and make child then run Pathfinding
                    return;
                }
                if (cellsSearched.Contains(potentialNeighbour) || cellToSearch == null) continue;

                float tempG = Mathf.Round(1 + cellToSearch.G + cellToSearch.cost);
                if (!cellsToSearch.Contains(potentialNeighbour) || tempG < potentialNeighbour.G)
                {
                    potentialNeighbour.G = tempG; //Later this will change based on what type of tile the neighbour is
                    potentialNeighbour.H = ManhattanDistance(potentialNeighbour.worldPosition, goalPos);
                    potentialNeighbour.F = potentialNeighbour.G + potentialNeighbour.H;
                    potentialNeighbour.bestNeighbour = cellToSearch;


                    if (!cellsToSearch.Contains(potentialNeighbour)) cellsToSearch.Add(potentialNeighbour);
                }
            }
        }
    }

    void ReconstructPath(Cell currentCell)
    {
        Debug.Log("backtracking");
        currentCell = currentCell.bestNeighbour;
        path = new List<Cell>() { currentCell };

        while (currentCell.bestNeighbour != null)
        {
            print(currentCell.worldPosition);
            currentCell = currentCell.bestNeighbour;
            path.Add(currentCell);
        }

        path.Reverse();
    }

    void ResetCells()
    {
        foreach (Cell cell in cells)
        {
            cell.bestNeighbour = null;
            cell.G = int.MaxValue;
            cell.H = int.MaxValue;
            cell.F = int.MaxValue;
        }

    }

    void MakeGrid()
    {
        
        Vector2 currentWorldPos = mapStartPos;
        float sphereRadius = 2.0f;
        for (int x = 0; x <= mapSizeX; x++)
        {
            for (int y = 0; y <= mapSizeY; y++)
            {
                Vector2 t = new Vector2(Mathf.Round(mapStartPos.x + x), Mathf.Round(mapStartPos.y + y));
                currentWorldPos = t;
                TileBehaviour.TypeOfTile typeOfTilep = TileBehaviour.TypeOfTile.NORMAL;
                //Collider[] GOs=Physics.OverlapSphere(currentWorldPos, sphereRadius,0,QueryTriggerInteraction.Collide);
                RaycastHit2D hit=Physics2D.Raycast(currentWorldPos, Vector2.up, sphereRadius);
                //Physics.Raycast(new Ray(new Vector3(currentWorldPos.x,currentWorldPos.y,-1f), Vector3.forward), out RaycastHit hit, sphereRadius,7,QueryTriggerInteraction.Collide);
                if(hit.collider!=null && hit.collider.gameObject.layer==LayerMask.NameToLayer("Tilemap"))
                {
                    hit.collider.gameObject.TryGetComponent(out TileBehaviour tileB);
                    if(tileB!=null)typeOfTilep = tileB.difficulty;
                    Debug.Log($"Successfully detected a tile: {hit.collider.gameObject.name}");
                }
                
                float cost = 0;
                switch (typeOfTilep)
                {
                    case (TileBehaviour.TypeOfTile.NORMAL):
                        cost = 0;
                        break;
                    case (TileBehaviour.TypeOfTile.DIFFICULT):
                        cost = 1;
                        break;
                    case (TileBehaviour.TypeOfTile.IMPASSABLE):
                        cost = 1000;
                        break;
                }
                Debug.Log("cost:" + cost);

                //use Physics.OverlapSphere(currentWorldPos, sphereRadius,layermask if needed) to get a list of objects here
                //we are aiming to get the tile object specifically and get its name
                //once we have this then we can bind the name of the tile to what the cost of traveling through it is
                //or if it should be unpassable terrain
                //TypeOfTile t=Globals.TILESCOST[resultFromPhysics.strippedtileName]
                
                Cell thisCell = new Cell(new Vector2(x, y), currentWorldPos, cost, typeOfTilep);
                //print(thisCell.worldPosition);
                cells.Add(thisCell);
                
            }
        }

    }


    public float ManhattanDistance(Vector2 currentPos, Vector2 goalPos)
    {
        float h = Mathf.Abs((currentPos.x - goalPos.x)) + Mathf.Abs((currentPos.y - goalPos.y));
        Mathf.Round(h);
        return h;
    }
}
