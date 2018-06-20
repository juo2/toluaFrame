LoginWindow = class(Window,"LoginWindow")

LoginWindow.inje_InputField = false
LoginWindow.inje_Button = false
LoginWindow.inje_panel = false

function LoginWindow:Awake()
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

function LoginWindow:Open()
    self:super("Window","Open");

    Util.log("LoginWindow:Open")
end

function LoginWindow:Close()
    self:super("Window","Close");

    Util.log("LoginWindow:Close")
end