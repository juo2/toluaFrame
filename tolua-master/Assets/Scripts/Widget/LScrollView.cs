/****************************************************************************
Copyright (c) 2015 Lingjijian

Created by Lingjijian on 2015

342854406@qq.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using System.Security;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Lui
{
    public enum ScrollDirection
    {
        HORIZONTAL,
        VERTICAL,
        BOTH
    }
    /// <summary>
    /// 滑块
    /// </summary>
	public class LScrollView : MonoBehaviour, IBeginDragHandler, IDragHandler , IEndDragHandler
    {
        public static int INVALID_INDEX = -1;
        public float RELOCATE_DURATION = 0.2f;
        public float AUTO_RELOCATE_SPPED = 100.0f;
        public float INERTANCE_SPEED = 0.96f;
		public float RESISTANCE_SPEED = 0.65f;
        public float LIMIT_VALUE = 1.0f;

        public GameObject container;

        public bool bounceable;
        protected GameObject _tweenMaker;
        public ScrollDirection direction;
        private Vector2 lastMovePoint;
        private Vector2 maxOffset;
        private Vector2 minOffset;
        protected Vector2 scrollDistance;
        public bool dragable;
        private bool _isPicking;
        public bool pickEnable;
		public bool inertanceEnable;
		private float _scrollPerc;
		private bool _isInertanceFinish;
		private bool _isDraging;
		private bool _hasDragBegin;

        protected float _rectWidth;
        protected float _rectHeight;
        protected float _containerWidth;
        protected float _containerHeight;
        protected Vector2 _containerLocalPosition;

        public bool isAlignCell = true;
        [HideInInspector]
        public GameObject curPickObj;

        public delegate T0 LDataSourceAdapter<T0, T1>(T0 arg0, T1 arg1);
        public UnityAction onMoveCompleteHandler;
        public UnityAction<float> onScrollingHandler;
        public UnityAction onScrollBeginHandler;
        public UnityAction onDraggingScrollEndedHandler;
        public UnityAction OnInertanceEnd;
        public UnityAction<GameObject> onPickBeginHandler;
        public UnityAction<Vector3> onPickIngHandler;
        public UnityAction<GameObject> onPickEndHandler;

        public LScrollView()
        {
            direction = ScrollDirection.BOTH;
            lastMovePoint = Vector2.zero;
            bounceable = true;
            scrollDistance = Vector2.zero;
            dragable = true;
            maxOffset = Vector2.zero;
            minOffset = Vector2.zero;
        }

        void Awake()
        {
            _tweenMaker = new GameObject();
            _tweenMaker.name = "tweenMaker";
            _tweenMaker.transform.SetParent(this.transform);
            _tweenMaker.transform.localPosition = Vector3.zero;

            updateLimitOffset();
        }

        void Start()
        {
            // LNoDrawingView noDrawView = this.GetComponent<LNoDrawingView>();
            // if(noDrawView==null){
            //     if(this.gameObject.GetComponent<Image>() == null)
            //         this.gameObject.AddComponent<LNoDrawingView>();
            // }
            Vector2 size = container.GetComponent<RectTransform>().sizeDelta;
            setContainerSize(size);
        }

        public void setContainerSize(Vector2 size)
        {
            Vector2 cs = GetComponent<RectTransform>().rect.size;
            int width = Mathf.Max((int)cs.x, (int)size.x);
            int height = Mathf.Max((int)cs.y, (int)size.y);
            _containerWidth = width;
            _containerHeight = height;
            _rectWidth = this.GetComponent<RectTransform>().rect.width;
            _rectHeight = this.GetComponent<RectTransform>().rect.height;

            if(container!=null)
                container.GetComponent<RectTransform>().sizeDelta = new Vector2(_containerWidth,_containerHeight);

            updateLimitOffset();
        }

        protected void updateLimitOffset()
        {
            Vector2 size = new Vector2(_rectWidth,_rectHeight);
            Vector2 innSize = new Vector2(_containerWidth,_containerHeight);
            maxOffset.x = 0;
            minOffset.x = size.x - innSize.x;

            maxOffset.y = 0;
            minOffset.y = size.y - innSize.y;

            if (direction == ScrollDirection.HORIZONTAL)
            {
                minOffset.y = 0;
            }else if (direction == ScrollDirection.VERTICAL)
            {
                minOffset.x = 0;
            }
        }
		
		public void Update()
        {
			if (_hasDragBegin == false) return;
            if (dragable == false) return;
			if (_isDraging) return;
            if (inertanceEnable)
            {
                Vector2 offset = getContentOffset() + scrollDistance;
                if (validateOffset(ref offset)){
					if (Mathf.Abs (scrollDistance.x) >= LIMIT_VALUE || Mathf.Abs (scrollDistance.y) >= LIMIT_VALUE) {
						scrollDistance *= RESISTANCE_SPEED;
						setContentOffsetWithoutCheck (getContentOffset () + scrollDistance);

						_isInertanceFinish = false;

					} else {
						if (!_isInertanceFinish) {
							_isInertanceFinish = true;
                            if (OnInertanceEnd != null)
                            {
                                OnInertanceEnd.Invoke();
                            }
                            scrollDistance = Vector2.zero;
						    relocateContainerWithoutCheck(offset);
						}
					}
				}else{
					if (Mathf.Abs (scrollDistance.x) >= LIMIT_VALUE || Mathf.Abs (scrollDistance.y) >= LIMIT_VALUE) {
						scrollDistance *= INERTANCE_SPEED;
						setContentOffsetWithoutCheck (getContentOffset () + scrollDistance);

						_isInertanceFinish = false;

					} else {
						if (!_isInertanceFinish) {
							_isInertanceFinish = true;

                            scrollDistance = Vector2.zero;
                            if(isAlignCell)
							    onDraggingScrollEnded ();
						}
					}
				}
            }
        }
		
		public void OnBeginDrag(PointerEventData eventData)
        {
            if(_isDraging) return;
			_hasDragBegin = true;

            Vector2 point = transform.InverseTransformPoint(eventData.position);
            if (dragable)
            {
                lastMovePoint = point;

				scrollDistance = Vector2.zero;
				_isDraging = true;
                LeanTween.cancel(_tweenMaker);
                
                onScrollBegin();
                onScrolling();
            }

            if (pickEnable)
            {
                _isPicking = false;
            }
        }

        
        public void OnDrag(PointerEventData eventData)
        {
            if (pickEnable && _isPicking)
            {
                if (onPickIngHandler != null)
                    onPickIngHandler.Invoke(eventData.position);
                return;
            }
                
            Vector2 point = transform.InverseTransformPoint(eventData.position);
            if (dragable)
            {
                scrollDistance = point - lastMovePoint;
                lastMovePoint = point;

                if(pickEnable && (Mathf.Abs(scrollDistance.y) > Mathf.Abs(scrollDistance.x)))
                {
                    _isPicking = true;
                    if (onPickBeginHandler != null)
                    {
                        onPickBeginHandler.Invoke(curPickObj);
                    }
                    return;
                }

                switch (direction)
                {
                    case ScrollDirection.HORIZONTAL:
                        scrollDistance.y = 0;
                        // scrollDistance.x = Mathf.Min(Mathf.Max(scrollDistance.x,-50),50);
                        break;
                    case ScrollDirection.VERTICAL:
                        scrollDistance.x = 0;
                        // scrollDistance.y = Mathf.Min(Mathf.Max(scrollDistance.y,-50),50);
                        break;
                    default:
                        break;
                }

				Vector2 vec = getContentOffset () + scrollDistance;
				if (validateOffset (ref vec)) {
					if (direction == ScrollDirection.VERTICAL) {
						scrollDistance.y *= 0.2f;
					} else if (direction == ScrollDirection.HORIZONTAL) {
						scrollDistance.x *= 0.2f;
					}
				}
                setContentOffsetWithoutCheck(getContentOffset() + scrollDistance);
            }
        }

        
		public void OnEndDrag(PointerEventData eventData)
        {
            if (dragable)
            {
				_isDraging = false;

                Vector2 offset = getContentOffset();
                if (validateOffset(ref offset))
                {
                    scrollDistance = Vector2.zero;
                    relocateContainerWithoutCheck(offset);
                }
                else
                {
					if(!inertanceEnable)
                    	onDraggingScrollEnded();
                }
            }

            if (pickEnable)
            {
                _isPicking = false;

                if (onPickEndHandler != null)
                    onPickEndHandler.Invoke(curPickObj);

                curPickObj = null;
            }
        }

        protected void relocateContainerWithoutCheck(Vector2 offset)
        {
            setContentOffsetEaseInWithoutCheck(offset, RELOCATE_DURATION);
        }

        protected void relocateContainer()
        {
            Vector2 offset = getContentOffset();
            if (validateOffset(ref offset))
            {
                setContentOffsetEaseInWithoutCheck(offset, RELOCATE_DURATION);
            }
        }

        protected void setContentOffsetEaseInWithoutCheck(Vector2 offset, float duration)
        {
            LeanTween.cancel(_tweenMaker);
            LeanTween.value(_tweenMaker,_containerLocalPosition, offset, duration)
                .setEase(LeanTweenType.easeInQuad)
                .setOnUpdate((Vector2 val) => {
                    _containerLocalPosition = val;
                    if(container!=null)
                        container.transform.localPosition = val;
                    onScrolling();
                })
                .setOnComplete(onMoveComplete);
                
            // onScrolling();
        }

        protected void setContentOffsetEaseIn(Vector2 offset, float duration, float rate)
        {
            validateOffset(ref offset);
            
            setContentOffsetEaseInWithoutCheck(offset, duration);
        }

        public void setContentOffset(Vector2 offset)
        {
			validateOffset (ref offset);

            _containerLocalPosition = offset;
            if(container!=null)
                container.transform.localPosition = offset;
			onScrolling();
        }

        public void setContentOffsetWithoutCheck(Vector2 offset)
        {
            _containerLocalPosition = offset;
            if(container!=null)
                container.transform.localPosition = offset;
            onScrolling();
        }

        public void setContentOffsetToTop()
        {
            if (direction == ScrollDirection.VERTICAL)
            {
                Vector2 point = new Vector2(0, -(_containerHeight - _rectHeight));
                setContentOffset(point);
            }
        }

        public void setContentOffsetToBottom()
        {
            if (direction == ScrollDirection.VERTICAL)
            {
                setContentOffset(maxOffset);
            }
        }

        public void setContentOffsetToRight()
        {
	        if( direction == ScrollDirection.HORIZONTAL )
	        {
		        setContentOffset(minOffset);
	        }
        }

        public void setContentOffsetToLeft()
        {
            if (direction == ScrollDirection.HORIZONTAL )
	        {
		        setContentOffset(maxOffset);
	        }
        }

        public void setContentOffsetInDuration(Vector2 offset, float duration)
        {
            if (bounceable)
            {
                validateOffset(ref offset);
            }
            setContentOffsetInDurationWithoutCheck(offset, duration);
        }

        public void setContentOffsetInDurationWithoutCheck(Vector2 offset, float duration)
        {
			LeanTween.cancel(_tweenMaker);
            LeanTween.value(_tweenMaker,_containerLocalPosition, offset, duration)
                .setOnUpdate((Vector2 val) => { 
                    _containerLocalPosition = val;
                    if(container!=null)
                        container.transform.localPosition = val;
                    onScrolling(); 
                })
                .setOnComplete(onMoveComplete);
        }

        protected bool validateOffset(ref Vector2 point)
        {
            float x = point.x, y = point.y;
            x = Mathf.Max(x, minOffset.x);
            x = Mathf.Min(x, maxOffset.x);
            y = Mathf.Max(y, minOffset.y);
            y = Mathf.Min(y, maxOffset.y);

            if (point.x != x || point.y != y)
            {
                point.x = x;
                point.y = y;
                return true;
            }

            point.x = x;
            point.y = y;
            return false;
        }

        public Vector2 getContentOffset()
        {
            return _containerLocalPosition;
        }

        protected void onMoveComplete()
        {
            if (onMoveCompleteHandler != null)
            {
                onMoveCompleteHandler.Invoke();
            }
        }

        protected virtual void onScrolling()
		{
			if (onScrollingHandler!=null){
                this._scrollPerc = 0.0f;
                if (direction == ScrollDirection.HORIZONTAL) {
                    float width = _rectWidth;
                    this._scrollPerc = _containerWidth - width == 0 ? 0 : -_containerLocalPosition.x / (_containerWidth - width);

                } else if(direction == ScrollDirection.VERTICAL) {
                    
                    float height = _rectHeight;
                    this._scrollPerc = _containerHeight - height == 0 ? 0 : -_containerLocalPosition.y / (_containerHeight - height);
                }
				onScrollingHandler.Invoke (this._scrollPerc);
            }
        }

        protected virtual void onScrollBegin()
        {
            if (onScrollBeginHandler !=null)
            {
                onScrollBeginHandler.Invoke();
            }
        }

        protected virtual void onDraggingScrollEnded()
        {
            if (onDraggingScrollEndedHandler!=null)
            {
                onDraggingScrollEndedHandler.Invoke();
            }
        }

        public void stopMoveing()
        {
            scrollDistance = Vector2.zero;
            LeanTween.cancel(_tweenMaker);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            this.stopMoveing();
        }

    }

}
