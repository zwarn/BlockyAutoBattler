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

        private SelectionContainer _shapeSelection = null;

        private void OnEnable()
        {
            SelectionEvents.OnSelected += OnSelection;
            SelectionEvents.OnDeselected += OnDeselection;
        }

        private void OnSelection(SelectionContainer selection)
        {
            Select(selection == _shapeSelection);
        }

        private void OnDeselection(SelectionContainer selection)
        {
            if (selection == _shapeSelection)
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
            _shapeSelection = new SelectionContainer(CreateNewShape());

            SelectionEvents.SelectEvent(_shapeSelection);
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