using System.Collections.Generic;
using System.Linq;
using blocks;
using events;
using UnityEngine;

namespace hand.selectable
{
    public class ShapeSelectable : Selectable
    {
        protected readonly Shape Shape;

        public ShapeSelectable(Shape shape)
        {
            Shape = shape;
        }

        public override void Interact(TileZone tileZone, Vector3Int position)
        {
            bool placed = tileZone.PlaceShape(Shape.GetTilesTranslated((Vector2Int)position));
            if (placed)
            {
                SelectionEvents.SelectEvent(None());
            }
        }

        public Shape GetShape()
        {
            return Shape;
        }
    }
}