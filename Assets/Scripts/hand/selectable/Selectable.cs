using blocks;
using UnityEngine;

namespace hand.selectable
{
    public abstract class Selectable
    {
        private static readonly Selectable Nothing = new NothingSelected();
        
        public abstract void OnSelect();

        public abstract void OnDeselect();

        public abstract void Execute(TileZone tileZone, Vector3Int position);
        
        public static Selectable None()
        {
            return Nothing;
        }
    }

    class NothingSelected : Selectable
    {
        public override void OnSelect()
        {
        }

        public override void OnDeselect()
        {
        }

        public override void Execute(TileZone tileZone, Vector3Int position)
        {
        }
    }
}