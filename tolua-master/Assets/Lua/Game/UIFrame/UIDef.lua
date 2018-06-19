UIDef = UIDef or {}

UIDef.UILayerType = 
{
    ViewLayer = "ViewLayer",
}

UIDef.UIWindowID = 
{
    LoginWindow = {path = "Prefabs/Login/LoginWindow",layer = UILayerType.ViewLayer},
}

UIDef.UIPanelID = 
{
    LoginWindow = 
    {
        LoginPanel1 = {path = "Prefabs/Login/LoginPanel1"},
        LoginPanel2 = {path = "Prefabs/Login/LoginPanel2"},
    }
}
