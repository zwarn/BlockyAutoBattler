using System;
using blocks;
using events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Zenject;

namespace ui
{
    public class TileSelection : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private Events _events;

        public TileTypeSO tileTypeSO;
        public Image image;
        public Image border;

        private void OnValidate()
        {
            if (tileTypeSO != null)
            {
                image.sprite = ExtractSprite(tileTypeSO.tile);
            }
        }

        private void OnEnable()
        {
            _events.OnSelectTileType += OnSelection;
        }

        private void OnDisable()
        {
            _events.OnSelectTileType -= OnSelection;
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
            _events.SelectTileTypeEvent(tileTypeSO);
        }

        private void OnSelection(TileTypeSO tileType)
        {
            border.gameObject.SetActive(tileType == tileTypeSO);
        }
    }
}