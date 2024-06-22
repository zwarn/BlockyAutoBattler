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
        public Tilemap tilemap;
        public Tilemap selectionTilemap;
        public Tilemap previewTilemap;
        public TileBase borderTile;
        [Inject] private Camera _camera;
        private BoxCollider2D _collider2D;
        [Inject] private HandController _hand;

        private TileZone _tileZone;

        public Vector2Int? CurrentMouseCellPosition;
        private Action<SelectionContainer> UpdatePreviewAction;
        private Action<SelectionContainer> UpdateSelectionAction;

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

            UpdateSelectionAction = _ => UpdateSelection();
            SelectionEvents.OnSelected += UpdateSelectionAction;
            SelectionEvents.OnDeselected += UpdateSelectionAction;
            OnTilesChanged();
        }

        private void OnDestroy()
        {
            _tileZone.OnTilesChanged -= OnTilesChanged;
            _tileZone.OnSingleTileChanged -= OnSingleTileChanged;
            SelectionEvents.OnSelected -= UpdatePreviewAction;
            SelectionEvents.OnDeselected -= UpdatePreviewAction;
            SelectionEvents.OnRotated -= UpdatePreviewAction;

            SelectionEvents.OnSelected -= UpdateSelectionAction;
            SelectionEvents.OnDeselected -= UpdateSelectionAction;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var selection = _hand.Selection;

            var cellPosition = (Vector2Int)GetCellPosition(eventData);

            if (selection != null)
            {
                HandlePlacement(selection, cellPosition);
            }
            else
            {
                HandlePickup(cellPosition);
            }
        }

        private void HandlePickup(Vector2Int cellPosition)
        {
            var shape = _tileZone.GetShape(cellPosition);
            if (shape != null)
            {
                _tileZone.RemoveShape(cellPosition);
                SelectionEvents.SelectEvent(new SelectionContainer(shape));
            }
        }

        private void HandlePlacement(SelectionContainer selection, Vector2Int cellPosition)
        {
            if (_tileZone.Place(selection, cellPosition))
            {
                SelectionEvents.SelectEvent(null);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CurrentMouseCellPosition = null;
            UpdatePreview();
            UpdateSelection();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            var cellPosition = (Vector2Int)GetCellPosition(eventData);

            if (CurrentMouseCellPosition != cellPosition)
            {
                CurrentMouseCellPosition = cellPosition;
                UpdatePreview();
                UpdateSelection();
            }
        }

        private TileZone CreateTileZone()
        {
            var colliderBounds = _collider2D.bounds;

            var minCell = tilemap.WorldToCell(colliderBounds.min);
            var maxCell = tilemap.WorldToCell(colliderBounds.max);

            var bounds = new BoundsInt2D(minCell, maxCell);
            return new TileZone(bounds);
        }

        private void OnSingleTileChanged(TileTypeSO tileType, Vector2Int position)
        {
            tilemap.SetTile(ToVec3Int(position), tileType.tile);
            UpdateSelection();
        }

        private void OnTilesChanged()
        {
            tilemap.ClearAllTiles();
            foreach (var pair in _tileZone.GetTiles()) tilemap.SetTile(ToVec3Int(pair.Position), pair.Tile.tile);
            UpdateSelection();
        }

        private Vector3Int GetCellPosition(PointerEventData eventData)
        {
            var mouseWorldPos = _camera.ScreenToWorldPoint(eventData.position);
            var cellPosition = tilemap.WorldToCell(mouseWorldPos);
            return cellPosition;
        }

        private Vector3Int ToVec3Int(Vector2Int vec2)
        {
            var position = new Vector3Int(vec2.x, vec2.y, 0);
            return position;
        }

        private void UpdateSelection()
        {
            selectionTilemap.gameObject.SetActive(false);
            var selected = _hand.Selection;

            if (CurrentMouseCellPosition == null || selected != null)
            {
                return;
            }

            var position = CurrentMouseCellPosition.Value;
            previewTilemap.gameObject.SetActive(true);
            previewTilemap.ClearAllTiles();

            var shape = _tileZone.GetShape(position);

            if (shape == null)
            {
                return;
            }

            foreach (var pair in shape.GetTilesTranslated(position))
                previewTilemap.SetTile(ToVec3Int(pair.Position), borderTile);
        }

        private void UpdatePreview()
        {
            previewTilemap.gameObject.SetActive(false);
            var selected = _hand.Selection;
            if (CurrentMouseCellPosition == null || selected == null)
            {
                return;
            }
            
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

        private void PreviewShape(IEnumerable<(TileTypeSO Tile, Vector2Int Position)> tiles)
        {
            foreach (var pair in tiles) previewTilemap.SetTile(ToVec3Int(pair.Position), pair.Tile.tile);
        }

        private void PreviewTile(TileTypeSO tile, Vector2Int offset)
        {
            previewTilemap.SetTile(ToVec3Int(offset), tile.tile);
        }
    }
}