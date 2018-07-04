local breakInfoFun = require("Game/Debug/LuaDebugjit")(ip and ip or "localhost",7003)
    --添加断点监听函数
local time = Timer.New(breakInfoFun,0.5,-1,1)

--主入口函数。从这里开始lua逻辑
require("Game/Common/Define")

function Main()					
	ModuleManager:OpenModule(ModuleType.WidgetTest)
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()

end