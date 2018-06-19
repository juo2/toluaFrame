using UnityEngine;
using System.Collections.Generic;
using System;


/// <summary>
/// 管理器单例基类
/// </summary>
public class Manager<T> : MonoBehaviour where T : Component
{
	private static Dictionary<System.Type, Component> m_managers = new Dictionary<System.Type, Component>();
	public static T Instance
	{
		get
		{
			Type type = typeof(T);
			if (!m_managers.ContainsKey(type))
			{
				Manager<T> manager = GameObject.FindObjectOfType(type) as Manager<T>;
				if (null == manager)
				{
					GameObject go = GameObject.Find(AppConst.AppEntrance);
					if (null == go)
					{
						go = new GameObject(AppConst.AppEntrance);
						DontDestroyOnLoad(go);
					} 
					manager = go.AddComponent(type) as Manager<T>;
				}
				if (!m_managers.ContainsKey(type))
					m_managers.Add(type, manager);
			}
			return (T)m_managers[type];
		}
	}

	public virtual void Awake()
	{
		Component m = Instance as Component;
	}
}

