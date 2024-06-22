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
        private readonly Dictionary<Vector2Int, Shape> _shapes = new();

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
            _shapes[position] = null;

            OnSingleTileChanged?.Invoke(tileType, position);

            return true;
        }

        public bool CanPlaceShape(Shape shape, Vector2Int position, int rotation)
        {
            var list = shape.GetTilesTranslatedAndRotated(position, rotation).ToList();
            return list.All(pair => CanPlaceTile(pair.Tile, pair.Position));
        }

        public bool PlaceShape(Shape shape, Vector2Int position, int rotation)
        {
            var tiles = shape.GetTilesTranslatedAndRotated(position, rotation).ToList();

            if (!CanPlaceShape(shape, position, rotation)) return false;

            tiles.ForEach(pair =>
            {
                _tiles[pair.Position] = pair.Tile;
                _shapes[pair.Position] = shape;
            });

            OnTilesChanged?.Invoke();
            return true;
        }

        public Shape GetShape(Vector2Int position)
        {
            if (_shapes.TryGetValue(position, out Shape shape))
            {
                var offset = position;
                var positions = _shapes.Where(pair => pair.Value == shape).Select(pair => pair.Key).ToList();
                var tiles = positions.ToDictionary(pos => pos - offset, pos => _tiles[pos]);

                return new Shape(tiles);
            }

            return null;
        }

        public Shape RemoveShape(Vector2Int position)
        {
            var shape = GetShape(position);

            if (shape == null) return null;
            
            shape.GetTilesTranslated(position).ToList().ForEach(pair =>
            {
                _tiles.Remove(pair.Position);
                _shapes.Remove(pair.Position);
            });
                
            OnTilesChanged?.Invoke();

            return shape;
        }

        public bool Place(SelectionContainer selection, Vector2Int cellPosition)
        {
            var value = selection.Value;
            switch (value)
            {
                case TileTypeSO tile:
                    return PlaceTile(tile, cellPosition);
                case Shape shape:
                    return PlaceShape(shape, cellPosition, selection.Rotation);
                default:
                    throw new ArgumentException($"Unknown type {value.GetType()} for placing selection");
            }
        }
    }
}