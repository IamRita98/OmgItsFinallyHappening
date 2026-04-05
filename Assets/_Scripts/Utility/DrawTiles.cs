using System.Collections.Generic;
using UnityEngine;

public class DrawTiles : MonoBehaviour
{
    List<GameObject> tilesGOList = new List<GameObject>();
    public void DrawTilesGO(GameObject tileGO, List<Vector2> listOfTiles)
    {
        foreach (var tile in listOfTiles)
        {
            GameObject tempTileGO = Instantiate(tileGO, tile, Quaternion.identity);
            tilesGOList.Add(tempTileGO);
        }
    }
    public void ClearTiles()
    {
        foreach (var tileGO in tilesGOList)
        {
            Destroy(tileGO);
        }
        tilesGOList.Clear();
    }
}
