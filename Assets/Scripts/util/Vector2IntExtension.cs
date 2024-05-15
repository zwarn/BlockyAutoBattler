using System;
using UnityEngine;

namespace util
{
    public static class Vector2IntExtension
    {
        public static Vector2Int Rotate(this Vector2Int vector, int rotation)
        {
            switch (rotation)
            {
                case 0: return vector;
                case 1: return new Vector2Int(vector.y, -vector.x);
                case 2: return new Vector2Int(-vector.x, -vector.y);
                case 3: return new Vector2Int(-vector.y, vector.x);
                default: throw new ArgumentException($"unknown rotation {rotation}");
            }
        }
    }
}