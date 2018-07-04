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
    WidgetWindow = {
        path = "Prefabs/WidgetTest/WidgetWindow.prefab",
        layer = UIDef.UILayerType.ViewLayer,
        lua = "Game/Modules/WidgetTest/View/WidgetWindow",
        screenType = UIDef.ScreenType.FullSceen
    },
}

UIDef.UIPanelID = 
{
    WidgetWindow = 
    {
        {
            path = "Prefabs/WidgetTest/ScrollViewTest.prefab",
            lua = "Game/Modules/WidgetTest/View/ScrollViewTest",
        },
        {
            path = "Prefabs/WidgetTest/TableViewTest.prefab",
            lua = "Game/Modules/WidgetTest/View/TableViewTest",
        },
    },
}