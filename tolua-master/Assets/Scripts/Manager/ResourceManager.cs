using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManager : Manager<ResourceManager> {

    public GameObject LoadInstantiate<T>(string path)
    {
        GameObject go = (GameObject)LoadExecute(path, typeof(T));
        return go;
    }

    public System.Object LoadExecute(string path,Type type)
    {
        System.Object obj = null;

        obj = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/" + path, type);

        if(obj == null)
        {
            Debug.Log("UnityEditor.AssetDatabase.LoadAssetAtPath：path 不存在");
        }

        return obj;
    }
}
