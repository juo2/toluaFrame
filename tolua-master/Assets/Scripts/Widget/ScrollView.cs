using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Widget
{
    public class ScrollView : MonoBehaviour, IDragHandler , IBeginDragHandler , IEndDragHandler
    {
        [SerializeField]
        protected GameObject m_Content;

        [SerializeField]
        protected bool m_Vertical = true;

        [SerializeField]
        protected bool m_Horizontal = false;

        protected RectTransform m_ViewRect;

        protected RectTransform m_ContentRect;

        private Vector2 m_PointerStartLocalCursor = Vector2.zero;

        private Vector2 m_ContentStartPosition = Vector2.zero;

        private Bounds m_ViewBounds;

        private Bounds m_ContentBounds;

        private bool m_Dragging = false;

        void Awake ()
        {
            //this.gameObject
            m_ViewRect = this.gameObject.GetComponent<RectTransform>();
            m_ContentRect = this.m_Content.GetComponent<RectTransform>();

            m_ViewBounds = new Bounds(m_ViewRect.rect.center, m_ViewRect.rect.size);
            m_ContentBounds = new Bounds(m_ContentRect.rect.center, m_ContentRect.rect.size);
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void OnBtnClick()
        {
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_PointerStartLocalCursor = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_ViewRect, eventData.position, eventData.pressEventCamera, out m_PointerStartLocalCursor);
            m_ContentStartPosition = m_ContentRect.anchoredPosition;
            m_Dragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(m_ViewRect, eventData.position, eventData.pressEventCamera, out localCursor))
                return;

            UpdateBounds();
            //手指移动的距离
            Vector2 pointerDelta = localCursor - m_PointerStartLocalCursor;

            Vector2 position = m_ContentStartPosition + pointerDelta;

            //边界检测offset不为0则到达边界
            Vector2 offset = CalculateOffset(position - m_ContentRect.anchoredPosition);
            position += offset;

            SetContentAnchoredPosition(position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_Dragging = false;
        }

        private void SetContentAnchoredPosition(Vector2 position)
        {
            position.x = m_ContentRect.anchoredPosition.x;

            if(position != m_ContentRect.anchoredPosition)
            {
                m_ContentRect.anchoredPosition = position;
            }
        }

        private Vector2 CalculateOffset(Vector2 delta)
        {
            return InternalCalculateOffset(ref m_ViewBounds, ref m_ContentBounds, m_Horizontal, m_Vertical, ref delta);
        }

        internal static Vector2 InternalCalculateOffset(ref Bounds viewBounds, ref Bounds contentBounds, bool horizontal, bool vertical, ref Vector2 delta)
        {
            Vector2 offset = Vector2.zero;

            Vector2 min = contentBounds.min;
            Vector2 max = contentBounds.max;

            if (horizontal)
            {
                min.x += delta.x;
                max.x += delta.x;
                if (min.x > viewBounds.min.x)
                    offset.x = viewBounds.min.x - min.x;
                else if (max.x < viewBounds.max.x)
                    offset.x = viewBounds.max.x - max.x;
            }

            if (vertical)
            {
                min.y += delta.y;
                max.y += delta.y;
                if (max.y < viewBounds.max.y)
                    offset.y = viewBounds.max.y - max.y;
                else if (min.y > viewBounds.min.y)
                    offset.y = viewBounds.min.y - min.y;
            }

            return offset;
        }

        protected void UpdateBounds()
        {
            m_ViewBounds = new Bounds(m_ViewRect.rect.center, m_ViewRect.rect.size);
            m_ContentBounds = GetBounds();

            if (m_Content == null)
                return;

            Vector3 contentSize = m_ContentBounds.size;
            Vector3 contentPos = m_ContentBounds.center;
            var contentPivot = m_ContentRect.pivot;
            InternalUpdateBounds(ref m_ViewBounds, ref contentPivot, ref contentSize, ref contentPos);

            m_ContentBounds.size = contentSize;
            m_ContentBounds.center = contentPos;
        }

        internal static void InternalUpdateBounds(ref Bounds viewBounds, ref Vector2 contentPivot, ref Vector3 contentSize, ref Vector3 contentPos)
        {
            // Make sure content bounds are at least as large as view by adding padding if not.
            // One might think at first that if the content is smaller than the view, scrolling should be allowed.
            // However, that's not how scroll views normally work.
            // Scrolling is *only* possible when content is *larger* than view.
            // We use the pivot of the content rect to decide in which directions the content bounds should be expanded.
            // E.g. if pivot is at top, bounds are expanded downwards.
            // This also works nicely when ContentSizeFitter is used on the content.
            Vector3 excess = viewBounds.size - contentSize;
            if (excess.x > 0)
            {
                contentPos.x -= excess.x * (contentPivot.x - 0.5f);
                contentSize.x = viewBounds.size.x;
            }
            if (excess.y > 0)
            {
                contentPos.y -= excess.y * (contentPivot.y - 0.5f);
                contentSize.y = viewBounds.size.y;
            }
        }

        private readonly Vector3[] m_Corners = new Vector3[4];
        private Bounds GetBounds()
        {
            if (m_Content == null)
                return new Bounds();
            m_ContentRect.GetWorldCorners(m_Corners);
           
            var viewWorldToLocalMatrix = m_ViewRect.worldToLocalMatrix;
            return InternalGetBounds(m_Corners, ref viewWorldToLocalMatrix);
        }

        internal static Bounds InternalGetBounds(Vector3[] corners, ref Matrix4x4 viewWorldToLocalMatrix)
        {
            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            for (int j = 0; j < 4; j++)
            {
                Vector3 v = viewWorldToLocalMatrix.MultiplyPoint3x4(corners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            var bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);
            return bounds;
        }
    }
}
