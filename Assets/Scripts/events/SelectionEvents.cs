using System;
using blocks;
using hand.selectable;

namespace events
{
    public static class SelectionEvents
    {
        public static event Action<Selectable> OnSelect;

        public static void SelectEvent(Selectable selectable)
        {
            OnSelect?.Invoke(selectable);
        }

        public static event Action<Selectable> OnSelected;

        public static void SelectedEvent(Selectable selectable)
        {
            OnSelected?.Invoke(selectable);
        }

        public static event Action<Selectable> OnDeselected;

        public static void DeselectedEvent(Selectable selectable)
        {
            OnDeselected?.Invoke(selectable);
        }

        public static event Action<Selectable> OnRotated;

        public static void RotationEvent(Selectable selectable)
        {
            OnRotated?.Invoke(selectable);
        }
    }
}