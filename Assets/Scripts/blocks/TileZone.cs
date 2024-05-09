using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using util;

namespace blocks
{
    public class TileZone
    {
        private readonly BoundsInt2D _bounds;
        private Dictionary<Vector2Int, TileTypeSO> _tiles = new Dictionary<Vector2Int, TileTypeSO>();

        public TileZone(BoundsInt2D bounds)
        {
            _bounds = bounds;
        }

        public event Action<TileTypeSO, Vector2Int> OnSingleTileChanged;
        public event Action OnTilesChanged;

        public IEnumerable<(TileTypeSO Tile, Vector2Int Position)> GetTiles()
        {
            var result = _tiles.Select(pair => (pair.Value, pair.Key));
            return result;
        }

        public bool CanPlaceTile(TileTypeSO tileType, Vector2Int position)
        {
            return _bounds.Contains(position) && !_tiles.ContainsKey(position);
        }

        public bool PlaceTile(TileTypeSO tileType, Vector2Int position)
        {
            if (!CanPlaceTile(tileType, position))
            {
                return false;
            }

            _tiles[position] = tileType;
            
            OnSingleTileChanged?.Invoke(tileType, position);

            return true;
        }
        
        
    }
}