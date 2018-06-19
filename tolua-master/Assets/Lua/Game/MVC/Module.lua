Module = class("Module")

function Module:ctor()
    self.isOpen = false
end

function Module:Open()
    self.isOpen = true
end

function Module:Close()
    self.isOpen = false
end
