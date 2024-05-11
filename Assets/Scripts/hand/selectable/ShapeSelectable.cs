using System.Collections.Generic;
using System.Linq;
using blocks;
using UnityEngine;

namespace hand.selectable
{
    public class ShapeSelectable : Selectable
    {
        private readonly Shape _shape;

        public ShapeSelectable(Shape shape)
        {
            _shape = shape;
        }

        public override void Execute(TileZone tileZone, Vector3Int position)
        {
            tileZone.PlaceShape(GetTiles().Select(tile => (tile.Tile, tile.Position + (Vector2Int)position)));
        }

        public override IEnumerable<(TileTypeSO Tile, Vector2Int Position)> GetTiles()
        {
            return _shape.GetTiles();
        }
    }
}