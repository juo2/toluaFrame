UIManager = UIManager or {}
UIManager._instance = nil

local M = class("UIManager")

function UIManager.GetInstance()
    if UIManager._instance == nil then
        UIManager._instance = M.New()
    end
    return UIManager._instance
end

function M:ctor()
    self.AllView = {}
    self.UIRoot = require("Game/Common/Layer/UIRoot")
end


--打开一个窗口UI 
--begin
function M:ShowView(viewId,param,callBack)

    local WindowIDConfig = UIDef.UIWindowID[viewId]
    --local viewData = param.viewData

    if not WindowIDConfig then
        Util.log(viewId.." 未配置，请前往 UIDef.lua 进行配置")
        return
    end
    
    if self.AllView[viewId] then
        self:OpenWindow(viewId)
    else
        require(WindowIDConfig.lua) --require 脚本
        window = self:LoadWindow(WindowIDConfig.path)
        self.AllView[viewId] = window
        self:AddChildLayer(viewId)
        self:OpenWindow(viewId)
    end
end

function M:LoadWindow(path)
    local prefab = CUtil.LoadPrefab(path)
    if prefab == nil then
        Util.log("obj为空")
        return
    end
    obj = UnityEngine.GameObject.Instantiate(prefab)
    return obj
end

function M:AddChildLayer(viewId)
    local WindowIDConfig = UIDef.UIWindowID[viewId]
    local layer = self.UIRoot.GetInstance():GetLayer(WindowIDConfig.layer)
    layer:AddChild(viewId)
end

function M:OpenWindow(viewId)
    local window = self.AllView[viewId]
    Util.dump(window,"window")
    local luaTable = window:GetComponent(typeof(FXGame.LuaBehaviour)):GetInstance()
    if luaTable then
        luaTable:Open()
    end
end

--end
--打开一个窗口UI 

--关闭一个窗口UI 
--begin
function M:HideView(viewId)
    if self.AllView[viewId] then
        local WindowIDConfig = UIDef.UIWindowID[viewId]
        local window = self.AllView[viewId]
        self:RemoveChildLayer(viewId)
        self:CloseWindow(viewId)
    
        GameObject.Destroy(window)
        self.AllView[viewId] = nil
        package.loaded[WindowIDConfig.lua] = nil --卸载lua脚本
    end
end

function M:RemoveChildLayer(viewId)
    local WindowIDConfig = UIDef.UIWindowID[viewId]
    local layer = self.UIRoot.GetInstance():GetLayer(WindowIDConfig.layer)
    layer:RemoveChild(viewId)
end

function M:CloseWindow(viewId)
    local window = self.AllView[viewId]
    local luaTable = window:GetComponent(typeof(FXGame.LuaBehaviour)):GetInstance()
    if luaTable then
        luaTable:Close()
    end
end

--end
--关闭一个窗口UI 
