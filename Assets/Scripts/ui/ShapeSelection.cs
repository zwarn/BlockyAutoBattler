using System;
using System.Collections.Generic;
using blocks;
using events;
using hand.selectable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Selectable = hand.selectable.Selectable;

namespace ui
{
    public class ShapeSelection : MonoBehaviour, IPointerClickHandler
    {
        public Image image;
        public Image border;

        [Inject] private ShapeCreator _shapeCreator;

        private ShapeSelectable _shapeSelectable = null;

        private void OnEnable()
        {
            SelectionEvents.OnSelected += OnSelection;
            SelectionEvents.OnDeselected += OnDeselection;
        }

        private void OnSelection(Selectable selectable)
        {
            Select(selectable == _shapeSelectable);
        }

        private void OnDeselection(Selectable selectable)
        {
            if (selectable == _shapeSelectable)
            {
                Select(false);
            }
        }

        private void OnDisable()
        {
            SelectionEvents.OnSelected -= OnSelection;
            SelectionEvents.OnDeselected -= OnDeselection;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _shapeSelectable = new ShapeSelectable(CreateNewShape());

            SelectionEvents.SelectEvent(_shapeSelectable);
        }

        private Shape CreateNewShape()
        {
            return _shapeCreator.CreateShape();
        }

        private void Select(bool showBorder)
        {
            border.gameObject.SetActive(showBorder);
        }
    }
}