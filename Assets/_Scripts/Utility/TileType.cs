using UnityEngine;

public class TileType
{
    int gridX, gridY;
    int worldX, WorldY;
    public enum TypeOfTile
    {
        GRASS,
        MUD,
        WALL,
        //add more as needed
    }
}
/*
 The strategy behind this is to get each world tiles world and grid position along with its type into
a 2d grid array -> TileType[,] GridTiles=new TileType[SizeOfMap.x,SizeOfmap.y];
Loop through our tilemap or whatever we end up using for our world, and get the name of each tile and assign it
the appropriate worldx/y and gridx/y. Along with the cost of movement or if its a wall.
 */
