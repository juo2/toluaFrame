WidgetWindow = class(Window,"WidgetWindow")

WidgetWindow.inje_InputField = false
WidgetWindow.inje_Button = false
WidgetWindow.inje_panel = false

function WidgetWindow:Awake()
    self.btnList = 
    {
        self.inje_InputField,
        self.inje_Button,
    }

    self:AddNode(self.inje_panel)
    for i,v in ipairs(self.btnList) do
        self:AddBtn(v)
    end
end

function WidgetWindow:Open()
    self:super("Window","Open");

    Util.log("WidgetWindow:Open")
end

function WidgetWindow:Close()
    self:super("Window","Close");

    Util.log("WidgetWindow:Close")
end