using System;
using blocks;
using events;
using hand.selectable;
using UnityEngine;
using Zenject;

namespace hand
{
    public class HandController : MonoBehaviour
    {
        public Selectable Selectable { get; private set;  } = Selectable.None();

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
            Selectable.OnDeselect();
            SelectionEvents.DeselectedEvent(Selectable);
            Selectable = selectable;
            Selectable.OnSelect();
            SelectionEvents.SelectedEvent(Selectable);
        }

    }
}