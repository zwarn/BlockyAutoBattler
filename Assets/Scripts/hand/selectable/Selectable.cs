using System.Collections.Generic;
using blocks;
using UnityEngine;

namespace hand.selectable
{
    public abstract class Selectable
    {
        private static readonly Selectable Nothing = new NothingSelected();

        public abstract void Interact(TileZone tileZone, Vector2Int position);

        public static Selectable None()
        {
            return Nothing;
        }
    }

    class NothingSelected : Selectable
    {
        public override void Interact(TileZone tileZone, Vector2Int position)
        {
        }

    }
}