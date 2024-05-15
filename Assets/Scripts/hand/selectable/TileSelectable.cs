using System.Collections.Generic;
using blocks;
using UnityEngine;

namespace hand.selectable
{
    public class TileSelectable : ShapeSelectable
    {
        public TileTypeSO TileType { get; }

        public TileSelectable(TileTypeSO tileType) : base(new Shape(new Dictionary<Vector2Int, TileTypeSO>
            { { Vector2Int.zero, tileType } }))
        {
            TileType = tileType;
        }

        public override void Interact(TileZone tileZone, Vector3Int position)
        {
            tileZone.PlaceTile(TileType, (Vector2Int)position);
        }
    }
}