local WidgetModule = class(Module,"WidgetModule")

function WidgetModule:ctor()
    
end

function WidgetModule:Open()
    UIManager.GetInstance():ShowView("WidgetWindow",{viewData = self.viewData})
end

return WidgetModule