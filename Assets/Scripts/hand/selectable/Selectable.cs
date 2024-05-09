using System.Collections.Generic;
using blocks;
using UnityEngine;

namespace hand.selectable
{
    public abstract class Selectable
    {
        private static readonly Selectable Nothing = new NothingSelected();

        public abstract void Execute(TileZone tileZone, Vector3Int position);
        public abstract IEnumerable<(TileTypeSO Tile, Vector2Int Position)> GetTiles();

        public static Selectable None()
        {
            return Nothing;
        }
    }

    class NothingSelected : Selectable
    {
        public override void Execute(TileZone tileZone, Vector3Int position)
        {
        }

        public override IEnumerable<(TileTypeSO Tile, Vector2Int Position)> GetTiles()
        {
            return new List<(TileTypeSO Tile, Vector2Int Position)>();
        }
    }
}