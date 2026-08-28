using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TestingTileScript : TileBase
{
    public Sprite sprite;
  public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
  {
    tileData.color = Color.white;
    tileData.sprite = sprite;
  }

  //public TileBehaviour.TypeOfTile typeOfTile;
}
