_G.TableView = class(ScrollView,"TableView")

TableView.inje_Button = false
TableView.inje_Prefab = false

TableView._positions = {}
TableView._cellUseList = {}
TableView._cellPoolList = {}
TableView._cellUseIndices = {}

TableView._cellsSize = Vector2(100,100)
TableView._cellCount = nil
TableView._oldCellCount = nil
TableView._first = nil
TableView.onCellHandle = nil

TableView.INVALID_INDEX = -1

function TableView:Awake()
    self:super("ScrollView","Awake")

    self.inje_Button.onClick:AddListener(function ()
        self:OnClick()
    end)

    if self.inje_Prefab then
        self.inje_Prefab:SetActive(false)
    end
end

function TableView:SetCellCountAndSize(cellCount,size)
    self._cellsSize = size
    self:SetCellCount(cellCount)
end

function TableView:SetCellCount(cellCount)
    self._oldCellCount = self._cellCount
    self._cellCount = cellCount
end

function TableView:SetCellHandle(handle)
    self.onCellHandle = handle
end

function TableView:OnClick()
    self:ReloadData()
end

function TableView:ReloadData()
    self:ClearData()    
    self:_updatePositions()
    self:_reloadJump()
    
    self:_onScrolling()
end

function TableView:AddNode()

end 

function TableView:RemoveNode()

end

function TableView:_updatePositions()
    if self.Horizontal then
        for i=1,self._cellCount do
            local pos = Vector2(self._cellsSize.x * (i-1),0)
            table.insert( self._positions,pos)
        end
        self:SetContentSize(Vector2(self._cellsSize.x * self._cellCount,self._cellsSize.y),true)
    elseif self.Vertical then
        local height = self._cellsSize.y * (self._cellCount - 1)
        height = math.max(height,self._viewRect.sizeDelta.y)
        local i = self._cellCount
        while i > 0 do
            local pos = Vector2(0,height)
            table.insert( self._positions,pos)
            height = height - self._cellsSize.y
            i = i - 1
        end
        self:SetContentSize(Vector2(self._cellsSize.x,self._cellCount * self._cellsSize.y),true)
    end
end

function TableView:_onScrolling()
    self:super("ScrollView","_onScrolling");

    if self._cellCount == 0 then
        return 
    end
    local beiginIdx = self:_cellBeginIndexFromOffset()
    local endIdx = self:_cellEndIndexFromOffset()

    self:_removeCellInvisible()
    self:_addCellVisible()
    self:_updateCellPosition()
end

--把看不见到cell放入对象池
function TableView:_removeCellInvisible()
    local beiginIdx = self:_cellBeginIndexFromOffset()
    local endIdx = self:_cellEndIndexFromOffset()

    for k,v in pairs(self._cellUseList) do
        if v.index < beiginIdx or v.index > endIdx then
            if self:_IsUseContainIndex(k) then
                self:_removeFromUse(k)
            end
        end
    end
end

function TableView:_removeFromUse(index)
    local cell = self._cellUseList[index]
    cell:Reset()
    table.insert( self._cellPoolList,cell) 
    self._cellUseList[index] = nil
    self._cellUseIndices[index] = nil
end

--把看得见到cell从对象池取出，或创建
function TableView:_addCellVisible()
    local beiginIdx = self:_cellBeginIndexFromOffset()
    local endIdx = self:_cellEndIndexFromOffset()

    for i=beiginIdx,endIdx do
        if not self:_IsUseContainIndex(i) then
            self:_addCellUse(i)
        end
    end
end

function TableView:_addCellUse(index)
    local cell = self:_getCellFromPool()
    if cell then
        cell:Awake(index,self._positions[index])
    else
        local prefab = UnityEngine.GameObject.Instantiate(self.inje_Prefab)
        prefab.transform:SetParent(self.transform)
        cell = TableViewCell.New(
            {
                index = index,
                position = self._positions[index],
                size = self._cellsSize,
                object = prefab,
            }
        )
    end
    
    self._cellUseIndices[index] = true
    self._cellUseList[index] = cell

    if self.onCellHandle then
        self.onCellHandle(index,cell.object)
    end
end

function TableView:_updateCellPosition()
    for k,v in pairs(self._cellUseList) do
        v:UpdatePosition(self:GetContentOffset())
    end
end

function TableView:_IsUseContainIndex(index)
    return self._cellUseIndices[index]
end

--从对象池抽取一个元素
function TableView:_getCellFromPool()
    local index = next(self._cellPoolList)
    local cell = nil
    if index then
        cell = self._cellPoolList[index]
        self._cellPoolList[index] = nil
    end

    return cell
end

function TableView:_cellBeginIndexFromOffset()
    local index = self.INVALID_INDEX
    if self._cellCount == 0 then
        return index
    end

    local contentOffset = self:GetContentOffset()
    if self.Horizontal then
        local offset = math.max(0,-contentOffset.x / self._cellsSize.x)
        index = math.floor(offset) + 1
    elseif self.Vertical then
        local offset = math.max(0,contentOffset.y + self._innHeight - self._viewRect.sizeDelta.y)  
        index = math.floor( offset / self._cellsSize.y ) + 1
    end

    return index
end

function TableView:_cellEndIndexFromOffset()
    local index = self.INVALID_INDEX
    if self._cellCount == 0 then
        return index
    end

    local contentOffset = self:GetContentOffset()
    if self.Horizontal then
        local offset = math.max(0,contentOffset.x + self._innWidth - self._viewRect.sizeDelta.x)  
        index = self._cellCount - math.floor( offset / self._cellsSize.x )
    elseif self.Vertical then
        local offset = math.max( 0,-contentOffset.y / self._cellsSize.y)  
        index = self._cellCount - math.floor(offset)
    end

    return index
end

function TableView:_reloadJump()
    if self._first == nil then
        self._first = true
    elseif self._first then
        self._first = false
    end

    if self._first then
        self:SetTop()
    else
        self:_setOriginPosition()
    end
end

function TableView:_setOriginPosition()
    if self._oldCellCount  then
        local offsetCount = self._cellCount - self._oldCellCount
        if self.Vertical then
            local offset = math.min( 0,self:GetContentOffset().y - offsetCount * self._cellsSize.y )
            self:SetContentOffset(Vector2(0,offset))
        end
        if self.Horizontal then
            local offset = math.max(self:GetContentOffset().x,self._viewRect.sizeDelta.x - self._innWidth)
            self:SetContentOffset(Vector2(offset,0))
        end
    end
end

function TableView:ClearData()
    self._positions = {}
    for k,v in pairs(self._cellUseList) do
        self:_removeFromUse(k)
    end
end
