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

        private SelectionContainer _tileSelection;

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
            _tileSelection = new SelectionContainer(tileTypeSO);
        }

        private void OnSelection(SelectionContainer selection)
        {
            Select(selection == _tileSelection);
        }

        private void OnDeselection(SelectionContainer selection)
        {
            if (selection == _tileSelection)
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