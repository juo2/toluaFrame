Controller = class("Controller")

function Controller:ctor()
    self:initServerEvent()
end

function Controller:initServerEvent()
    error("没有重载改方法")
end