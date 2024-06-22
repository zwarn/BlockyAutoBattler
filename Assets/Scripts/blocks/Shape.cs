using System.Collections;
using System.Collections.Generic;
using System.Linq;
using hand;
using UnityEngine;
using util;

namespace blocks
{
    public class Shape : ISelectable
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

        public IEnumerable<(TileTypeSO Tile, Vector2Int Position)> GetTilesTranslated(Vector2Int offset)
        {
            return _tiles.Select(tile => (tile.Value, tile.Key + offset));
        }
        
        public IEnumerable<(TileTypeSO Tile, Vector2Int Position)> GetTilesTranslatedAndRotated(Vector2Int offset, int rotation)
        {
            return _tiles.Select(tile => (tile.Value, tile.Key.Rotate(rotation) + offset));
        }
    }
}