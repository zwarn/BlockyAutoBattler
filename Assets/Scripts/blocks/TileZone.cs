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
            var selection = _hand.GetSelection();
            if (selection == null)
            {
                return;
            }
            
            Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(eventData.position);
            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);
            
            tilemap.SetTile(cellPosition, selection.tile);

        }
    }
}