using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Test
{
    [RequireComponent(typeof(RectTransform))]
    public class Tile : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Text descriptionText, numberText;

        private ListWindow listWindow;
        private ContainerTiles myContainer;
        private RectTransform rectTransform;
        private Canvas canvas;

        public ContainerTiles SetContainer { set => myContainer = value; }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetData(ListWindow listWindow, string description, int number)
        {
            this.listWindow = listWindow;

            descriptionText.text = description;
            numberText.text = number.ToString();

            canvas = GetComponentInParent<Canvas>();
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            listWindow.BeginDragTile(this);
            myContainer.RemoveTile(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            listWindow.EndDragTile(this);
        }
    }
}