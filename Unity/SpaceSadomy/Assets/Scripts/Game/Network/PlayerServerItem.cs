using UnityEngine;
using System.Collections;
using Game.Space;
using System.Collections.Generic;

namespace Game.Network
{

}
/*
public class PlayerServerItem 
{

    private NetworkGame _game;
    private float _innerRadius;
    private float _outerRadius;
    private ISpaceObject _target;
    private bool _hasTarget;

    private PlayerItemComponent _playerItemComponent;
    private bool _visibilityToTherPlayers;

    private float _REQUEST_SELF_PROPERTIES_INTERVAL = 1.0f;
    private float _SEND_TRANSFORM_INTERVAL = 0.05f;
    private float _UPDATE_INTEREST_AREA_INTERVAL = 5.0f;

    private float _requestSelfPropertiesLastTime;
    private float _sendTransformLastTime;
    private float _updateInterestAreaLastTime;

    private Vector3 _lastPosition;
    private Vector3 _lastRotation;

    public PlayerServerItem(NetworkGame game, string id, ItemType type, float[] position, float[] rotation, float innerRadius, float outerRadius)
        : base(id, type, position, rotation)
    {
        _game = game;
        _innerRadius = innerRadius;
        _outerRadius = outerRadius;
        _visibilityToTherPlayers = false;
        _requestSelfPropertiesLastTime = Time.time;
        _sendTransformLastTime = Time.time;
        _updateInterestAreaLastTime = Time.time;
        Events.ItemRemoved += OnItemRemoved;
        Events.ServerItemPropertiesReceived += Events_ServerItemPropertiesReceived;
    }






    public void RequestControlState(PlayerState state)
    {
        object[] actionParameters = new object[] { (byte)state };
        object[] opParams = new object[] { Id, "ChangeControlState", actionParameters };
        _game.Operations.Add(OperationCode.CompleteAction, opParams);
    }

    public void RequestActionState(PlayerState state)
    {
        object[] actionParameters = new object[] { (byte)state };
        object[] opParams = new object[] { Id, "ChangeActionState", actionParameters };
        _game.Operations.Add(OperationCode.CompleteAction, opParams);
    }

    /// <summary>
    /// Force change state client without request on server
    /// </summary>
    /// <param name="state"></param>
    public void ForceActionState(PlayerState state) {
        RequestActionState(state);
        _game.GamePlayer.Ship.ForceActionState(state);
    }

    public void ForceControlState(PlayerState state) {
        RequestControlState(state);
        _game.GamePlayer.Ship.ForceControlState(state);
    }

    public void SetPlayerTarget(ISpaceObject target)
    {
        if (target == null)
        {
            DebugConsole.PrintError(this, "Target must be not null");
            return;
        }

        _target = target;
        RequestTarget(target);
        if (SpaceObjectContextInfo.Get)
            SpaceObjectContextInfo.Get.SetObject(target);
    }

    private void RequestTarget(ISpaceObject targetObj)
    {
        if (targetObj != null)
        {
            object[] actionParameters = new object[] { true, targetObj.GetId(), (byte)targetObj.GetObjectType(), targetObj.IsLocal() };
            object[] opParams = new object[] { Id, "SetTarget", actionParameters };
            _game.Operations.Add(OperationCode.CompleteAction, opParams);
        }
        else
        {
            object[] actionParameters = new object[] { false, string.Empty, (byte)ItemType.Player, false };
            object[] opParams = new object[] { Id, "SetTarget", actionParameters };
            _game.Operations.Add(OperationCode.CompleteAction, opParams);
        }
    }

    private void ResetTarget()
    {
        RequestTarget(null);
        if (SpaceObjectContextInfo.Get)
            SpaceObjectContextInfo.Get.RemoveObject();
    }

    public bool VisibilityToOtherPlayers
    {
        get { return _visibilityToTherPlayers; }
    }

    public void SetVisibilityToOtherPlayers(bool vis)
    {
        //Debug.Log("set visibility: " + vis);
        _visibilityToTherPlayers = vis;
    }

    public void SetViewRadius(float inner, float outer)
    {
        _innerRadius = inner;
        _outerRadius = outer;
        if (View)
        {
            ViewSphere[] radiusSpheres = View.GetComponentsInChildren<ViewSphere>();
            foreach (ViewSphere vs in radiusSpheres)
            {
                if (vs.radiusType == ViewSphere.SphereRadiusType.Inner)
                    vs.transform.localScale = Vector3.one * _innerRadius;
                if (vs.radiusType == ViewSphere.SphereRadiusType.Outer)
                    vs.transform.localScale = Vector3.one * _outerRadius;
            }
        }
        _game.Operations.Add(OperationCode.SetViewRadius, new object[] { Id, _innerRadius, _outerRadius });
    }




    #region Properties
    public NetworkGame Game
    {
        get
        {
            return _game;
        }
    }

    public bool HasTarget
    {
        get { return _hasTarget; }
    }

    public float InnerRadius {
        get {
            return _innerRadius;
        }
    }

    public float OuterRadius {
        get {
            return _outerRadius;
        }
    }
    public ISpaceObject Target
    {
        get
        {
            return _target;
        }
    }
    #endregion

    #region Overrides
    public override void Update()
    {
        if (_game.CurrentWorld != null)
        {
            if (Time.time > _requestSelfPropertiesLastTime + _REQUEST_SELF_PROPERTIES_INTERVAL)
            {
                _requestSelfPropertiesLastTime = Time.time;
                _game.Operations.Add(OperationCode.GetProperties, new object[]{ Id, 
                            new string[]{ GroupProps.DEFAULT_STATE, GroupProps.SHIP_BASE_STATE, GroupProps.SHIP_WEAPON_STATE, 
                            GroupProps.MECHANICAL_SHIELD_STATE, GroupProps.POWER_FIELD_SHIELD_STATE}});
            }
        }
        if (_game && _game.Connected && ViewCreated && _game.Player.VisibilityToOtherPlayers && _playerItemComponent) {
            if (Time.time >= _sendTransformLastTime + _SEND_TRANSFORM_INTERVAL) {
                _sendTransformLastTime = Time.time;
                if (_lastPosition != _playerItemComponent.transform.position || _lastRotation != _playerItemComponent.transform.rotation.eulerAngles) {
                    _game.Operations.Add(OperationCode.MoveItem, new object[] { _playerItemComponent.transform.position, _playerItemComponent.transform.rotation.eulerAngles });
                }
            }
            if (Time.time >= _updateInterestAreaLastTime + _UPDATE_INTEREST_AREA_INTERVAL) {
                _updateInterestAreaLastTime = Time.time;
                _game.Operations.Add(OperationCode.UpdateInterestArea, new object[] { });
            }
        }
    }

    protected override void OnSettedProperties(string group, Hashtable properties)
    {
        //throw new System.NotImplementedException();
    }

    protected override void SetSpaceObject()
    {
        this.spaceObject = _playerItemComponent;
    }

    public override void DestroyView()
    {
        base.DestroyView();
    }
    public override void CreateView(GameObject prefab, bool isLocalPlayer)
    {
        base.CreateView(prefab, isLocalPlayer);
        PlayerItemComponent component = View.AddComponent<PlayerItemComponent>();
        component.Setup(_game);
        _playerItemComponent = component;
        View.AddComponent<PlayerSpaceshipControl>().SetRotateSpeed(1);
        View.AddComponent<SpaceMovingTrailManager>().Setup(
            PrefabCache.Get("Prefabs/Effects/SpacemovingTrail"),
            5.0f, 0.5f, 100, 20, 200
            );
        SetSpaceObject();
    }
    #endregion

    #region Events
    //Item remove event handler
    void OnItemRemoved(ItemType type, string id)
    {
        //reset removed item if he is our target
        if (_hasTarget)
        {
            if (_target != null && !_target.IsLocal())
            {
                ServerItem serverItem = _target.GetItem() as ServerItem;
                if (serverItem != null && serverItem.Removed)
                {
                    ResetTarget();
                }
                else
                {
                    if (serverItem == null)
                        DebugConsole.PrintError("OnItemRemoved", string.Format("Removed server item of type: {0} with id: {1} is null", type, id));
                    else if (!serverItem.Removed)
                        DebugConsole.PrintError("OnItemRemoved", string.Format("Removed server item of type: {0} with id: {1} not marked as removed", type, id));
                }
            }
        }
    }

    void Events_ServerItemPropertiesReceived(string itemId, string group, Hashtable properties)
    {
        if (Id == itemId)
        {
            switch (group)
            {
                case GroupProps.SHIP_BASE_STATE:
                    foreach (DictionaryEntry entry in properties)
                    {
                        switch (entry.Key.ToString())
                        {
                            case Props.SHIP_BASE_STATE_CONTROL_STATE:
                                {
                                    PlayerState state = (PlayerState)(byte)entry.Value;
                                    switch (state)
                                    {
                                        case PlayerState.MoveToTarget:
                                            if (_hasTarget && _target != null)
                                                _playerItemComponent.ControlComponent.SetTarget(_target);
                                            break;
                                        case PlayerState.MoveDirection:
                                            break;
                                        case PlayerState.Idle:
                                            break;
                                        case PlayerState.JumpToTarget:
                                            if (_hasTarget && _target != null && View)
                                                _playerItemComponent.ControlComponent.SetTarget(_target);
                                            break;
                                        case PlayerState.OrbitToTarget:
                                            if (_hasTarget && _target != null)
                                            {
                                                _playerItemComponent.ControlComponent.SetTarget(_target);
                                                _playerItemComponent.ControlComponent.InitOrbitState();
                                            }
                                            break;
                                    }
                                }
                                break;
                            case Props.SHIP_BASE_STATE_ACTION_STATE:
                                {
                                    PlayerState state = (PlayerState)(byte)entry.Value;
                                    switch (state)
                                    {
                                        case PlayerState.FireToTarget:
                                            if (_hasTarget && _target != null)
                                            {
                                                _playerItemComponent.ControlComponent.SetTarget(_target);
                                                _playerItemComponent.ControlComponent.InitFireState();
                                            }
                                            break;
                                        case PlayerState.Idle:
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case GroupProps.DEFAULT_STATE:
                    {
                        foreach (DictionaryEntry entry in properties)
                        {
                            switch (entry.Key.ToString())
                            {
                                case Props.DEFAULT_STATE_HAS_TARGET:
                                    {
                                        _hasTarget = (bool)entry.Value;
                                        if (!_hasTarget)
                                        {
                                            _target = null;
                                            _playerItemComponent.ControlComponent.SetTarget(_target);
                                        }
                                        _playerItemComponent.ControlComponent.SetTarget(_target);
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }
        }
    }

    #endregion
}

*/
