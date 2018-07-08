_G.GridView = class(ScrollView,"GridView")

GridView.inje_Prefab = false

GridView.AsynAmount = 1

GridView._positions = {}
GridView._cellUseList = {}
GridView._cellUseIndices = {}
GridView._cellLoadList = {}
GridView._IsHandleMap = {}

GridView._cellsSize = Vector2.zero
GridView._cellCount = nil
GridView._columns = nil
GridView._rows = nil
GridView._oldCellCount = nil
GridView._first = true
GridView.onCellHandle = nil

GridView.INVALID_INDEX = -1

function GridView:Awake()
    self:super("ScrollView","Awake")

    if self.inje_Prefab then
        self.inje_Prefab:SetActive(false)
    end

    self:_beginLoadUpdate()
end

function GridView:SetParam(param)
    self._cellsSize = param.size
    self:SetCellCount(param.cellCount)
    self:SetColumns(param.columns) 
    self:SetRows(param.rows)
end

--设置列数
function GridView:SetColumns(columns)
    if columns then
        self._columns = columns
        self.Vertical = true
        self.Horizontal = false
        if self._cellCount then
            self._rows = math.ceil(self._cellCount / self._columns) 
        end
    end
end

--设置行数
function GridView:SetRows(rows)
    if rows then
        self._rows = rows
        self.Vertical = false
        self.Horizontal = true
        if self._cellCount then
            self._columns = math.ceil(self._cellCount / self._rows) 
        end
    end
end

--设置字容器数量
function GridView:SetCellCount(cellCount)
    self._oldCellCount = self._cellCount
    self._cellCount = cellCount
end

--设置回调函数
function GridView:SetCellHandle(handle)
    self.onCellHandle = handle
end

--刷新子容器
function GridView:UpdateCellHandle(index)
    local cell = self:GetCellByIndex(index)
    if self.onCellHandle and cell then
        self._IsHandleMap[index] = false
        if self:_IsUseContainIndex(index) then
            self:_updateCellHandleInner(index,cell)
        end
    end
end

--根据索引获得自容器
function GridView:GetCellByIndex(index)
    return self._cellUseList[index]
end

--开始渲染
function GridView:ReloadData()
    self:_clearData()    
    self:_updatePositions()
    self:_reloadJump()
    
    self:_onScrolling()
end

function GridView:AddNode()

end 

function GridView:RemoveNode()

end

--更新位置信息
function GridView:_updatePositions()
    if self.Horizontal then
        for col=1,self._columns do
            local width = self._cellsSize.x * (col-1)
            local height = self._cellsSize.y * (self._rows - 1)
            local opRow = self._rows
            while opRow > 0 do
                opRow = opRow - 1
                local row = self._rows - opRow
                if (row -1)*self._columns +col > self._cellCount then
                    break
                end
                local pos = Vector2(width,height)
                table.insert( self._positions,pos)
                height = height - self._cellsSize.y
            end
        end
    elseif self.Vertical then
        local height = self._cellsSize.y * (self._rows - 1)
        local opRow = self._rows
        while opRow > 0 do
            opRow = opRow - 1
            for col=1,self._columns do
                local row = self._rows - opRow
                if (row -1)*self._columns +col > self._cellCount then
                    break
                end
                local width = self._cellsSize.x * (col-1)
                local pos = Vector2(width,height)
                table.insert( self._positions,pos)
            end
            height = height - self._cellsSize.y
        end 
    end
    self:SetContentSize(Vector2(self._columns * self._cellsSize.x,self._rows * self._cellsSize.y))
end

--找到开始的index和结束的index并刷新面板
function GridView:_onScrolling()
    self:super("ScrollView","_onScrolling");

    if self._cellCount == 0 then
        return 
    end

    self:_removeCellInvisible()
    self:_addCellVisible()
    self:_updateCellPosition()
end

--把看不见到cell放入对象池
function GridView:_removeCellInvisible()
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

function GridView:_removeFromUse(index)
    local cell = self._cellUseList[index]
    cell:Reset()
    self._cellUseIndices[index] = nil
end

--把看得见到cell从对象池取出，或创建
function GridView:_addCellVisible()
    local beiginIdx = self:_cellBeginIndexFromOffset()
    local endIdx = self:_cellEndIndexFromOffset()

    for i=beiginIdx,endIdx do
        if not self:_IsUseContainIndex(i) then
            self:_addCellUse(i)
        end
    end
end

function GridView:_addCellUse(index)
    local cell = self:_getCellFromPool(index)
    if cell then
        cell:Awake(index,self._positions[index])
        self:_updateCellHandleInner(index,cell)
    else
        table.insert(self._cellLoadList,index)
    end
    self._cellUseIndices[index] = true
end

function GridView:_addCellUseAsyn(index)
    local prefab = UnityEngine.GameObject.Instantiate(self.inje_Prefab)
    prefab.transform:SetParent(self.transform)
    local cell = GridCell.New(
        {
            index = index,
            position = self._positions[index],
            size = self._cellsSize,
            object = prefab,
        }
    )
    cell:UpdatePosition(self:GetContentOffset())
    self:_updateCellHandleInner(index,cell)
    self._cellUseList[index] = cell
end

function GridView:_updateCellHandleInner(index,cell)
    if self.onCellHandle and cell and not self:_isHandle(index) then
        self.onCellHandle(index,cell.object)
        self._IsHandleMap[index] = true
    end
end

function GridView:_updateCellPosition()
    for k,v in pairs(self._cellUseList) do
        if self:_IsUseContainIndex(k) then
            v:Awake(k,self._positions[k])
            v:UpdatePosition(self:GetContentOffset())
        end
    end
end

function GridView:_IsUseContainIndex(index)
    return self._cellUseIndices[index]
end

--从对象池抽取一个元素
function GridView:_getCellFromPool(index)
    local cell = self._cellUseList[index]
    return cell
end

--根据拖动的位置计算开始的index
function GridView:_cellBeginIndexFromOffset()
    local index = self.INVALID_INDEX
    if self._cellCount == 0 then
        return index
    end

    local contentOffset = self:GetContentOffset()
    if self.Horizontal then
        local offset = math.max(0,-contentOffset.x / self._cellsSize.x)
        index = math.floor(offset) * self._rows +  1 
    elseif self.Vertical then
        local offset = math.max(0,contentOffset.y + self._innHeight - self._viewRect.sizeDelta.y) 
        index = math.floor( offset / self._cellsSize.y ) * self._columns + 1
    end

    return index
end

--根据拖动的位置计算结束的index
function GridView:_cellEndIndexFromOffset()
    local index = self.INVALID_INDEX
    if self._cellCount == 0 then
        return index
    end

    local contentOffset = self:GetContentOffset()
    if self.Horizontal then
        local offset = math.max(0,contentOffset.x + self._innWidth - self._viewRect.sizeDelta.x)  
        index = self._cellCount - math.floor( offset / self._cellsSize.x )* self._rows
    elseif self.Vertical then
        local offset = math.max( 0,-contentOffset.y / self._cellsSize.y)  
        if (self._rows - math.floor(offset)) * self._columns < 0 then
            index = self._cellCount
        else
            index = (self._rows - math.floor(offset)) * self._columns - (self._rows * self._columns - self._cellCount)
        end
    end

    return index
end

function GridView:_reloadJump()
    if self._first then
        self:SetTop()
    else
        self:_setOriginPosition()
    end
    self._first = false
end

function GridView:_setOriginPosition()
    if self._oldCellCount  then
        if self.Horizontal then
            local offsetCount = (self._cellCount - self._oldCellCount) / self._rows
            local offset = math.max(self:GetContentOffset().x,self._viewRect.sizeDelta.x - self._innWidth)
            self:SetContentOffset(Vector2(offset,0))
        end
        if self.Vertical then
            local offsetCount = (self._cellCount - self._oldCellCount) / self._columns
            local offset = math.min( 0,self:GetContentOffset().y - offsetCount * self._cellsSize.y )
            self:SetContentOffset(Vector2(0,offset))
        end
    end
end

function GridView:_isHandle(index)
    return self._IsHandleMap[index]
end

function GridView:_beginLoadUpdate()
    if not self._loadHandle then
        self._loadHandle = UpdateBeat:CreateListener(self.LoadUpdate, self)
    end
    UpdateBeat:AddListener(self._loadHandle)	
end

function GridView:_endLoadUpdate()
    if self._loadHandle then
        UpdateBeat:RemoveListener(self._loadHandle)	
    end
end

function GridView:LoadUpdate()
    for i=1,self.AsynAmount do
        if #self._cellLoadList > 0 then
            local index = table.remove( self._cellLoadList,1)
            if self:_IsUseContainIndex(index) then
                self:_addCellUseAsyn(index)
            end
        end
    end
end

function GridView:_clearData()
    self._positions = {}
    self._IsHandleMap = {}
    self._cellLoadList = {}
end

function GridView:OnDestroy()
    self:super("ScrollView","OnDestroy")
    self:_clearData()
    self:_endLoadUpdate()

    self._cellUseList = {}
    self._cellsSize = Vector2.zero
    self._cellCount = nil
    self._columns = nil
    self._rows = nil
    self._oldCellCount = nil
    self._first = true
    self.onCellHandle = nil
end