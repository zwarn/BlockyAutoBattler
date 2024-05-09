using UnityEngine;

namespace util
{
    public struct BoundsInt2D
    {
        public Vector2Int min;
        public Vector2Int max;

        public BoundsInt2D(Vector2Int min, Vector2Int max)
        {
            this.min = min;
            this.max = max;
        }

        public BoundsInt2D(Vector3Int min, Vector3Int max)
        {
            this.min = (Vector2Int)min;
            this.max = (Vector2Int)max;
        }

        public BoundsInt2D(BoundsInt bounds3d)
        {
            min = (Vector2Int)bounds3d.min;
            max = (Vector2Int)bounds3d.max;
        }

        public bool Contains(Vector2Int point)
        {
            return point.x >= min.x && point.y >= min.y && point.x <= max.x && point.y <= max.y;
        }

        public Vector2Int Clamp(Vector2Int point)
        {
            return new Vector2Int(
                Mathf.Clamp(point.x, min.x, max.x),
                Mathf.Clamp(point.y, min.y, max.y)
            );
        }

        public Vector2Int Size()
        {
            return max - min + Vector2Int.one;
        }
    }
}