_G.TableViewCell = class(TableViewCell)
TableViewCell.INVALID_INDEX = -1
TableViewCell.index = TableViewCell.INVALID_INDEX
TableViewCell.object = nil
TableViewCell.position = Vector2.zero

function TableViewCell:ctor(index,position,size,object)
    self.index = index
    self.position = position
    self.object = object


    self.object:SetActive(true)
end

function TableViewCell:Reset()
    self.index = self.INVALID_INDEX
    self.object:SetActive(false)
end

_G.TableView = class(ScrollView,"TableView")

TableView.inje_Button = false

TableView._positions = {}
TableView._cellUseList = {}
TableView._cellPoolList = {}

TableView._cellsSize = Vector2(100,100)
TableView._cellCount = 8

TableView.INVALID_INDEX = -1

function TableView:Awake()
    self:super("ScrollView","Awake")

    self.inje_Button.onClick:AddListener(function ()
        self:OnClick()
    end)

end

function TableView:SetCellCount(cellCount)
    self._cellCount = cellCount
end

function TableView:SetData()
    
end

function TableView:OnClick()
    local beigin = self:_cellBeginIndexFromOffset()
    local endIdx = self:_cellEndIndexFromOffset()

    Util.dump(beigin,"beigin")
    Util.dump(endIdx,"endIdx")
end

function TableView:ReloadData()
    
    self:ClearData()    
    self:_updatePositions()

    self:_onScrolling()
end

function TableView:_updatePositions()
    if self.Horizontal then
        self:SetContentSize(Vector2(self._cellsSize.x * self._cellCount,self._cellsSize.y))
        for i=1,self._cellCount do
            table.insert( self._positions,self._cellsSize.x * i)
        end
    elseif self.Vertical then
        local height = self._cellsSize.y * self._cellCount
        self:SetContentSize(Vector2(self._cellsSize.x,height))
        height = math.max(height,self._viewRect.sizeDelta)
        local i = self._cellCount
        while i > 0 do
            table.insert( self._positions,height)
            height = height - self._cellsSize.y
            i = i - 1
        end
    end
end

function TableView:_onScrolling()
    if self._cellCount == 0 then
        return 
    end

    self:_removeCellInvisible()
    self:_addCellVisible()
end

--把看不见到cell放入对象池
function TableView:_removeCellInvisible()
    local beiginIdx = self:_cellBeginIndexFromOffset()
    local endIdx = self:_cellEndIndexFromOffset()

    for k,v in pairs(self._cellUseList) do
        if v.index < beiginIdx or v.index > endIdx then
            self:_removeFromUse(k)
        end
    end
end

function TableView:_removeFromUse(index)
    local cell = self._cellUseList[index]
    table.insert( self._cellPoolList,cell ) 
    self._cellUseList[index] = nil
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
        
    else
    
    end
end

function TableView:_IsUseContainIndex(index)
    for k,v in pairs(self._cellUseList) do
        if v.index == index then
            return true
        end
    end
    return false
end

--从对象池抽取一个元素
function TableView:_getCellFromPool()
    local index = next(self._cellPoolList)
    if index then
        local cell = self._cellPoolList[index]
        self._cellPoolList[index] = nil
        return cell
    end
end

function TableView:_cellBeginIndexFromOffset()
    local index = self.INVALID_INDEX
    if self._cellCount == 0 then
        return index
    end

    if self.Horizontal then
        
    elseif self.Vertical then
        local contentOffset = self:GetContentOffset()
        index = math.floor((math.abs(contentOffset.y) + self._cellsSize.y) / self._cellsSize.y)
    end

    return index
end

function TableView:_cellEndIndexFromOffset()
    local index = self.INVALID_INDEX
    if self._cellCount == 0 then
        return index
    end

    if self.Horizontal then
        
    elseif self.Vertical then
        local contentOffset = self:GetContentOffset()
        local topContentOffsetY = contentOffset.y + self._contentRect.sizeDelta.y 
        local offsetY = topContentOffsetY - self._viewRect.sizeDelta.y
        index = self._cellCount - math.floor(  offsetY / self._cellsSize.y )
    end

    return index
end

function TableView:ClearData()
    self._positions = {}
end