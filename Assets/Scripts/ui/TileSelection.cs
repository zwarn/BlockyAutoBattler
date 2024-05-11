using System;
using blocks;
using events;
using hand.selectable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Selectable = hand.selectable.Selectable;

namespace ui
{
    public class TileSelection : MonoBehaviour, IPointerClickHandler
    {
        public TileTypeSO tileTypeSO;
        public Image image;
        public Image border;

        private TileSelectable _tileSelection;

        private void OnValidate()
        {
            if (tileTypeSO != null)
            {
                image.sprite = ExtractSprite(tileTypeSO.tile);
            }
        }

        private void OnEnable()
        {
            SelectionEvents.OnSelected += OnSelection;
            SelectionEvents.OnDeselected += OnDeselection;
        }

        private void Start()
        {
            _tileSelection = new TileSelectable(tileTypeSO);
        }

        private void OnSelection(Selectable selectable)
        {
            Select(selectable == _tileSelection);
        }

        private void OnDeselection(Selectable selectable)
        {
            if (selectable == _tileSelection)
            {
                Select(false);
            }
        }

        private void OnDisable()
        {
            SelectionEvents.OnSelected -= OnSelection;
            SelectionEvents.OnDeselected -= OnDeselection;
        }

        private Sprite ExtractSprite(TileBase tileBase)
        {
            if (tileBase is Tile tile)
            {
                return tile.sprite;
            }

            if (tileBase is RuleTile ruleTile)
            {
                return ruleTile.m_DefaultSprite;
            }

            if (tileBase is AnimatedTile animatedTile)
            {
                return animatedTile.m_AnimatedSprites[0];
            }

            return null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SelectionEvents.SelectEvent(_tileSelection);
        }

        private void Select(bool showBorder)
        {
            border.gameObject.SetActive(showBorder);
        }
    }
}