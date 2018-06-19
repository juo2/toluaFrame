GameControllers = GameControllers or {}

for k,v in pairs(ModuleConfig) do
    GameControllers[k] = require(v.controller).New(k)
end