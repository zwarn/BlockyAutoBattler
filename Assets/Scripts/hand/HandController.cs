using System;
using blocks;
using events;
using UnityEngine;
using Zenject;

namespace hand
{
    public class HandController : MonoBehaviour
    {
        [Inject] private Events _events;

        private TileTypeSO _selectedTile;

        private void OnEnable()
        {
            _events.OnSelectTileType += OnTileTypeSelection;
        }

        private void OnDisable()
        {
            _events.OnSelectTileType -= OnTileTypeSelection;
        }

        private void OnTileTypeSelection(TileTypeSO tileType)
        {
            _selectedTile = tileType;
        }

        public TileTypeSO GetSelection()
        {
            return _selectedTile;
        }
    }
}