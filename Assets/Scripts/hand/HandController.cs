using events;
using hand.selectable;
using JetBrains.Annotations;
using UnityEngine;

namespace hand
{
    public class HandController : MonoBehaviour
    {
        [CanBeNull] public SelectionContainer Selection { get; private set; }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Q)) Rotate(true);

            if (Input.GetKeyUp(KeyCode.E)) Rotate(false);
        }

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