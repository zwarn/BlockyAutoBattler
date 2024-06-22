using System;
using System.Collections.Generic;
using events;
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
        private Action<SelectionContainer> UpdatePreviewAction;

        public Tilemap tilemap;
        public Tilemap previewTilemap;

        public Vector2Int? CurrentMouseCellPosition = null;

        private void Awake()
        {
            _collider2D = GetComponent<BoxCollider2D>();
            _tileZone = CreateTileZone();
            _tileZone.OnTilesChanged += OnTilesChanged;
            _tileZone.OnSingleTileChanged += OnSingleTileChanged;
            UpdatePreviewAction = _ => UpdatePreview();
            SelectionEvents.OnSelected += UpdatePreviewAction;
            SelectionEvents.OnDeselected += UpdatePreviewAction;
            SelectionEvents.OnRotated += UpdatePreviewAction;
            OnTilesChanged();
        }

        private void OnDestroy()
        {
            _tileZone.OnTilesChanged -= OnTilesChanged;
            _tileZone.OnSingleTileChanged -= OnSingleTileChanged;
            SelectionEvents.OnSelected -= UpdatePreviewAction;
            SelectionEvents.OnDeselected -= UpdatePreviewAction;
            SelectionEvents.OnRotated -= UpdatePreviewAction;
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
            var selection = _hand.Selection;

            var cellPosition = GetCellPosition(eventData);

            _tileZone.Place(selection, (Vector2Int)cellPosition);
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
            var selected = _hand.Selection;
            if (CurrentMouseCellPosition != null && selected != null)
            {
                previewTilemap.gameObject.SetActive(true);
                previewTilemap.ClearAllTiles();

                var value = selected.Value;
                var offset = CurrentMouseCellPosition.Value;

                switch (value)
                {
                    case TileTypeSO tile:
                        PreviewTile(tile, offset);
                        break;
                    case Shape shape:
                        PreviewShape(shape.GetTilesTranslatedAndRotated(offset, selected.Rotation));
                        break;
                    default:
                        throw new ArgumentException($"Unknown type {value.GetType()} for previewing selection");
                }
            }
        }

        private void PreviewShape(IEnumerable<(TileTypeSO Tile, Vector2Int Position)> tiles)
        {
            foreach (var pair in tiles)
            {
                previewTilemap.SetTile(ToVec3Int(pair.Position), pair.Tile.tile);
            }
        }

        private void PreviewTile(TileTypeSO tile, Vector2Int offset)
        {
            previewTilemap.SetTile(ToVec3Int(offset), tile.tile);
        }
    }
}