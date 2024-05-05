using System;
using blocks;
using UnityEngine;

namespace events
{
    public class Events
    {
        public event Action<TileTypeSO> OnSelectTileType;

        public void SelectTileTypeEvent(TileTypeSO tileType)
        {
            OnSelectTileType?.Invoke(tileType);
        }
    }
}