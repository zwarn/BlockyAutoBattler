using System.Collections.Generic;
using blocks;
using UnityEngine;

namespace hand.selectable
{
    public class TileSelectable : Selectable
    {
        public TileTypeSO TileType { get; }

        public TileSelectable(TileTypeSO tileType)
        {
            TileType = tileType;
        }

        public override void Execute(TileZone tileZone, Vector3Int position)
        {
            tileZone.PlaceTile(TileType, (Vector2Int)position);
        }

        public override IEnumerable<(TileTypeSO Tile, Vector2Int Position)> GetTiles()
        {
            return new List<(TileTypeSO Tile, Vector2Int Position)> { (TileType, Vector2Int.zero) };
        }
    }
}