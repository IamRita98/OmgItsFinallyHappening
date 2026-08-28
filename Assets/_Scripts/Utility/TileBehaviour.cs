using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public enum TypeOfTile
    {
        NORMAL, //grass, roads, floors,
        DIFFICULT, //mud,forest,mountains,ice, quicksand, sand
        IMPASSABLE, //walls, water maybe,cliffs, holes,lava
        //add more as needed
    }
    public TypeOfTile difficulty;
    public string name;
}
