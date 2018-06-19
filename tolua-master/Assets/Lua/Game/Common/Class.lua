--克隆 深度复制对象
local rawset = rawset;
local setmetatable = setmetatable;
local getmetatable = getmetatable;
function deepcopy( object )
    local lookup_table = {}
    local function copyObj( object )
        if type( object ) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs( object ) do
            new_table[copyObj( key )] = copyObj( value )
        end
        return setmetatable( new_table, getmetatable( object ) )
    end
    return copyObj( object )
end



function clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs(object) do
            new_table[_copy(key)] = _copy(value)
        end
        return setmetatable(new_table, getmetatable(object))
    end
    return _copy(object)
end

--Create an class.

function shiftToSuper( tb, k, v, class_type )
    local tbv = rawget(tb,k)
    if tbv then
        local super = class_type.super
        while super do
            if super.selfFun[k] then
                if not tb.Super then
                    tb.Super = {}
                end
                if not tb.Super[super.className] then
                    tb.Super[super.className] = {}
                end
                tb.Super[super.className][k] = super.selfFun[k]
            end
            super = super.super
        end
        rawset(tb,k,v)
        return
    else
        rawset(tb,k,v)
        return
    end
end

local function redfineNewIndex(t,v) error("attempt to newindex a not exist value:"..v)end

local deepcopy = deepcopy;	
function class(super,className,defualArg)
    local class_type = {}
    class_type.className = className
    
    if "string" == type(super) then
        class_type.className = super
    elseif "table" == type(super) then
        class_type.super = super
    end

    class_type.ctor = false;--类初始化函数,每个父类都会执行，顺序：父类 >> 子类

    class_type[class_type.className] = function(ooo,...)
        --如果不默认构造一个函数，默认链关系将会断层
        --执行顺序：可使用Base调节

        if(ooo and ooo.Base ~= nil)then
            ooo:Base(...);
        end
    end;

    class_type.defualArg = defualArg or {}
    class_type.vtbl = {}
    class_type.selfFun = {}

    class_type.New = function(argTableInit)
            local obj = {}
            local copyVtbl = deepcopy(class_type.vtbl);
           -- local mt = { __index =  copyVtbl}
			obj.__index = copyVtbl ;
            setmetatable(obj,obj)

    
            local argTable = {}
            for k,v in pairs( class_type.defualArg ) do
                argTable[k] = v
            end
            if argTableInit then
                if type(argTableInit) == "table" then
                    for k,n in pairs(argTableInit) do
                        argTable[k] = n
                    end
			elseif "string" == type(argTableInit) or "number" == type(argTableInit) or "bool" == type(argTableInit) then
                    argTable = argTableInit
                end
            end
            obj.__className = class_type.className
  
            do
                local create

                create = function(c)

                    if c.super then
                        create(c.super)
                    end
                    if c.ctor  and type(c.ctor) == "function"then
                        c.ctor(obj,argTable)
                    end

                end

                create(class_type);

                class_type(obj,argTable);--执行同名构造函数

            end
            
            --mt.__newindex = redfineNewIndex
            return obj
    end
    
    local vtbl = class_type.vtbl
    --obj add defualt clone functon
    
    function vtbl:clone( )
        return deepcopy(self)
    end
    
    function vtbl:super(className,method,...)
        return self.Super[className][method](self,...)
    end

    function vtbl:Base(...)
        if(self._f_a_t_h_e_r_ ~= nil)then
            self._f_a_t_h_e_r_(self,...);
        end
        self._f_a_t_h_e_r_ = nil;--Base()只能调用一次
    end

    if class_type.super then
        for k,v in pairs(class_type.super.vtbl) do 
            if k == "Super" then
                vtbl[k] = deepcopy(v)
            else
                vtbl[k] = v
            end
        end
    end

    function class_type.getProperty(usc,argTable)
        local create
        create = function(c)
            if c.super then
                create(c.super)
            end
            if c.ctor then
                c.ctor(usc,argTable)
            end
        end
        create(class_type)
    end
    
    function class_type.getFunc(usc)
        for index, value in pairs(vtbl) do
            usc[index] = value
        end
    end
	
    class_type.__newindex =  function(t,k,v) 
		 if "ctor" == k then
                rawset(t,k,v)
             elseif(k == t.className) then
                 rawset(t,k,v)
            else
                local st,ed = string.find(string.lower(k),"static_");
                if(st == 1)then
                    --静态变量
                    --例子 Class.static_var = false;
                    --关键字必须在开头
                      rawset(t,k,v)
                else
                     shiftToSuper( vtbl,k,v,class_type)
                     class_type.selfFun[k] = v
                end

            end
	end
	
	class_type.__call =  function(tb,obj,...)

            if(obj and obj.__className ~= nil)then
                local tClassName = tb.className;
                obj._f_a_t_h_e_r_ = tb.super;
               if(tb[tClassName])  and type(tb[tClassName]) == "function"then
                    tb[tClassName](obj,...);
                end
            end
           
        end
	
	class_type.__mode  = "k"
	
	setmetatable(class_type,class_type);

    return class_type
end



local  function search(k, plist)  
    for i, v in pairs(plist) do  
        local temp_v = v[k]  
        if temp_v then  
            return temp_v  
        end  
    end  
end  
  

