using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUtil {

	public static void Log(string str)
	{
		Debug.Log(str);
	}

    public static UnityEngine.GameObject LoadPrefab(string path)
    {
        return ResourceManager.Instance.LoadInstantiate<GameObject>(path);
    }
}
