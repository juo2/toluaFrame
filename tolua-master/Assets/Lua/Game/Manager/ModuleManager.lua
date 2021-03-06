ModuleManager = ModuleManager or {}

--打开模块
function ModuleManager:OpenModule(moduleType,viewData)
    local module  = self:GetModule(moduleType)
    module.viewData = viewData
    module:Open()
    return module
end

--关闭模块
function ModuleManager:CloseModule(moduleType)
    local module = self:GetModule(moduleType)
    module:Close()
end

--判断模块是否开启
function ModuleManager:IsOpenModule(moduleType)
    local module = self:GetModule(moduleType)
    return module.isOpen
end

function ModuleManager:GetModule(moduleType)
    if ModuleManager[moduleType] then
        return ModuleManager[moduleType]
    else
        ModuleManager[moduleType] = require(ModuleConfig[moduleType].module).New()
        if  not ModuleManager[moduleType] then
            log("模块没有配置")
        else
            return ModuleManager[moduleType]
        end
    end
end