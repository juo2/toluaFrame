using UnityEngine;
using UnityEditor;
using FXGame;
[CustomEditor(typeof(LuaBehaviour))]
public class LBEditor : Editor
{
    SerializedProperty m_luaName;
    SerializedProperty m_inject;
    SerializedProperty m_injectToLua;
    void OnEnable()
    {
        m_luaName = serializedObject.FindProperty("className");
        m_inject = serializedObject.FindProperty("objs");
        m_injectToLua = serializedObject.FindProperty("injectToLua");
        if (m_inject.arraySize < 1)
        {
            InserNullItem(0);
            serializedObject.ApplyModifiedProperties();
        }   
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(m_luaName.displayName, GUILayout.Width(100f));
        m_luaName.stringValue = EditorGUILayout.TextField(m_luaName.stringValue);
        m_injectToLua.boolValue = EditorGUILayout.Toggle(m_injectToLua.boolValue, GUILayout.Width(25f));
        EditorGUILayout.LabelField(m_injectToLua.displayName, GUILayout.Width(100f));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        if (m_inject.arraySize > 0)
        {
            for (int i = 0; i < m_inject.arraySize; i++)
            {
                DrawItem(i);
            }
        }
        //if(GUILayout.Button("+"))
        //{
        //    InserNullItem(m_inject.arraySize);
        //}
        EditorGUILayout.Space();
        serializedObject.ApplyModifiedProperties();
        //base.OnInspectorGUI();
    }


    void DrawItem(int i)
    {
        SerializedProperty item = m_inject.GetArrayElementAtIndex(i);
        SerializedProperty name = item.FindPropertyRelative("name");
        SerializedProperty comp = item.FindPropertyRelative("compName");
        SerializedProperty idx = item.FindPropertyRelative("idx");
        SerializedProperty go = item.FindPropertyRelative("go");

        Color hbcolor = GUI.backgroundColor;
        if (null == go.objectReferenceValue)
            GUI.backgroundColor = Color.red;
        EditorGUILayout.BeginHorizontal("Box");
        GUI.backgroundColor = hbcolor;

        EditorGUI.BeginDisabledGroup(i == m_inject.arraySize - 1);
        if (GUILayout.Button("↓", GUILayout.Width(20f)))
        {
            m_inject.MoveArrayElement(i, i + 1);
        }
        EditorGUI.EndDisabledGroup();

        EditorGUI.BeginDisabledGroup(i == 0);
        if (GUILayout.Button("↑", GUILayout.Width(20f)))
        {
            m_inject.MoveArrayElement(i, i - 1);
        }
        EditorGUI.EndDisabledGroup();


        go.objectReferenceValue = EditorGUILayout.ObjectField(go.objectReferenceValue, typeof(Object), true);

        GameObject ggo = go.objectReferenceValue as GameObject;
        string[] compsStr = new string[0];
        int pidx = 0;
        if (null != ggo)
        {
            ArrayUtility.Add<string>(ref compsStr, "GameObject");
            Component[] comps = ggo.GetComponents<Component>();
            for (int j = 0; j < comps.Length; j++)
            {
                Component citem = comps[j];
                string cname = citem.GetType().Name;
                if (cname == comp.stringValue) pidx = j + 1;
                ArrayUtility.Add<string>(ref compsStr, cname);
            }
        }

        //if (idx.intValue == 0 && compsStr.Length > 0) idx.intValue = 0;
        if (pidx != idx.intValue){ idx.intValue = pidx;} 

        idx.intValue = EditorGUILayout.Popup(idx.intValue, compsStr);
        if (compsStr.Length > 0)
        {
            if (GUI.changed)
            {
                comp.stringValue = compsStr[idx.intValue];
            }
        }

        if (string.IsNullOrEmpty(name.stringValue) && null != ggo) name.stringValue = ggo.name;
        name.stringValue = EditorGUILayout.TextField(name.stringValue);

        EditorGUI.BeginDisabledGroup(m_inject.arraySize < 2);
        if (GUILayout.Button("-", GUILayout.Width(20f)))
        {
            m_inject.DeleteArrayElementAtIndex(i);
        }
        EditorGUI.EndDisabledGroup();

        Color color = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("+", GUILayout.Width(20f)))
        {
            InserNullItem(i + 1);
        }
        GUI.backgroundColor = color;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
    }

    void InserNullItem(int idx)
    {
        m_inject.InsertArrayElementAtIndex(idx);
        SerializedProperty item = m_inject.GetArrayElementAtIndex(idx);
        item.FindPropertyRelative("name").stringValue = "";
        item.FindPropertyRelative("compName").stringValue = "";
        item.FindPropertyRelative("idx").intValue = 0;
        item.FindPropertyRelative("go").objectReferenceValue = null;
    }
}
