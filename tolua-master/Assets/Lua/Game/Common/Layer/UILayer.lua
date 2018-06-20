_G.UILayer = class("UILayer")

function UILayer:ctor(data)

    self.Childs = {}

    if data then
        self.index = data.index
        self.name = data.name
        self.gameObject = data.gameObject
    end
end

function UILayer:AddChild(viewId)
    local display = UIManager.GetInstance().AllView[viewId]
    if not display or table.include(self.Childs,display) then
        return
    end

    display:SetActive(true)
    display.transform:SetParent(self.gameObject.transform,false)
    display.transform:SetAsLastSibling()

    self.Childs[viewId] = display
end

function UILayer:RemoveChild(viewId)
    local display = UIManager.GetInstance().AllView[viewId]
    if not display then
        return
    end
    
    self.Childs[viewId] = nil
end