using System;
using hand;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Zenject;

namespace blocks
{
    public class TileZoneView : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private HandController _hand;
        [Inject] private Camera _camera;

        private TileZone _tileZone;

        public Tilemap tilemap;

        private void Awake()
        {
            _tileZone = new TileZone();
            _tileZone.OnTilesChanged += OnTilesChanged;
            _tileZone.OnSingleTileChanged += OnSingleTileChanged;
            OnTilesChanged();
        }

        private void OnSingleTileChanged(TileTypeSO tileType, Vector2Int position)
        {
            tilemap.SetTile(ToVec3Int(position), tileType.tile);
        }

        private void OnTilesChanged()
        {
            tilemap.ClearAllTiles();
            foreach (var pair in _tileZone.GetTiles())
            {
                tilemap.SetTile(ToVec3Int(pair.Position), pair.Tile.tile);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var selectable = _hand.Selectable;

            Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(eventData.position);
            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

            selectable.Execute(_tileZone, cellPosition);
        }

        private Vector3Int ToVec3Int(Vector2Int vec2)
        {
            Vector3Int position = new Vector3Int(vec2.x, vec2.y, 0);
            return position;
        }
    }
}