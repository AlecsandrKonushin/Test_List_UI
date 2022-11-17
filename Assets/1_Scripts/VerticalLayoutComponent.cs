using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Test
{
    [RequireComponent(typeof(RectTransform)), ExecuteAlways]
    public class VerticalLayoutComponent : UIBehaviour, ILayoutElement, ILayoutGroup
    {
        [SerializeField] private Vector2 flexibleSize;
        [SerializeField] private int layoutPrioruty;
        [SerializeField] private Vector4 padding;
        [SerializeField] private float spacing;

        [SerializeField] private float animationSpeed = 0.5f;
        [SerializeField] private Ease ease = Ease.OutSine;

        private Vector2 minSize;
        private Vector2 preferredSize;
        private ChildData[] childrenData = Array.Empty<ChildData>();
        protected DrivenRectTransformTracker tracker;

        public float minWidth => minSize.x;
        public float minHeight => minSize.y;
        public float preferredWidth => preferredSize.x;
        public float preferredHeight => preferredSize.y;
        public float flexibleWidth => flexibleSize.x;
        public float flexibleHeight => flexibleSize.y;

        int ILayoutElement.layoutPriority => layoutPrioruty;

        private void GatherChildren()
        {
            childrenData = transform
                .OfType<RectTransform>()
                .Select(MapToChildData)
                .ToArray();

            ChildData MapToChildData(RectTransform childTransform)
            {
                childTransform.TryGetComponent<CanvasGroup>(out var canvasGroup);
                childTransform.TryGetComponent<ILayoutIgnorer>(out var ignorer);

                return new ChildData
                {
                    CanvasGroup = canvasGroup,
                    Transform = childTransform,
                    Ignorer = ignorer
                };
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            GatherChildren();
            SetDirty();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            tracker.Clear();
        }

        private void OnTransformChildrenChanged()
        {
            GatherChildren();
            SetDirty();
        }

        private void SetDirty()
        {
            if (!IsActive()) return;

            if (!CanvasUpdateRegistry.IsRebuildingLayout())
            {
                LayoutRebuilder.MarkLayoutForRebuild((RectTransform)transform);
            }
            else
            {
                StartCoroutine(CoDelayedSetDirty((RectTransform)transform));
            }
        }

        private IEnumerator CoDelayedSetDirty(RectTransform rectTransform)
        {
            yield return null;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            SetDirty();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            SetDirty();
        }

        protected override void OnDidApplyAnimationProperties()
        {
            base.OnDidApplyAnimationProperties();

            SetDirty();
        }

        public void CalculateLayoutInputHorizontal()
        {
            minSize.x = 0;
            preferredSize.x = 0;

            for (int i = 0; i < childrenData.Length; i++)
            {
                ref var childData = ref childrenData[i];

                if (childData.IsIgnored) continue;

                var childMinWidth = LayoutUtility.GetMinWidth(childData.Transform);
                var childPreferredWidth = LayoutUtility.GetPreferredWidth(childData.Transform);

                if (minSize.x < childMinWidth)
                {
                    minSize.x = childMinWidth;
                }

                if (preferredSize.x < childPreferredWidth)
                {
                    preferredSize.x = childPreferredWidth;
                }
            }

            minSize.x += padding.x + padding.z;
            preferredSize.x += padding.x + padding.z;
        }

        public void CalculateLayoutInputVertical()
        {
            minSize.y = 0;
            preferredSize.y = 0;

            for (int i = 0; i < childrenData.Length; i++)
            {
                ref var childData = ref childrenData[i];

                if (childData.IsIgnored) continue;

                var childMinHeight = LayoutUtility.GetMinHeight(childData.Transform);
                var childPreferredHeight = LayoutUtility.GetPreferredHeight(childData.Transform);

                minSize.y += childMinHeight + spacing;
                preferredSize.y += childPreferredHeight + spacing;
            }

            minSize.y += padding.y + padding.w - spacing;
            preferredSize.y += padding.y + padding.w - spacing;
        }

        public void SetLayoutHorizontal() { }

        public void SetLayoutVertical()
        {
            tracker.Clear();
            var size = ((RectTransform)transform).rect.size;
            var y = padding.y;

            for (int i = 0; i < childrenData.Length; i++)
            {
                ref var childData = ref childrenData[i];

                if (childData.IsIgnored) continue;

                var childMinHeight = LayoutUtility.GetMinHeight(childData.Transform);
                var childPreferredHeight = LayoutUtility.GetPreferredHeight(childData.Transform);

                childData.Size.x = size.x - padding.x - padding.z;
                childData.Size.y = childPreferredHeight > childMinHeight ? childPreferredHeight : childMinHeight;

                childData.Position.x = padding.x;
                childData.Position.y = -y;
                y += childData.Size.y + spacing;

                tracker.Add(this, childData.Transform,
                    DrivenTransformProperties.Anchors
                    | DrivenTransformProperties.Pivot
                    | DrivenTransformProperties.AnchoredPosition
                    | DrivenTransformProperties.SizeDelta);
            }

            ApplyChildrenSizes();
        }

        private void ApplyChildrenSizes()
        {
            if (!Application.isPlaying)
            {
                ApplyChildrenSizesImmediate();
            }
            else
            {
                ApplyChildrenSizesAnimated();
            }
        }

        private void ApplyChildrenSizesAnimated()
        {
            for (int i = 0; i < childrenData.Length; i++)
            {
                ref var child = ref childrenData[i];

                if (child.IsIgnored) continue;

                child.Transform.pivot = Vector2.up;
                child.Transform.anchorMin = Vector2.up;
                child.Transform.anchorMax = Vector2.up;

                child.Transform.sizeDelta = child.Size;
                child.Transform.DOKill();

                if (child.Position != child.Transform.anchoredPosition)
                {
                    child.Transform.DOAnchorPos(child.Position, animationSpeed).SetEase(ease);
                }
            }
        }

        private void ApplyChildrenSizesImmediate()
        {
            for (int i = 0; i < childrenData.Length; i++)
            {
                ref var child = ref childrenData[i];

                if (child.IsIgnored) continue;

                child.Transform.anchorMin = Vector2.up;
                child.Transform.anchorMax = Vector2.up;
                child.Transform.pivot = Vector2.up;

                child.Transform.sizeDelta = child.Size;
                child.Transform.anchoredPosition = child.Position;

                if (child.CanvasGroup != null)
                {
                    child.CanvasGroup.alpha = 1;
                }
            }
        }
    }

    internal struct ChildData
    {
        public ILayoutIgnorer Ignorer;
        public RectTransform Transform;
        public CanvasGroup CanvasGroup;
        public Vector2 Position;
        public Vector2 Size;

        public bool IsIgnored => Ignorer?.ignoreLayout ?? false;
    }
}