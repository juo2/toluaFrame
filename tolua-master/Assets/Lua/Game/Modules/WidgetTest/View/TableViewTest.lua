TableViewTest = class(Panel,"TableViewTest")

TableViewTest.inje_TableView = 1

function TableViewTest:Open()
    local tableView = self.inje_TableView:GetInstance()
    tableView:ReloadData()
end

function TableViewTest:Close()

end