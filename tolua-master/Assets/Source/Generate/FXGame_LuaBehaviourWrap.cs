﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FXGame_LuaBehaviourWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FXGame.LuaBehaviour), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("OnBeginDrag", OnBeginDrag);
		L.RegFunction("OnDrag", OnDrag);
		L.RegFunction("OnEndDrag", OnEndDrag);
		L.RegFunction("GetInstance", GetInstance);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("className", get_className, set_className);
		L.RegVar("objs", get_objs, set_objs);
		L.RegVar("injectToLua", get_injectToLua, set_injectToLua);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnBeginDrag(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FXGame.LuaBehaviour obj = (FXGame.LuaBehaviour)ToLua.CheckObject<FXGame.LuaBehaviour>(L, 1);
			UnityEngine.EventSystems.PointerEventData arg0 = (UnityEngine.EventSystems.PointerEventData)ToLua.CheckObject<UnityEngine.EventSystems.PointerEventData>(L, 2);
			obj.OnBeginDrag(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDrag(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FXGame.LuaBehaviour obj = (FXGame.LuaBehaviour)ToLua.CheckObject<FXGame.LuaBehaviour>(L, 1);
			UnityEngine.EventSystems.PointerEventData arg0 = (UnityEngine.EventSystems.PointerEventData)ToLua.CheckObject<UnityEngine.EventSystems.PointerEventData>(L, 2);
			obj.OnDrag(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnEndDrag(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FXGame.LuaBehaviour obj = (FXGame.LuaBehaviour)ToLua.CheckObject<FXGame.LuaBehaviour>(L, 1);
			UnityEngine.EventSystems.PointerEventData arg0 = (UnityEngine.EventSystems.PointerEventData)ToLua.CheckObject<UnityEngine.EventSystems.PointerEventData>(L, 2);
			obj.OnEndDrag(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetInstance(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FXGame.LuaBehaviour obj = (FXGame.LuaBehaviour)ToLua.CheckObject<FXGame.LuaBehaviour>(L, 1);
			LuaInterface.LuaTable o = obj.GetInstance();
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_className(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FXGame.LuaBehaviour obj = (FXGame.LuaBehaviour)o;
			string ret = obj.className;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index className on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_objs(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FXGame.LuaBehaviour obj = (FXGame.LuaBehaviour)o;
			System.Collections.Generic.List<FXGame.ObjMap> ret = obj.objs;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index objs on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_injectToLua(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FXGame.LuaBehaviour obj = (FXGame.LuaBehaviour)o;
			bool ret = obj.injectToLua;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index injectToLua on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_className(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FXGame.LuaBehaviour obj = (FXGame.LuaBehaviour)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.className = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index className on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_objs(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FXGame.LuaBehaviour obj = (FXGame.LuaBehaviour)o;
			System.Collections.Generic.List<FXGame.ObjMap> arg0 = (System.Collections.Generic.List<FXGame.ObjMap>)ToLua.CheckObject(L, 2, typeof(System.Collections.Generic.List<FXGame.ObjMap>));
			obj.objs = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index objs on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_injectToLua(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FXGame.LuaBehaviour obj = (FXGame.LuaBehaviour)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.injectToLua = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index injectToLua on a nil value");
		}
	}
}

