using System.Collections.Generic;
using UnityEngine;

public static class Globals
{/*
        NORMAL,//grass, roads, floors,
        DIFFICULT,//mud,forest,mountains,ice, quicksand, sand
        IMPASSABLE,//walls, water maybe,cliffs, holes,lava*/
    public static Dictionary<string, Cell.TypeOfTile> TILESCOST=new Dictionary<string, Cell.TypeOfTile>()
    {
        {"grass",Cell.TypeOfTile.NORMAL},
        {"road",Cell.TypeOfTile.NORMAL},
        {"floor",Cell.TypeOfTile.NORMAL},
        {"mud",Cell.TypeOfTile.DIFFICULT},
        {"forest",Cell.TypeOfTile.DIFFICULT},
        {"mountain",Cell.TypeOfTile.DIFFICULT},
        {"quicksand",Cell.TypeOfTile.DIFFICULT},
        {"sand",Cell.TypeOfTile.DIFFICULT},
        {"wall",Cell.TypeOfTile.IMPASSABLE},
        {"ocean",Cell.TypeOfTile.IMPASSABLE},
        {"water",Cell.TypeOfTile.IMPASSABLE},
        {"cliff",Cell.TypeOfTile.IMPASSABLE},
        {"hole",Cell.TypeOfTile.IMPASSABLE},
        {"lava",Cell.TypeOfTile.IMPASSABLE},
    };
}
