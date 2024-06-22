using blocks;
using events;
using hand.selectable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace ui
{
    public class ShapeSelection : MonoBehaviour, IPointerClickHandler
    {
        public Image image;
        public Image border;

        [Inject] private ShapeCreator _shapeCreator;

        private SelectionContainer _shapeSelection;

        private void OnEnable()
        {
            SelectionEvents.OnSelected += OnSelection;
            SelectionEvents.OnDeselected += OnDeselection;
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

        private void OnSelection(SelectionContainer selection)
        {
            Select(selection == _shapeSelection);
        }

        private void OnDeselection(SelectionContainer selection)
        {
            if (selection == _shapeSelection) Select(false);
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