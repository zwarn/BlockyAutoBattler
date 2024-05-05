using hand;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Zenject;

namespace blocks
{
    public class TileZone : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private HandController _hand;
        [Inject] private Camera _camera;
        public Tilemap tilemap;

        public void OnPointerClick(PointerEventData eventData)
        {
            var selectable = _hand.Selectable;

            Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(eventData.position);
            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

            selectable.Execute(this, cellPosition);
        }

        public void ChangeTile(Vector3Int position, TileTypeSO tileType)
        {
            tilemap.SetTile(position, tileType.tile);
        }
    }
}