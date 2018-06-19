--普通事件分发器(一般是视图层事件)
Dispatcher = {}
local _dispatcher = EventDispatcher.New()

function Dispatcher.addEventListener(name, listener, listenerCaller, priority)
	_dispatcher:addEventListener(name, listener, listenerCaller, priority)
end

function Dispatcher.removeEventListener(name, listener)
	_dispatcher:removeEventListener(name, listener)
end

function Dispatcher.dispatchEvent(name, ...)
	_dispatcher:dispatchEvent(name, ...)
end

function Dispatcher.hasEventListener(name)
	_dispatcher:hasEventListener(name)
end

function Dispatcher.clear()
	_dispatcher:clear()
end