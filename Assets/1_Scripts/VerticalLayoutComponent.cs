using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Test
{
    [RequireComponent(typeof(RectTransform)), ExecuteAlways]
    public class VerticalLayoutComponent : UIBehaviour, ILayoutElement
    {
        [SerializeField] private Vector2 flexibleSize;
        [SerializeField] private int layoutPrioruty;
        [SerializeField] private Vector4 padding;
        [SerializeField] private float spacing;

        private Vector2 minSize;
        private Vector2 preferredSize;
        private ChildData[] childrenData = Array.Empty<ChildData>();

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
                childTransform.TryGetComponent<ILayoutIgnorer>(out var ignorer);

                return new ChildData
                {
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
    }

    internal struct ChildData
    {
        public ILayoutIgnorer Ignorer;
        public RectTransform Transform;
        public Vector2 Position;
        public Vector2 Size;

        public bool IsIgnored => Ignorer?.ignoreLayout ?? false;
    }
}