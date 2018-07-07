_G.GridCell = class("GridCell")

GridCell.INVALID_INDEX = -1
GridCell.index = GridCell.INVALID_INDEX
GridCell.object = nil
GridCell.position = Vector2.zero
GridCell.size = Vector2.zero

function GridCell:ctor(tab)
    self.index = tab.index
    self.position = tab.position
    self.size = tab.size
    self.object = tab.object

    if self.object then
        self.object.transform.localScale = Vector3.one
        local rect = self.object:GetComponent(typeof(UnityEngine.RectTransform))
        rect.sizeDelta = self.size
        rect.pivot = Vector2.zero
    end

    self:Awake(self.index,self.position)
end

function GridCell:Awake(index,position)
    self.index = index
    self.position = position
    if self.object then
        self.object:SetActive(true)
        self.object.transform.localPosition = position
    end
end

function GridCell:UpdatePosition(pos)
    if self.object then
        self.object.transform.localPosition = pos + self.position
    end
end

function GridCell:Reset()
    self.index = self.INVALID_INDEX
    self.object:SetActive(false)
end