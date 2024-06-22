using System;
using System.Collections.Generic;
using System.Linq;
using hand.selectable;
using UnityEngine;
using util;

namespace blocks
{
    public class TileZone
    {
        private readonly BoundsInt2D _bounds;
        private readonly Dictionary<Vector2Int, TileTypeSO> _tiles = new();

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
            if (!CanPlaceTile(tileType, position)) return false;

            _tiles[position] = tileType;

            OnSingleTileChanged?.Invoke(tileType, position);

            return true;
        }

        public bool CanPlaceShape(IEnumerable<(TileTypeSO tileType, Vector2Int position)> shape)
        {
            return shape.All(tile => CanPlaceTile(tile.tileType, tile.position));
        }

        public bool PlaceShape(IEnumerable<(TileTypeSO tileType, Vector2Int position)> shape)
        {
            var tiles = shape.ToList();

            if (!CanPlaceShape(tiles)) return false;

            tiles.ForEach(tile => { _tiles.Add(tile.position, tile.tileType); });

            OnTilesChanged?.Invoke();
            return true;
        }

        public void Place(SelectionContainer selection, Vector2Int cellPosition)
        {
            var value = selection.Value;
            switch (value)
            {
                case TileTypeSO tile:
                    PlaceTile(tile, cellPosition);
                    break;
                case Shape shape:
                    PlaceShape(shape.GetTilesTranslatedAndRotated(cellPosition, selection.Rotation));
                    break;
                default:
                    throw new ArgumentException($"Unknown type {value.GetType()} for placing selection");
            }
        }
    }
}