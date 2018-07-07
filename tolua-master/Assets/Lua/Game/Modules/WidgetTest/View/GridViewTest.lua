GridViewTest = class(Panel,"GridViewTest")

GridViewTest.inje_GridView = 1

function GridViewTest:Open()

    local GridView = self.inje_GridView:GetInstance()
    GridView:SetCellHandle(function (idx,item)
        local txt = Util.Find(item,"Text",typeof(UnityEngine.UI.Text))
        txt.text = idx
    end)
    GridView:SetParam({
        size = Vector2(100,100),
        cellCount = 30,
        rows = 3,
    })
    GridView:ReloadData()
end

function GridViewTest:Close()

end