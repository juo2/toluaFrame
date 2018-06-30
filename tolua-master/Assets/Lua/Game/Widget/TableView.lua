_G.TableView = class(ScrollView,"TableView")

TableView._positions = {}
TableView._cellsSize = Vector2.zero
TableView._cellCount = 0

function TableView:SetCellCount(cellCount)
    self._cellCount = cellCount
end

function TableView:SetData()

end

function TableView:ReloadData()
    
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

    
end