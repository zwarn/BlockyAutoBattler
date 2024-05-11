using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace blocks
{
    public class ShapeCreator
    {
        [Inject] private List<TileTypeSO> _tileTypes;

        private const int ShapeSize = 4;

        public Shape CreateShape()
        {
            var positions = new List<Vector2Int>();
            positions.Add(Vector2Int.zero);

            while (positions.Count < ShapeSize)
            {
                AddNeighbor(positions);
            }

            var tiles = new Dictionary<Vector2Int, TileTypeSO>();
            positions.ForEach(pos => tiles.Add(pos, RandomType()));
            Shape shape = new Shape(tiles);
            return shape;
        }

        private void AddNeighbor(List<Vector2Int> positions)
        {
            HashSet<Vector2Int> candidates = new HashSet<Vector2Int>();

            positions.ForEach(pos =>
            {
                candidates.Add(pos + Vector2Int.up);
                candidates.Add(pos + Vector2Int.right);
                candidates.Add(pos + Vector2Int.down);
                candidates.Add(pos + Vector2Int.left);
            });

            candidates.RemoveWhere(positions.Contains);

            var draw = candidates.ToArray();
            positions.Add(draw[Random.Range(0, draw.Length)]);
        }

        private TileTypeSO RandomType()
        {
            return _tileTypes[Random.Range(0, _tileTypes.Count)];
        }
    }
}