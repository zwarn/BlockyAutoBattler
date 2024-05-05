using blocks;
using UnityEngine;

namespace hand.selectable
{
    public class TileSelectable : Selectable
    {
        public TileTypeSO TileType { get; }

        public TileSelectable(TileTypeSO tileType)
        {
            TileType = tileType;
        }

        public override void OnSelect()
        {
            
        }

        public override void OnDeselect()
        {
            
        }

        public override void Execute(TileZone tileZone, Vector3Int position)
        {
            tileZone.ChangeTile(position, TileType);
        }
    }
}