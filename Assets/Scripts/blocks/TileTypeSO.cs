using UnityEngine;
using UnityEngine.Tilemaps;

namespace blocks
{
    [CreateAssetMenu(fileName = "TileType", menuName = "Block/TileType", order = 0)]
    public class TileTypeSO : ScriptableObject
    {
        public TileBase tile;
    }
}