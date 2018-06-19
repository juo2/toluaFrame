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
    self.allView = {}
end

function M:ShowView(viewId,param,callBack)
    local WindowIDConfig = UIDef.UIWindowID[viewId]
    local viewData = param.viewData

    if not WindowIDConfig then
        Util.log(viewId.." 未配置，请前往 UIDef.lua 进行配置")
        return
    end
    
    
end

function M:HideView(viewId)

end
