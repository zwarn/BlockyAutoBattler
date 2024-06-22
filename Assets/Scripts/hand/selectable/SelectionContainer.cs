using util;

namespace hand.selectable
{
    public class SelectionContainer
    {
        public readonly ISelection Value;

        public SelectionContainer(ISelection value)
        {
            Value = value;
        }

        public int Rotation { get; private set; }

        public void Rotate(bool clockwise)
        {
            var delta = clockwise ? 1 : -1;
            Rotation = (Rotation + delta).Mod(4);
        }
    }
}