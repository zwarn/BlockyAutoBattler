using System;
using hand;
using hand.selectable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using util;
using Zenject;

namespace blocks
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TileZoneView : MonoBehaviour, IPointerClickHandler, IPointerMoveHandler, IPointerExitHandler
    {
        [Inject] private HandController _hand;
        [Inject] private Camera _camera;

        private TileZone _tileZone;
        private BoxCollider2D _collider2D;

        public Tilemap tilemap;
        public Tilemap previewTilemap;

        public Vector2Int? CurrentMouseCellPosition = null;

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

            var cellPosition = GetCellPosition(eventData);

            selectable.Interact(_tileZone, cellPosition);
        }

        private Vector3Int GetCellPosition(PointerEventData eventData)
        {
            Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(eventData.position);
            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);
            return cellPosition;
        }

        private Vector3Int ToVec3Int(Vector2Int vec2)
        {
            Vector3Int position = new Vector3Int(vec2.x, vec2.y, 0);
            return position;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            var cellPosition = (Vector2Int)GetCellPosition(eventData);

            if (CurrentMouseCellPosition != cellPosition)
            {
                CurrentMouseCellPosition = cellPosition;
                UpdatePreview();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CurrentMouseCellPosition = null;
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            previewTilemap.gameObject.SetActive(false);
            var selected = _hand.Selectable;
            if (CurrentMouseCellPosition != null && selected != Selectable.None())
            {
                previewTilemap.gameObject.SetActive(true);
                previewTilemap.ClearAllTiles();

                var offset = CurrentMouseCellPosition.Value;

                if (selected is ShapeSelectable shapeSelection)
                {
                    foreach (var pair in shapeSelection.GetShape().GetTilesTranslated(offset))
                    {
                        previewTilemap.SetTile(ToVec3Int(pair.Position), pair.Tile.tile);
                    }
                }
            }
        }
    }
}