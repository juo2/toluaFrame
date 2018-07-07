WidgetWindow = class(Window,"WidgetWindow")

WidgetWindow.inje_opt1 = false
WidgetWindow.inje_opt2 = false
WidgetWindow.inje_opt3 = false
WidgetWindow.inje_panel = false

function WidgetWindow:Awake()
    self.btnList = 
    {
        self.inje_opt1,
        self.inje_opt2,
        self.inje_opt3,
    }

    self:AddNode(self.inje_panel)
    for i,v in ipairs(self.btnList) do
        self:AddBtn(v)
    end
end

function WidgetWindow:Open()
    self:super("Window","Open");

end

function WidgetWindow:Close()
    self:super("Window","Close");

end