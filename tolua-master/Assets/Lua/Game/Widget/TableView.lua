_G.TableView = class(GridView,"TableView")

TableView._cellPoolList = {}

function TableView:Awake()
    self:super("GridView","Awake")
end

function TableView:SetParam(param)
    self._cellsSize = param.size
    self:SetCellCount(param.cellCount)

    if param.vertical then
        self:SetColumns(1) 
    elseif param.horizontal then
        self:SetRows(1)
    end
    self.Vertical = param.vertical or false
    self.Horizontal = param.horizontal or false
end

function TableView:AddNode()

end 

function TableView:RemoveNode()

end

function TableView:_removeFromUse(index)
    local cell = self._cellUseList[index]
    cell:Reset()
    table.insert( self._cellPoolList,cell) 
    self._cellUseList[index] = nil
    self._cellUseIndices[index] = nil
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

function TableView:_updateCellHandleInner(index,cell)
    if self.onCellHandle and cell and not self:_isHandle(index) then
        self.onCellHandle(index,cell.object)
    end
end

function TableView:_clearData()
    self._positions = {}
    self._cellLoadList = {}
end

function TableView:OnDestroy()
    self:super("GridView","OnDestroy")
    self._cellPoolList = {}
end