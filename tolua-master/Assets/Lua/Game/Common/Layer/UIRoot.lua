
UIRoot = UIRoot or {}
UIRoot._instance = nil

require("Game/Common/Layer/UILayer")
require("Game/Common/Layer/ViewLayer")
require("Game/Common/Layer/TopLayer")

local M = class("UIRoot")

function UIRoot.GetInstance() 
    if UIRoot._instance == nil then
        UIRoot._instance = M.New()
    end
    return UIRoot._instance
end

function M:ctor()
    self.Layers = {}
    self.RootObject = nil
    self:CreatRoot()
end

function M:CreatRoot()
    self.RootObject = UnityEngine.GameObject.New("UIRoot")
    self.RootObject.layer = LayerMask.NameToLayer("UI")
    self.RootObject:AddComponent(typeof(UnityEngine.RectTransform))

    local cameraObject = UnityEngine.GameObject.New("UICamera")
    local camera = cameraObject:AddComponent(typeof(UnityEngine.Camera))
    camera.orthographicSize = 360 * (UnityEngine.Screen.height / 720)
    camera.clearFlags = UnityEngine.CameraClearFlags.SolidColor
    camera.orthographic = true
    camera.nearClipPlane = -8000
    camera.farClipPlane = 8000
    cameraObject.transform:SetParent(self.RootObject.transform)
    cameraObject.transform.localPosition = Vector3.zero
    local canvas = self.RootObject:AddComponent(typeof(UnityEngine.Canvas))
    canvas.renderMode = UnityEngine.RenderMode.ScreenSpaceCamera
    canvas.worldCamera = camera

    local cs = self.RootObject:AddComponent(typeof(UnityEngine.UI.CanvasScaler))
    cs.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize
    cs.referenceResolution = Vector2.New(GameConfig.maxWidth,GameConfig.maxHeight)
    cs.screenMatchMode = UnityEngine.UI.CanvasScaler.ScreenMatchMode.MatchWidthOrHeight
    cs.matchWidthOrHeight = 1 -- 高对齐

    self.RootObject:AddComponent(typeof(UnityEngine.UI.GraphicRaycaster))

    local eventObj = UnityEngine.GameObject.New("EventSystem")
    eventObj.layer = LayerMask.NameToLayer("UI")
    eventObj.transform:SetParent(self.RootObject.transform)
    eventObj:AddComponent(typeof(UnityEngine.EventSystems.EventSystem))
    eventObj:AddComponent(typeof(UnityEngine.EventSystems.StandaloneInputModule))

    for i,v in ipairs(UIDef.UILayerInfos) do
        self:CreateLayer(i,v)
    end
end

function M:CreateLayer(index,LayerInfo)
    local go = UnityEngine.GameObject.New(LayerInfo.name)
    local rt = go:AddComponent(typeof(UnityEngine.RectTransform))
    rt.anchorMin = Vector2.zero
    rt.anchorMax = Vector2.one 
    rt.offsetMin = Vector2.zero
    rt.offsetMax = Vector2.zero
    go.transform:SetParent(self.RootObject.transform)

    local canvas = go:AddComponent(typeof(UnityEngine.Canvas))
    go:AddComponent(typeof(UnityEngine.UI.GraphicRaycaster));
    go.layer = LayerMask.NameToLayer("UI")
    go.transform.localPosition = Vector2.zero
    
    canvas.overrideSorting = true
    canvas.sortingOrder = index * 500

    local layer = _G[LayerInfo.class].New({gameObject = go,index = index,name = LayerInfo.name})
    self.Layers[index] = layer;
end

function M:GetLayer(name)
    local index = nil
    for k,v in pairs(UIDef.UILayerInfos) do
        if v.name == name then
            index = k
        end
    end

    if not index then
        Util.log("没有该层")
        return 
    end

    return self.Layers[index]
end

return UIRoot