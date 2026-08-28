using System.Collections.Generic;
using UnityEngine;

public static class Globals
{/*
        NORMAL,//grass, roads, floors,
        DIFFICULT,//mud,forest,mountains,ice, quicksand, sand
        IMPASSABLE,//walls, water maybe,cliffs, holes,lava*/
    public static Dictionary<string, TileBehaviour.TypeOfTile> TILESCOST=new Dictionary<string, TileBehaviour.TypeOfTile>()
    {
        {"grass",TileBehaviour.TypeOfTile.NORMAL},
        {"road",TileBehaviour.TypeOfTile.NORMAL},
        {"floor",TileBehaviour.TypeOfTile.NORMAL},
        {"mud",TileBehaviour.TypeOfTile.DIFFICULT},
        {"forest",TileBehaviour.TypeOfTile.DIFFICULT},
        {"mountain",TileBehaviour.TypeOfTile.DIFFICULT},
        {"quicksand",TileBehaviour.TypeOfTile.DIFFICULT},
        {"sand",TileBehaviour.TypeOfTile.DIFFICULT},
        {"wall",TileBehaviour.TypeOfTile.IMPASSABLE},
        {"ocean",TileBehaviour.TypeOfTile.IMPASSABLE},
        {"water",TileBehaviour.TypeOfTile.IMPASSABLE},
        {"cliff",TileBehaviour.TypeOfTile.IMPASSABLE},
        {"hole",TileBehaviour.TypeOfTile.IMPASSABLE},
        {"lava",TileBehaviour.TypeOfTile.IMPASSABLE},
    };
}
