Window = class(UIBase,"Window")

function Window:ctor()
    self.__BtnList = {}
    self.__node = false
    self.__PanelList = {}
    self.__LastPanel = false
end

function Window:Open()
    if self.ViewData and self.ViewData.index then
        self:OnClickPanel(self.ViewData.index)
    else
        self:OnClickPanel(1)
    end
end

function Window:Close()
    self:OnClosePanel()
end

function Window:AddNode(node)
    self.__node = node
end

function Window:AddBtn(btn)
    table.insert( self.__BtnList,btn)
    local index = #self.__BtnList
    self.__BtnList[index]:GetComponent(typeof(UnityEngine.UI.Button)).onClick:AddListener(function ()
        if self.__PanelList[index] and self.__PanelList[index] == self.__LastPanel then
            return
        end
        if self.__LastPanel then
            self:OnClosePanel()
        end
        self:OnClickPanel(index)
    end)
end

function Window:PlayFullScreenAnim()

end

function Window:OnClickPanel(index)
    local panelInfo = UIDef.UIPanelID[self.this.className]
    local nodeInfo = panelInfo[index]
    
    local panel = self.__PanelList[index]
    if panel then
        self:_onOpenPanel(panel)
    else
        require(nodeInfo.lua) -- 引入lua脚本
        local prefab = CUtil.LoadPrefab(nodeInfo.path) 
        panel = UnityEngine.GameObject.Instantiate(prefab)
        if self.__node and panel then
            panel.transform:SetParent(self.__node.transform)
			local rt = panel:GetComponent(typeof(UnityEngine.RectTransform))
			rt.anchorMin = Vector2.zero
			rt.anchorMax = Vector2.one 
			rt.offsetMin = Vector2.zero
			rt.offsetMax = Vector2.zero
			panel.transform.localPosition = Vector3.zero
			panel.transform.localScale = Vector3(1,1,1)
			self:_onOpenPanel(panel)
        else
            error("panel为空")
        end 
    end
    self.__PanelList[index] = panel
    self.__LastPanel = panel
end

function Window:_onOpenPanel(panel)
    panel:SetActive(true)
    panel:GetComponent(typeof(FXGame.LuaBehaviour)):GetInstance():Open()
end

function Window:OnClosePanel()
    -- for k,v in pairs(self.__PanelList) do
    --     if v == self.__LastPanel then
    --         self.__PanelList[k] = nil
    --     end
    -- end
    self.__LastPanel:SetActive(false)
    self.__LastPanel:GetComponent(typeof(FXGame.LuaBehaviour)):GetInstance():Close()
end

function Window:GetPanel(index)
    return self.__PanelList[index]
end

function Window:OnDestroy()
    for i,v in ipairs(self.__BtnList) do
        v:GetComponent(typeof(UnityEngine.UI.Button)).onClick:RemoveAllListeners()
    end

    self.__BtnList = {}
    self.__node = false
    self.__PanelList = {}

    for k,v in pairs(UIDef.UIPanelID[self.this.className]) do
        package.loaded[v.lua] = nil --卸载lua脚本
    end
end