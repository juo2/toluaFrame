using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LSrollView : MonoBehaviour, IBeginDragHandler, IDragHandler , IEndDragHandler{

	[SerializeField]
	protected bool m_Vertical = true;

	[SerializeField]
	protected bool m_Horizontal = false;

	[SerializeField]
	protected GameObject m_Content;

	protected GameObject m_TweenMaker;

	protected bool m_IsDraging;

	protected bool m_IsDragable;

	private RectTransform m_ViewRect;

	private RectTransform m_ContentRect;

	public LSrollView()
	{

	}

	void Awake()
	{
		m_TweenMaker = new GameObject();
		m_TweenMaker.name = "TweenMaker";
		m_TweenMaker.transform.SetParent(this.transform);
		m_TweenMaker.transform.localPosition = Vector3.zero;
	}

	public void SetContentSize(Vector2 size)
	{
		Vector2 cs = GetComponent<RectTransform>().rect.size;
		int width = Mathf.Max((int)cs.x, (int)size.x);
		int height = Mathf.Max((int)cs.y, (int)size.y);

		if(m_Content!=null)
		{
			m_ContentRect = m_Content.GetComponent<RectTransform> ();
			m_ContentRect.sizeDelta = new Vector2(width,height);
			m_ViewRect = this.GetComponent<RectTransform>();
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		m_IsDraging = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if(m_IsDragable)
		{

		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		m_IsDraging = false;
	}
}
