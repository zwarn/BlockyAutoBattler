using System;
using blocks;
using events;
using hand.selectable;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace hand
{
    public class HandController : MonoBehaviour
    {
        public Selectable Selectable { get; private set; } = Selectable.None();

        private void OnEnable()
        {
            SelectionEvents.OnSelect += OnSelect;
        }

        private void OnDisable()
        {
            SelectionEvents.OnSelect -= OnSelect;
        }

        private void OnSelect(Selectable selectable)
        {
            SelectionEvents.DeselectedEvent(Selectable);
            Selectable = selectable;
            SelectionEvents.SelectedEvent(Selectable);
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
            if (Selectable is ShapeSelectable shapeSelectable)
            {
                shapeSelectable.Rotate(clockwise);
                SelectionEvents.RotationEvent(Selectable);
            }
        }
    }
}