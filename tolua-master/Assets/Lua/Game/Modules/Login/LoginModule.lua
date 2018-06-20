local LoginModule = class(Module,"LoginModule")

function LoginModule:ctor()
    
end

function LoginModule:Open()
    UIManager.GetInstance():ShowView("LoginWindow",{viewData = self.viewData})
end

return LoginModule