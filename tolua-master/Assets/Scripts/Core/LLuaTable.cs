using UnityEngine;

using LuaInterface;
using System.Collections;
using System;

// The lua table operator base class.
public class LLuaTable
{
    // The lua table of this behavior.
    private LuaTable m_cLuaTable = null;

    /**
     * Constructor.
     * 
     * @param void.
     * @return void.
     */
    public LLuaTable(LuaTable cTable)
    {
        m_cLuaTable = cTable;
    }

    /**
     * Destructor.
     * 
     * @param void.
     * @return void.
     */
    ~LLuaTable()
    {
        // Dispose table.
        if (null != m_cLuaTable)
        {
            m_cLuaTable.Dispose();
            m_cLuaTable = null;
        }
    }

    // Get if this lua table is valid.
    // true if valid, false is invalid.
    public bool Valid
    {
        get
        {
            return (null != m_cLuaTable);
        }
    }

    /**
     * Get the lua code chunk (table).
     * 
     * @param void.
     * @return LuaTable - The chunk table.
     */
    public LuaTable GetChunk()
    {
        return Valid ? m_cLuaTable : null;
    }

    /**
     * Set lua data to a lua table, used to communiate with other lua files.
     * 
     * @param int nIndex - The index of the table. (Start from 1.).
     * @param object cValue - The value associated to the key.
     * @return void.
     */
    public void SetData(string strName, object cValue)
    {
        if (!Valid || string.IsNullOrEmpty(strName))
        {
            return;
        }

        m_cLuaTable[strName] = cValue;
    }

    /**
     * Call a lua method.
     * 
     * @param ref LuaFunction cResFunc - The result of the function, if the lua function calls ok, if it is not null, will call it instead of look up from table by strFunc.
     * @param string strFunc - The function name.
     * @param object cParam - The param.
     * @return object - The returned value.
     */
    public object CallMethod(ref LuaFunction cResFunc, string strFunc, object cParam)
    {
        // Check function first.
        if (null == cResFunc)
        {
            // Check params.
            if (string.IsNullOrEmpty(strFunc))
            {
                return null;
            }

            // Check table.
            if (!Valid)
            {
                return null;
            }

            // Check function.
            object cFuncObj = m_cLuaTable[strFunc];
            if ((null == cFuncObj) || !(cFuncObj is LuaFunction))
            {
                return null;
            }

            // Get function.
            cResFunc = (LuaFunction)cFuncObj;
            if (null == cResFunc)
            {
                return null;
            }
        }

        // Try to call this method.
        try
        {
            cResFunc.BeginPCall();
            cResFunc.Push(cParam);
            cResFunc.PCall();
            object ret = cResFunc.CheckVariant();
            cResFunc.EndPCall();
            return ret;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            cResFunc.EndPCall();
            cResFunc = null;
            return null;
        }
    }
}
