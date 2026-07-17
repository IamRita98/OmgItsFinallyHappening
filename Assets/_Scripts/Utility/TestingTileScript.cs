using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TestingTileScript : TileBase
{
  public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
  {
    tileData.color = Color.white;
  }

  public Cell.TypeOfTile typeOfTile;
}
