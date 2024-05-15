using System.Collections.Generic;
using System.Linq;
using blocks;
using events;
using UnityEngine;
using util;

namespace hand.selectable
{
    public class ShapeSelectable : Selectable
    {
        protected readonly Shape Shape;
        public int Rotation { get; private set; } = 0;

        public ShapeSelectable(Shape shape)
        {
            Shape = shape;
        }


        public override void Interact(TileZone tileZone, Vector2Int position)
        {
            bool placed = tileZone.PlaceShape(GetTilesTranslatedAndRotated(position));
            if (placed)
            {
                SelectionEvents.SelectEvent(None());
            }
        }

        public IEnumerable<(TileTypeSO Tile, Vector2Int Position)> GetTilesTranslatedAndRotated(Vector2Int offset)
        {
            return Shape.GetTilesTranslatedAndRotated(offset, Rotation);
        }

        public void Rotate(bool clockwise)
        {
            int delta = clockwise ? 1 : -1;
            Rotation = (Rotation + delta).Mod(4);
        }
    }
}