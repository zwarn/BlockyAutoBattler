using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace blocks
{
    public class Shape
    {
        private readonly Dictionary<Vector2Int, TileTypeSO> _tiles;

        public Shape(Dictionary<Vector2Int, TileTypeSO> tiles)
        {
            _tiles = tiles;
        }

        public IEnumerable<(TileTypeSO Tile, Vector2Int Position)> GetTiles()
        {
            return _tiles.Select(tile => (tile.Value, tile.Key));
        }
    }
}