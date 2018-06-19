using UnityEngine;

using LuaInterface;

// The lua behavior base class.
public class LLuaBehaviourInterface
{
    // The callback method name.
    private static readonly string AWAKE = "Awake";
    private static readonly string LATE_UPDATE = "LateUpdate";
    private static readonly string FIXED_UPDATE = "FixedUpdate";
    private static readonly string ON_ANIMATOR_IK = "OnAnimatorIK";
    private static readonly string ON_ANIMATOR_MOVE = "OnAnimatorMove";
    private static readonly string ON_APPLICATION_FOCUS = "OnApplicationFocus";
    private static readonly string ON_APPLICATION_PAUSE = "OnApplicationPause";
    private static readonly string ON_APPLICATION_QUIT = "OnApplicationQuit";
    //private static readonly string ON_AUDIO_FILTER_READ = "OnAudioFilterRead";	// Skip.
    private static readonly string ON_BECAME_INVISIBLE = "OnBecameInvisible";
    private static readonly string ON_BECAME_VISIBLE = "OnBecameVisible";
    private static readonly string ON_COLLISION_ENTER = "OnCollisionEnter";
    private static readonly string ON_COLLISION_ENTER_2D = "OnCollisionEnter2D";
    private static readonly string ON_COLLISION_EXIT = "OnCollisionExit";
    private static readonly string ON_COLLISION_EXIT_2D = "OnCollisionExit2D";
    private static readonly string ON_COLLISION_STAY = "OnCollisionStay";
    private static readonly string ON_COLLISION_STAY_2D = "OnCollisionStay2D";
    //private static readonly string ON_CONNECTED_SERVER = "OnConnectedToServer";	// Skip.
    private static readonly string ON_CONTROLLER_COLLIDER_HIT = "OnControllerColliderHit";
    private static readonly string ON_DESTROY = "OnDestroy";
    private static readonly string ON_DISABLE = "OnDisable";
    //private static readonly string ON_DISCONNECTED_FROM_SERVER = "OnDisconnectedFromServer";	// Skip.
    //private static readonly string ON_DRAW_GIZMOS = "OnDrawGizmos";	// Skip.
    //private static readonly string ON_DRAW_GIZMOS_SELECTED = "OnDrawGizmosSelected";	// Skip.
    private static readonly string ON_ENABLE = "OnEnable";
    //private static readonly string ON_FAILED_TO_CONNECT = "OnFailedToConnect";	// Skip.
    //private static readonly string ON_FAILED_TO_CONNECT_MASTER_SERVER = "OnFailedToConnectToMasterServer";	// Skip
    //private static readonly string ON_GUI = "OnGUI";	// Skip.
    private static readonly string ON_JOINT_BREAK = "OnJointBreak";
    private static readonly string ON_LEVEL_WAS_LOADED = "OnLevelWasLoaded";
    //private static readonly string ON_MASTER_SERVER_EVENT = "OnMasterServerEvent";	// Skip.
    private static readonly string ON_MOUSE_DOWN = "OnMouseDown";
    private static readonly string ON_MOUSE_DRAG = "OnMouseDrag";
    private static readonly string ON_MOUSE_ENTER = "OnMouseEnter";
    private static readonly string ON_MOUSE_EXIT = "OnMouseExit";
    private static readonly string ON_MOUSE_OVER = "OnMouseOver";
    private static readonly string ON_MOUSE_UP = "OnMouseUp";
    private static readonly string ON_MOUSE_UP_AS_BUTTON = "OnMouseUpAsButton";
    //private static readonly string ON_NETWORK_INSTANTIATE = "OnNetworkInstantiate";	// Skip.
    private static readonly string ON_PARTICLE_COLLISION = "OnParticleCollision";
    //private static readonly string ON_PLAYER_CONNECTED = "OnPlayerConnected";	// Skip.
    //private static readonly string ON_PLAYER_DISCONNECTED = "OnPlayerDisconnected";	// Skip.
    private static readonly string ON_POST_RENDER = "OnPostRender";
    private static readonly string ON_PRE_CULL = "OnPreCull";
    private static readonly string ON_PRE_RENDER = "OnPreRender";
    private static readonly string ON_RENDER_IMAGE = "OnRenderImage";
    private static readonly string ON_RENDER_OBJECT = "OnRenderObject";
    //private static readonly string ON_SERIALIZE_NETWORK_VIEW = "OnSerializeNetworkView";	// Skip.
    //private static readonly string ON_SERVER_INITIALIZED = "OnServerInitialized";	// Skip.
    private static readonly string ON_TRANSFORM_CHILDREN_CHANGED = "OnTransformChildrenChanged";
    private static readonly string ON_TRANSFORM_PARENT_CHANGED = "OnTransformParentChanged";
    private static readonly string ON_TRIGGER_ENTER = "OnTriggerEnter";
    private static readonly string ON_TRIGGER_ENTER_2D = "OnTriggerEnter2D";
    private static readonly string ON_TRIGGER_EXIT = "OnTriggerExit";
    private static readonly string ON_TRIGGER_EXIT_2D = "OnTriggerExit2D";
    private static readonly string ON_TRIGGER_STAY = "OnTriggerStay";
    private static readonly string ON_TRIGGER_STAY_2D = "OnTriggerStay2D";
    private static readonly string ON_VALIDATE = "OnValidate";
    private static readonly string ON_WILL_RENDER_OBJECT = "OnWillRenderObject";
    //private static readonly string RESET = "Reset";	// Skip.
    private static readonly string START = "Start";
    private static readonly string UPDATE = "Update";
    //custom
    private static readonly string ON_EVENT_ANIM = "OnEventAnim";
    private static readonly string ON_EVENT_ANIM_INT = "OnEventAnimInt";
    private static readonly string ON_EVENT_ANIM_FLOAT = "OnEventAnimFloat";
    private static readonly string ON_EVENT_ANIM_STRING = "OnEventAnimString";
    private static readonly string ON_EVENT_ANIM_OBJECT = "OnEventAnimObject";

    // The function for monobehavior callback event.
    private LuaFunction m_cAwakeFunc = null;
    private LuaFunction m_cLateUpdateFunc = null;
    private LuaFunction m_cFixedUpdateFunc = null;
    private LuaFunction m_cOnAnimatorIKFunc = null;
    private LuaFunction m_cOnAnimatorMoveFunc = null;
    private LuaFunction m_cOnApplicationFocusFunc = null;
    private LuaFunction m_cOnApplicationPauseFunc = null;
    private LuaFunction m_cOnApplicationQuitFunc = null;
    private LuaFunction m_cOnBecameInvisibleFunc = null;
    private LuaFunction m_cOnBecameVisibleFunc = null;
    private LuaFunction m_cOnCollisionEnterFunc = null;
    private LuaFunction m_cOnCollisionEnter2DFunc = null;
    private LuaFunction m_cOnCollisionExitFunc = null;
    private LuaFunction m_cOnCollisionExit2DFunc = null;
    private LuaFunction m_cOnCollisionStayFunc = null;
    private LuaFunction m_cOnCollisionStay2DFunc = null;
    private LuaFunction m_cOnControllerColliderHitFunc = null;
    private LuaFunction m_cOnDestroy = null;
    private LuaFunction m_cOnDisableFunc = null;
    private LuaFunction m_cOnEnableFunc = null;
    private LuaFunction m_cOnJointBreakFunc = null;
    private LuaFunction m_cOnLevelWasLoadedFunc = null;
    private LuaFunction m_cOnMouseDownFunc = null;
    private LuaFunction m_cOnMouseDragFunc = null;
    private LuaFunction m_cOnMouseEnterFunc = null;
    private LuaFunction m_cOnMouseExitFunc = null;
    private LuaFunction m_cOnMouseOverFunc = null;
    private LuaFunction m_cOnMouseUpFunc = null;
    private LuaFunction m_cOnMouseUpAsButtonFunc = null;
    private LuaFunction m_cOnParticleCollisionFunc = null;
    private LuaFunction m_cOnPostRenderFunc = null;
    private LuaFunction m_cOnPreCullFunc = null;
    private LuaFunction m_cOnPreRenderFunc = null;
    private LuaFunction m_cOnRenderImageFunc = null;
    private LuaFunction m_cOnRenderObjectFunc = null;
    private LuaFunction m_cOnTransformChildrenChangedFunc = null;
    private LuaFunction m_cOnTransformParentChangedFunc = null;
    private LuaFunction m_cOnTriggerEnterFunc = null;
    private LuaFunction m_cOnTriggerEnter2DFunc = null;
    private LuaFunction m_cOnTriggerExitFunc = null;
    private LuaFunction m_cOnTriggerExit2DFunc = null;
    private LuaFunction m_cOnTriggerStayFunc = null;
    private LuaFunction m_cOnTriggerStay2DFunc = null;
    private LuaFunction m_cOnValidateFunc = null;
    private LuaFunction m_cOnWillRenderObjectFunc = null;
    private LuaFunction m_cStartFunc = null;
    private LuaFunction m_cUpdateFunc = null;
    //custom
    private LuaFunction m_cOnEventAnim = null;
    private LuaFunction m_cOnEventAnimInt = null;
    private LuaFunction m_cOnEventAnimFloat = null;
    private LuaFunction m_cOnEventAnimString = null;
    private LuaFunction m_cOnEventAnimObject = null;

    // The lua table operator of this behavior.
    private LLuaTable m_cLuaTableOpt = null;

    /**
     * Constructor.
     * 
     * @param void.
     * @return void.
     */
    public LLuaBehaviourInterface()
    {
    }

    /**
     * Destructor.
     * 
     * @param void.
     * @return void.
     */
    ~LLuaBehaviourInterface()
    {
    }

    /**
     * Awake method.
     * 
     * @param void.
     * @return void.
     */
    public void Awake()
    {
        CallMethod(ref m_cAwakeFunc, AWAKE, m_cLuaTableOpt.GetChunk());
    }
    /**
     * On destroy method.
     * 
     * @param void.
     * @return void.
     */
    public void OnDestroy()
    {
        CallMethod(ref m_cOnDestroy, ON_DESTROY, m_cLuaTableOpt.GetChunk());
    }

    public void Start()
    {
        CallMethod(ref m_cStartFunc, START, m_cLuaTableOpt.GetChunk());
    }

    public bool CreateClassInstance(string strClassName)
    {
        if (string.IsNullOrEmpty(strClassName))
        {
            return false;
        }

        // Try to get global lua class.
        try
        {
            // Get class first.
            LuaTable cClsTable = LuaManager.Instance.GetLuaTable(strClassName);
            if (null == cClsTable)
            {
                return false;
            }

            // Get "new" method of the lua class to create instance.
            LuaFunction cNew = (LuaFunction)cClsTable["New"];
            if (null == cNew)
            {
                return false;
            }

            // We choose no default init parameter for constructor.
            cNew.BeginPCall();
            cNew.PCall();
            object cInsChunk = (object)cNew.CheckVariant();
            cNew.EndPCall();

            if (null == cInsChunk)
            {
                return false;
            }

            // If we create instance ok, use it as table.
            m_cLuaTableOpt = new LLuaTable((LuaTable)cInsChunk);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        return false;
    }

    /**
     * Get the lua code chunk holder (lua table).
     * 
     * @param void.
     * @return YwLuaTable - The chunk table holder.
     */
    public LLuaTable GetChunkHolder()
    {
        return m_cLuaTableOpt;
    }

    /**
     * Get the lua code chunk (table).
     * 
     * @param void.
     * @return LuaTable - The chunk table.
     */
    public LuaTable GetChunk()
    {
        if (null == m_cLuaTableOpt)
        {
            return null;
        }

        return m_cLuaTableOpt.GetChunk();
    }

    /**
     * Set lua data to a lua table, used to communiate with other lua files.
     * 
     * @param string strName - The key name of the table.
     * @param object cValue - The value associated to the key.
     * @return void.
     */
    public void SetData(string strName, object cValue)
    {
        if (null == m_cLuaTableOpt)
        {
            return;
        }

        m_cLuaTableOpt.SetData(strName, cValue);
    }

    /**
     * Call a lua method.
     * 
     * @param ref LuaFunction cFunc - The out function. If it is not null, will call it instead of look up from table by strFunc.
     * @param string strFunc - The function name.
     * @param object cParam - The param.
     * @return object - The number of result.
     */
    public object CallMethod(ref LuaFunction cFunc, string strFunc, object cParam)
    {
        if (null == m_cLuaTableOpt)
        {
            return null;
        }

        return m_cLuaTableOpt.CallMethod(ref cFunc, strFunc, cParam);
    }
}

