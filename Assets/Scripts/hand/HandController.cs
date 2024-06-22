using events;
using hand.selectable;
using JetBrains.Annotations;
using UnityEngine;
using util;

namespace hand
{
    public class HandController : MonoBehaviour
    {
        [CanBeNull] public SelectionContainer Selection { get; private set; } = null;

        private void OnEnable()
        {
            SelectionEvents.OnSelect += OnSelect;
        }

        private void OnDisable()
        {
            SelectionEvents.OnSelect -= OnSelect;
        }

        private void OnSelect(SelectionContainer selectable)
        {
            SelectionEvents.DeselectedEvent(Selection);
            Selection = selectable;
            SelectionEvents.SelectedEvent(Selection);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                Rotate(true);
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                Rotate(false);
            }
        }

        private void Rotate(bool clockwise)
        {
            if (Selection != null)
            {
                Selection.Rotate(clockwise);
                SelectionEvents.RotationEvent(Selection);
            }
        }
    }
}