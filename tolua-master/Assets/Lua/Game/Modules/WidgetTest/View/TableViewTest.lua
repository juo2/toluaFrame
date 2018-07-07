TableViewTest = class(Panel,"TableViewTest")

TableViewTest.inje_TableView = 1

function TableViewTest:Open()
    -- local tableView = self.inje_TableView:GetInstance()
    -- tableView:SetCellHandle(function (idx,item)
    --     local txt = Util.Find(item,"Text",typeof(UnityEngine.UI.Text))
    --     txt.text = idx
    -- end)
    -- tableView:SetCellCountAndSize(16,Vector2(200,200))
    -- tableView:ReloadData()

    local tableView = self.inje_TableView:GetInstance()
    tableView:SetCellHandle(function (idx,item)
        local txt = Util.Find(item,"Text",typeof(UnityEngine.UI.Text))
        txt.text = idx
    end)
    tableView:SetParam({
        size = Vector2(100,100),
        cellCount = 10,
        vertical = true,
    })
    tableView:ReloadData()
end

function TableViewTest:Close()

end