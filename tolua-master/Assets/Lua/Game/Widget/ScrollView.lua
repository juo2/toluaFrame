_G.ScrollView = class(UIBase,"ScrollView")

ScrollView.inje_content = false

ScrollView._dragging = false
ScrollView._lastMovePoint = Vector2.zero
ScrollView._scrollDistance = Vector2.zero
ScrollView._viewRect = false
ScrollView._contentRect = false
ScrollView._debug = false
ScrollView._maxOffset = Vector2.zero
ScrollView._minOffset = Vector2.zero
ScrollView._tweenMaker = false
ScrollView._handle = false
ScrollView._isInertanceFinish = false
ScrollView._innWidth = 0
ScrollView._innHeight = 0
ScrollView._contentX = 0
ScrollView._contentY = 0

--可设置的参数
ScrollView.Dragable = true
ScrollView.Horizontal = true
ScrollView.Vertical = false
ScrollView.Inertia = true
ScrollView.ResistanceSpeed = 0.65
ScrollView.LimitValue = 1
ScrollView.InertanceSpeed = 0.96
ScrollView.dragSpeed = 0.5
ScrollView.dragValidateSpeed = 0.8

--委托
ScrollView.onScrollingHandler = false

function ScrollView:Awake()
    self._viewRect = self.gameObject:GetComponent(typeof(UnityEngine.RectTransform))
    if self.inje_content then
        self._contentRect = self.inje_content:GetComponent(typeof(UnityEngine.RectTransform))
        self:SetContentSize()
    end
    
    self._tweenMaker = UnityEngine.GameObject.New("TweenMaker")
    self._tweenMaker.transform:SetParent(self.transform)
    self._tweenMaker.transform.localPosition = Vector3.zero
end

function ScrollView:Start()
    
end

function ScrollView:onClick()

end

--设置content的size
function ScrollView:SetContentSize(size,notUpdateView)
    size = size or self._contentRect.sizeDelta
   
    local viewSize = self._viewRect.sizeDelta
    self._innWidth = math.max(viewSize.x,size.x)
    self._innHeight = math.max(viewSize.y,size.y)

    if self.inje_content then
        self._contentRect.sizeDelta = Vector2(self._innWidth,self._innHeight)
    end 

    if self:GetContentOffset() == Vector2.zero then
        self:SetContentOffset(Vector2(0,0),notUpdateView)
    end

    self:_updateLimitOffset()
end

--获取content的位置
function ScrollView:GetContentOffset()
    return Vector2(self._contentX,self._contentY)
end

function ScrollView:_updateLimitOffset()
    local size = Vector2(self._viewRect.sizeDelta.x,self._viewRect.sizeDelta.y)
    local innSize = Vector2(self._innWidth,self._innHeight)

    self._maxOffset.x = 0
    self._maxOffset.y = 0

    self._minOffset.x = size.x - innSize.x
    self._minOffset.y = size.y - innSize.y

    if not self.Horizontal then
        self._minOffset.x = 0
    end

    if not self.Vertical then
        self._minOffset.y = 0
    end
end

--边界检测
function ScrollView:_validateOffset(point)
    local x = point.x
    local y = point.y
    x = math.max(x, self._minOffset.x)
    x = math.min(x, self._maxOffset.x)
    y = math.max(y, self._minOffset.y)
    y = math.min(y, self._maxOffset.y)

    if point.x ~= x or point.y ~= y then
        point.x = x;
        point.y = y;
        return true;
    end
    
    point.x = x;
    point.y = y;
    return false;
end

--开始拖拽
function ScrollView:OnBeginDrag(eventData)
    if self.Dragable then
        self._dragging = true
        --记录一开始的点
        local point = self.transform:InverseTransformPoint(eventData.position)
        self._lastMovePoint = point;
        self:_onScrolling()
    end
end

function ScrollView:OnDrag(eventData)
    if self.Dragable then
        local point = self.transform:InverseTransformPoint(eventData.position)
        self._scrollDistance = point - self._lastMovePoint
        self._lastMovePoint = point

        if not self.Horizontal then
            self._scrollDistance.x = 0
        end

        if not self.Vertical then
            self._scrollDistance.y = 0
        end

        self._scrollDistance = self._scrollDistance * self.dragSpeed
        local vec = self:GetContentOffset() + self._scrollDistance
        if self:_validateOffset(vec) then
            self._scrollDistance = self._scrollDistance * self.dragValidateSpeed
        end
        self:SetContentOffset(self:GetContentOffset() + self._scrollDistance)
    end
end

--结束拖拽
function ScrollView:OnEndDrag(eventData)
    if self.Dragable then
        self._dragging = false
        local offset = self:GetContentOffset()
        if self:_validateOffset(offset) then
            self._scrollDistance = Vector2.zero
            self:SetContentOffsetInDuration(offset)
        end
        self:BeginInertia()
    end
end

--开始惯性update
function ScrollView:BeginInertia()
    if self.Inertia then
        if not self._handle then
            self._handle = UpdateBeat:CreateListener(self.Update, self)
        end
        UpdateBeat:AddListener(self._handle)	
    end
end

--关闭惯性update
function ScrollView:EndInertia()
    if self.Inertia then
        if self._handle then
            UpdateBeat:RemoveListener(self._handle)	
        end
    end
end

--设置content位置
function ScrollView:SetContentOffset(offset,notUpdateView)
    if self.inje_content then
        self.inje_content.transform.localPosition = Vector3(offset.x,offset.y,0)
    end
    self._contentX = offset.x
    self._contentY = offset.y
    if not notUpdateView then
        self:_onScrolling()
    end
end

--设置content位置缓动
function ScrollView:SetContentOffsetInDuration(offset,duration,notUpdateView)
    duration = duration or 0.2
    local containerLocalPosition = self:GetContentOffset()
    LeanTween.cancel(self._tweenMaker)
    local tween = LeanTween.value(self._tweenMaker,containerLocalPosition,offset,duration)
    tween:setEase(LeanTweenType.easeInQuad)
    tween:setOnUpdate(System.Action_UnityEngine_Vector2(function (val)
        self:SetContentOffset(val,notUpdateView)
    end))
end

--惯性update
function ScrollView:Update()
    local offset = self:GetContentOffset() + self._scrollDistance
    if  self:_validateOffset(offset) then
        if math.abs(self._scrollDistance.x) >= self.LimitValue or math.abs(self._scrollDistance.y) >= self.LimitValue then
            self._scrollDistance = self._scrollDistance * self.ResistanceSpeed;
            self:SetContentOffset (self:GetContentOffset() + self._scrollDistance);
            self._isInertanceFinish = false;
        else 
            if not self._isInertanceFinish then
                self._isInertanceFinish = true;
                self._scrollDistance = Vector2.zero;
                self:EndInertia()
                self:SetContentOffsetInDuration(offset)
            end
        end
    else
        if math.abs(self._scrollDistance.x) >= self.LimitValue or math.abs(self._scrollDistance.y) >= self.LimitValue then
            self._scrollDistance =  self._scrollDistance * self.InertanceSpeed
            self:SetContentOffset (self:GetContentOffset() + self._scrollDistance);
            self._isInertanceFinish = false;
        else 
            if not self._isInertanceFinish then
                self._isInertanceFinish = true;
                self._scrollDistance = Vector2.zero;
                self:EndInertia()
            end
        end
    end
end

function ScrollView:_onScrolling()
    if self.onScrollingHandler then
        self.onScrollingHandler()
    end
end

function ScrollView:SetTop(duration,notUpdateView)
    if not self.Vertical then
        return
    end
    local offset = Vector2(self._contentX,self._viewRect.sizeDelta.y - self._innHeight)
    if duration then
        self:SetContentOffsetInDuration(offset,duration,notUpdateView)
    else
        self:SetContentOffset(offset,notUpdateView)
    end
end

function ScrollView:SetDown(duration,notUpdateView)
    if not self.Vertical then
        return
    end
    local offset = Vector2(self._contentX,0)
    if duration then
        self:SetContentOffsetInDuration(offset,duration,notUpdateView)
    else
        self:SetContentOffset(offset,notUpdateView)
    end
end

function ScrollView:SetLeft(duration,notUpdateView)
    if not self.Horizontal then
        return 
    end
    local offset = Vector2(0,self._contentY)
    if duration then
        self:SetContentOffsetInDuration(offset,duration,notUpdateView)
    else
        self:SetContentOffset(offset,notUpdateView)
    end
end

function ScrollView:SetRight(duration,notUpdateView)
    if not self.Horizontal then
        return 
    end
    local offset = Vector2(self._viewRect.sizeDelta.x - self._innWidth,self._contentY)
    if duration then
        self:SetContentOffsetInDuration(offset,duration,notUpdateView)
    else
        self:SetContentOffset(offset,notUpdateView)
    end
end

function ScrollView:SetData(param)

end

function ScrollView:OnDestroy()

end