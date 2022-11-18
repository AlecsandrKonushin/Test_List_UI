using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Test
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(LayoutElement))]
    public class Tile : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Text descriptionText, numberText;
        
        private RectTransform rectTransform;
        private LayoutElement layoutElement;
        private Canvas canvas;

        public ContainerTiles Container { get; set; }
        public string DescriptionText { get => descriptionText.text; set { descriptionText.text = value; } }
        public string NumberText { get => numberText.text; set => numberText.text = value; }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            layoutElement = GetComponent<LayoutElement>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(canvas == null)
            {
                canvas = GetComponentInParent<Canvas>();
            }

            layoutElement.ignoreLayout = true;

            ListWindow.Instance.BeginDragTile(this);
            Container.RemoveTile(this);
        }

        public void OnDrag(PointerEventData eventData)
        {            
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            layoutElement.ignoreLayout = false;

            ListWindow.Instance.EndDragTile(this);
        }
    }
}