using events;
using util;

namespace hand.selectable
{
    public class SelectionContainer
    {
        public readonly ISelectable Value;

        public int Rotation { get; private set; } = 0;

        public SelectionContainer(ISelectable value)
        {
            Value = value;
        }

        public void Rotate(bool clockwise)
        {
            int delta = clockwise ? 1 : -1;
            Rotation = (Rotation + delta).Mod(4);
        }
    }
}