DataManager = DataManager or {}

function DataManager:GetData(moduleType)
    if DataManager[moduleType] then
        DataManager[moduleType] = require(ModuleConfig[moduleType])
    else
        return DataManager[moduleType]
    end
end

