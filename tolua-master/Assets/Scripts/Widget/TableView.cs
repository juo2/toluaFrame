using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Widget
{
    public class TableViewCell
    {
        public GameObject cell;
        public Vector3 pos;
        public int idx;
    }

    public class TableView : ScrollView
    {
        [SerializeField]
        private int m_CellsCount = 0;

        [SerializeField]
        private GameObject m_CellPrefab;

        [SerializeField]
        private Vector2 m_Size;

        //当前元素的列表
        private List<TableViewCell> m_UseCellList;

        //复用元素的列表
        private List<TableViewCell> m_ReuseCellList;

		private List<Vector2> m_PositionList;

        protected UnityAction<int, GameObject> onCellHandle;

        void Awake()
        {
            m_UseCellList = new List<TableViewCell>();
            m_ReuseCellList = new List<TableViewCell>();
			m_PositionList = new List<Vector2> ();
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void SetCellsCount(int count)
        {
            m_CellsCount = count;
        }

        public void Reload()
        {
			for (int i = 0; i < m_CellsCount; i++) 
			{
				Vector2 v2 = new Vector2 (0, -m_Size.y * i);
				m_PositionList.Add (v2);
			}
        }

        public void Clear()
        {
            m_UseCellList.Clear();
        }

        void _newCellObject(int index)
        {
            TableViewCell viewCell;
            GameObject obj;

            int reuseListCount = m_ReuseCellList.Count;
            if (reuseListCount == 0)
            {
                viewCell = new TableViewCell();
                obj = GameObject.Instantiate(m_CellPrefab) as GameObject;
                viewCell.cell = obj;
            }
            else
            {
                viewCell = m_ReuseCellList[reuseListCount - 1];
                obj = viewCell.cell;
            }

            Vector2 position = new Vector2(m_Size.x, index * m_Size.y);
            obj.GetComponent<RectTransform>().anchoredPosition = position;
            
            viewCell.idx = index;
            viewCell.pos = position;
        }
    }
}
