using System;
using hand;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using util;
using Zenject;

namespace blocks
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TileZoneView : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private HandController _hand;
        [Inject] private Camera _camera;

        private TileZone _tileZone;
        private BoxCollider2D _collider2D;

        public Tilemap tilemap;

        private void Awake()
        {
            _collider2D = GetComponent<BoxCollider2D>();
            _tileZone = CreateTileZone();
            _tileZone.OnTilesChanged += OnTilesChanged;
            _tileZone.OnSingleTileChanged += OnSingleTileChanged;
            OnTilesChanged();
        }

        private TileZone CreateTileZone()
        {
            Bounds colliderBounds = _collider2D.bounds;

            var minCell = tilemap.WorldToCell(colliderBounds.min);
            var maxCell = tilemap.WorldToCell(colliderBounds.max);

            BoundsInt2D bounds = new BoundsInt2D(minCell, maxCell);
            return new TileZone(bounds);
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