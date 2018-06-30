using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaManager : Manager<LuaManager> {

    private LuaState lua;

    private LuaLooper loop = null;

    public void Init()
    {
        this.InitLua();
        this.InitMain();
        this.StartLooper();
    }

    public void InitLua()
    {
        lua = new LuaState();
        this.OpenLibs();

        lua.LuaSetTop(0);
        LuaBinder.Bind(lua);
        DelegateFactory.Init();   
        LuaCoroutine.Register(lua, this);
        lua.Start();
    }

    public void InitMain()
    {
        lua.DoFile("Game/Main.lua");
        LuaFunction main = lua.GetFunction("Main");
        main.Call();
        main.Dispose();
        main = null;
    }

    void StartLooper() {
        loop = gameObject.AddComponent<LuaLooper>();
        loop.luaState = lua;
    }

    public LuaTable GetLuaTable(string name)
    {
        return lua.GetTable(name);
    }

    private void OpenLibs()
    {
        lua.OpenLibs(LuaDLL.luaopen_socket_core);
        this.OpenLuaSocket();
    }

    private void OpenLuaSocket()
    {
        LuaConst.openLuaSocket = true;

        lua.BeginPreLoad();
        lua.RegFunction("socket.core", LuaOpen_Socket_Core);
        lua.RegFunction("mime.core", LuaOpen_Mime_Core);
        lua.EndPreLoad();
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int LuaOpen_Socket_Core(IntPtr L)
    {
        return LuaDLL.luaopen_socket_core(L);
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int LuaOpen_Mime_Core(IntPtr L)
    {
        return LuaDLL.luaopen_mime_core(L);
    }
}
