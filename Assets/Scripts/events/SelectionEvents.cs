using System;
using hand.selectable;

namespace events
{
    public static class SelectionEvents
    {
        public static event Action<SelectionContainer> OnSelect;

        public static void SelectEvent(SelectionContainer selection)
        {
            OnSelect?.Invoke(selection);
        }

        public static event Action<SelectionContainer> OnSelected;

        public static void SelectedEvent(SelectionContainer selection)
        {
            OnSelected?.Invoke(selection);
        }

        public static event Action<SelectionContainer> OnDeselected;

        public static void DeselectedEvent(SelectionContainer selection)
        {
            OnDeselected?.Invoke(selection);
        }

        public static event Action<SelectionContainer> OnRotated;

        public static void RotationEvent(SelectionContainer selection)
        {
            OnRotated?.Invoke(selection);
        }
    }
}