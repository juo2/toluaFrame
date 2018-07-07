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
        self:SetRows(param.cellCount)
    elseif param.horizontal then
        self:SetColumns(param.cellCount) 
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

function TableView:_addCellUse(index)
    local cell = self:_getCellFromPool()
    if cell then
        cell:Awake(index,self._positions[index])
    else
        local prefab = UnityEngine.GameObject.Instantiate(self.inje_Prefab)
        prefab.transform:SetParent(self.transform)
        cell = GridCell.New(
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