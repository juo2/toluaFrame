using UnityEngine;
using System;
using LuaInterface;
using System.Collections.Generic;
using UnityEngine.UI;

namespace FXGame
{
    [Serializable]
    public class ObjMap
    {
        public string name;
        public int idx;
        public string compName;
        public GameObject go;
    }

    public class LuaBehaviour : MonoBehaviour
    {
        public string className;
        // Is ready or not.
        private bool m_bReady = false;

        private bool hasAwake = false;

        public List<ObjMap> objs = new List<ObjMap>();

        // The lua behavior.
        private LLuaBehaviourInterface m_cBehavior = new LLuaBehaviourInterface();

        [Tooltip("开启此选项将会 把对象列表 注入到 luaName 对像身上 无需代码Get引用 \n 注意属性名皆以 inje_ + 对象名 开头 如 inje_button1 = 1")]
        public bool injectToLua = true;
        private Dictionary<string, LuaFunction> buttons = new Dictionary<string, LuaFunction>();

        // The awake method.
        void Awake()
        {
            if (!enabled) return;

            if (className == string.Empty)
            {
                return;
            }
            if (!CreateClassInstance(className) || !m_bReady)
            {
                return;
            }

            if (hasAwake == false)
            {
                if (injectToLua) InjectObjectToLua();
                m_cBehavior.Awake();
            }
        }

        private void InjectObjectToLua()
        {
            if (string.IsNullOrEmpty(className)) return;
            LuaTable tab = GetInstance();
            if (null == tab) return;

            int len = objs.Count;
            for (int i = 0; i < len; i++)
            {
                if (null != tab["inje_" + objs[i].name] && null != objs[i].go)
                {
                    if (string.IsNullOrEmpty(objs[i].compName) || objs[i].compName == "GameObject")
                        tab["inje_" + objs[i].name] = objs[i].go;
                    else
                        tab["inje_" + objs[i].name] = objs[i].go.GetComponent(objs[i].compName);
                }
            }
        }

        void ClearInject()
        {
            if (string.IsNullOrEmpty(className)) return;
            LuaTable tab = GetInstance();
            if (null == tab) return;
            int len = objs.Count;
            for (int i = 0; i < len; i++)
            {
                if (null != tab["inje_" + objs[i].name])
                    tab["inje_" + objs[i].name] = null;
            }
        }

        // Use this for initialization
        void Start()
        {
            if (m_bReady)
            {
                m_cBehavior.Start();
            }
        }

        // The destroy event.
        void OnDestroy()
        {
            if (m_bReady)
            {
                m_cBehavior.OnDestroy();
                ClearInject();
            }
        }

        /**
         * Get the lua class instance (Actually a lua table).
         * 
         * @param void.
         * @return LuaTable - The class instance table..
         */
        public LuaInterface.LuaTable GetInstance()
        {
            if (m_bReady == false)
            {
                CreateClassInstance(className);
            }
            return m_cBehavior.GetChunk();
        }

        /**
         * Create a lua class instance for monobehavior instead of do a file.
         * 
         * @param string strFile - The lua class name.
         * @return bool - true if success, otherwise false.
         */
        private bool CreateClassInstance(string strClassName)
        {
            if (!m_cBehavior.CreateClassInstance(strClassName))
            {
                return false;
            }

            // Init variables.
            m_cBehavior.SetData("this", this);
            m_cBehavior.SetData("transform", transform);
            m_cBehavior.SetData("gameObject", gameObject);

            LuaTable tab = m_cBehavior.GetChunk();
            if (null == tab) return false;

            int len = objs.Count;
            for (int i = 0; i < len; i++)
            {
                if (null != tab["inje_" + objs[i].name] && null != objs[i].go)
                {
                    if (string.IsNullOrEmpty(objs[i].compName) || objs[i].compName == "GameObject")
                        tab["inje_" + objs[i].name] = objs[i].go;
                    else
                        tab["inje_" + objs[i].name] = objs[i].go.GetComponent(objs[i].compName);
                }
            }
            m_cBehavior.Awake();

            m_bReady = true;
            hasAwake = true;
            return true;
        }
    }

}
