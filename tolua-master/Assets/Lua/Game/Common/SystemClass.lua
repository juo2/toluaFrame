--通过值检索是否存在
--list 即table表
--需要检索的value 可为table
function table.include(list , value)
    if(not list)then return false end;
    for k,v in pairs(list) do
        if v==value then 
            return true
        end
    end
    return false
end
