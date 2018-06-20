UIDef = UIDef or {}

UIDef.UILayerType = 
{
    ViewLayer = "ViewLayer",
    TopLayer = "TopLayer",
}

UIDef.UILayerInfos = 
{
    {name="ViewLayer",class = "ViewLayer"},   --UI层
    {name="TopLayer",class = "TopLayer"},    --顶层
}

UIDef.ScreenType = 
{
    NoFullSceen = "NoFullSceen",
    FullSceen = "FullSceen",
}

UIDef.UIWindowID = 
{
    LoginWindow = {
        path = "Prefabs/Login/LoginWindow.prefab",
        layer = UIDef.UILayerType.ViewLayer,
        lua = "Game/Modules/Login/View/LoginWindow",
        screenType = UIDef.ScreenType.FullSceen
    },
}

UIDef.UIPanelID = 
{
    LoginWindow = 
    {
        {
            path = "Prefabs/Login/LoginPanel1.prefab",
            lua = "Game/Modules/Login/View/LoginPanel1",
        },
        {
            path = "Prefabs/Login/LoginPanel2.prefab",
            lua = "Game/Modules/Login/View/LoginPanel2",
        },
    },
}