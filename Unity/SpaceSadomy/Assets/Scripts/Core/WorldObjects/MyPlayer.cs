using Common;
using Nebula;
using Nebula.Mmo.Games;
using Nebula.Mmo.Items;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyPlayer : BaseSpaceObject {
    //private float _linearSpeed = 10.0f;
    private Vector3 _targetDirection;
    private float _angTheta;
    private float _angPhi;
    private bool _orbitInitialized;
    private float _orbitAngleSpeed;
    private Vector3 _orbitSmoothingVelocity;
    private float _orbitSmoothTime = 0.3f;
    private float _requestWeaponNextTime;

    private Vector3 _lastMovePosition;
    private Vector3 _lastMoveRotation;
    private float _nextMoveTime;
    private float _nextUpdatePropertiesTime;

    public enum ControlState { WaitState, Move, Jump, Orbit, Dead, Idle }

    private BaseFSM<ControlState> controlFSM = new BaseFSM<ControlState>();


    private bool inDeepSpace = false;


    public BaseFSM<ControlState> ControlFSM
    {
        get
        {
            return this.controlFSM;
        }
    }


    public bool InDeepSpace
    {
        get
        {
            return this.inDeepSpace;
        }
    }

    public void SetInDeepSpace(bool val)
    {
        this.inDeepSpace = val;
    }


    #region BaseSpaceObject overrides
    public override void Start() 
    {
        base.Start();

        FSMState<ControlState> cWait = new FSMState<ControlState>(ControlState.WaitState, BeginWaitForState, WaitForState, EndWaitForState);
        FSMState<ControlState> cMove = new FSMState<ControlState>(ControlState.Move, BeginMoveDirection, MoveDirection, EndMoveDirection);
        FSMState<ControlState> cIdle = new FSMState<ControlState>(ControlState.Idle, BeginIdle, Idle, EndIdle);
        FSMState<ControlState> cJump = new FSMState<ControlState>(ControlState.Jump, BeginJump2Target, Jump2Target, EndJump2Target);
        FSMState<ControlState> cOrbit = new FSMState<ControlState>(ControlState.Orbit, BeginOrbitTarget, OrbitTarget, EndOrbitTarget);
        FSMState<ControlState> cDestroyed = new FSMState<ControlState>(ControlState.Dead, BeginDestroyed, Destroyed, EndDestroyed);
        this.controlFSM.AddState(cWait);
        this.controlFSM.AddState(cMove);
        this.controlFSM.AddState(cIdle);
        this.controlFSM.AddState(cJump);
        this.controlFSM.AddState(cOrbit);
        this.controlFSM.AddState(cDestroyed);
        this.controlFSM.ForceState(ControlState.Idle);



        //make request to ship model at start
        NRPC.RequestShipModel();
    }

    public override void OnDestroy() 
    {
    }

    private void BeginWaitForState() { 
    
    }

    private void WaitForState() { 
    
    }

    private void EndWaitForState() { 
    
    }

    private void BeginMoveDirection() { 
    
    }

    private float _angleSpeed = 0;
    //private Rigidbody _rigidbody = null;
    private Vector3 _velocity;
    private Vector3 _rotate;

    private void MoveDirection() 
    {
        //if (_rigidbody == null)
        //{
        //    _rigidbody = transform.GetComponent<Rigidbody>();
        //}
        var game = G.Game;
        //MyItem myItem = Item as MyItem;
        if (_targetDirection == Vector3.zero)
            _targetDirection = transform.forward;
        float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(_targetDirection));
        float aSpd = GameData.instance.ship.AngleSpeed * 50;
        _angleSpeed = ((aSpd < angle) ? aSpd : angle);
        _angleSpeed = (angle > 0.1f) ? angle : 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_targetDirection), Time.deltaTime * _angleSpeed);
        //_linearSpeed = Mathf.Lerp(_linearSpeed, game.Ship.LinearSpeed, Time.deltaTime * game.Ship.Acceleration);
        //_angleSpeed -= (_angleSpeed / 2) * Time.deltaTime;


        //Vector3 delta = transform.eulerAngles - Quaternion.LookRotation(_targetDirection).eulerAngles;

        //delta.x = ClampAngle(delta.x, -50, 50);
        //delta.y = ClampAngle(delta.y, -50, 50);
        //delta.z = ClampAngle(delta.z, -50, 50);

        //_rotate -= delta.normalized * _angleSpeed * angle * Time.deltaTime;
        //_rotate -= (_rotate /2 ) * Time.deltaTime;
        //transform.eulerAngles += _rotate;


        ////_rigidbody.velocity += transform.forward * Game.Ship.LinearSpeed * Time.deltaTime;
        ////_rigidbody.velocity -= (_rigidbody.velocity / 2) * Time.deltaTime;
        _velocity += transform.forward * GameData.instance.ship.LinearSpeed * Time.deltaTime;
        _velocity -= (_velocity ) * Time.deltaTime;
        transform.position += _velocity * Time.deltaTime;

        //transform.position += transform.forward * Game.Ship.LinearSpeed * Time.deltaTime;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -180)
            angle += 360;
        if (angle > 180)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    private void EndMoveDirection() { 
    
    }

    private void BeginIdle() { 
    
    }

    private void Idle() 
    {
        transform.position += transform.forward * GameData.instance.ship.LinearSpeed * Time.deltaTime;
    }

    private void EndIdle() { 
        
    }

    private void BeginJump2Target() {
        print("begin jump state");
    }

    private void Jump2Target() 
    {
        MyItem myItem = Item as MyItem;
        if (myItem.Target.HasTargetAndTargetGameObjectValid)
        {
            //print(string.Format("STATE JUMP2TARGET, lin speed: {0} rot speed: {1}", _linearSpeed, myItem.Ship.AngleSpeed));
            Vector3 dir = (myItem.Target.Item.View.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * GameData.instance.ship.AngleSpeed);
            //_linearSpeed = Mathf.Lerp(_linearSpeed, Game.Ship.LinearSpeed, Time.deltaTime * Game.Ship.Acceleration);
            transform.position += transform.forward * GameData.instance.ship.LinearSpeed * Time.deltaTime;
            float distanceToTarget = Vector3.Distance(transform.position, myItem.Target.Item.View.transform.position);
            if (distanceToTarget <= 500)
            {
                if (false == this.controlFSM.IsState(ControlState.WaitState))
                {
                    myItem.RequestLinearSpeed(0.0f);
                    this.controlFSM.GotoState(ControlState.WaitState);
                }
            }
        }
        else 
        {
            if (false == this.controlFSM.IsState(ControlState.WaitState))
            {
                this.controlFSM.GotoState(ControlState.WaitState);
            }
        }
    }

    private void EndJump2Target() {
        print("end jump state");
    }

    private void BeginOrbitTarget() 
    {
        float dist = Vector3.Distance(transform.position, Game.Avatar.Target.Item.View.transform.position);
        Vector3 diff = transform.position - Game.Avatar.Target.Item.View.transform.position;
        _angTheta = Mathf.Atan2(diff.x * diff.x + diff.y * diff.y, diff.z);
        _angPhi = Mathf.Atan2(diff.y, diff.x);
        float orbitLength = 2.0f * Mathf.PI * dist;
        float period = 0.0f;
        if (GameData.instance.ship.LinearSpeed != 0.0f)
            period = orbitLength / GameData.instance.ship.LinearSpeed;
        else
            period = orbitLength;
        _orbitAngleSpeed = 2.0f * Mathf.PI / period;
        _orbitSmoothingVelocity = Vector3.zero;
    }

    private void OrbitTarget() {
        MyItem myItem = Item as MyItem;

        if (Game.Avatar.Target.HasTargetAndTargetGameObjectValid)
        {
            float dist = Vector3.Distance(transform.position, Game.Avatar.Target.Item.View.transform.position);
            if (dist <= 500)
            {
                _angTheta += _orbitAngleSpeed * Time.deltaTime;
                _angPhi += _orbitAngleSpeed * Time.deltaTime;
                float dx = dist * Mathf.Sin(_angTheta) * Mathf.Cos(_angPhi);
                float dy = dist * Mathf.Sin(_angTheta) * Mathf.Sin(_angPhi);
                float dz = dist * Mathf.Cos(_angTheta);
                Vector3 newPosition = Game.Avatar.Target.Item.View.transform.position + new Vector3(dx, dy, dz);
            }
            else
            {
                Vector3 dir = (Game.Avatar.Target.Item.View.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * GameData.instance.ship.AngleSpeed);
                //_linearSpeed = Mathf.Lerp(_linearSpeed, Game.Ship.LinearSpeed, Time.deltaTime * Game.Ship.Acceleration);
                transform.position += transform.forward * GameData.instance.ship.LinearSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (false == this.controlFSM.IsState(ControlState.Idle))
            {
                myItem.RequestControlState(PlayerState.Idle);
                this.controlFSM.GotoState(ControlState.WaitState);
            }
        }
    }

    private void EndOrbitTarget() { 
    
    }

    private void BeginActionIdle() { 
    
    }

    private void ActionIdle() { 
    
    }

    private void EndActionIdle() { 
    
    }

    private void BeginDestroyed() { 
    
    }


    private void Destroyed() { 
    
    }

    private void EndDestroyed() { 
    
    }

    private float lastShiftRequestTime;

    public override void Update() {

        base.Update();

        if (Game != null) 
        {
            MyItem myItem = Item as MyItem;

            this.controlFSM.Update();

#if UNITY_EDITOR|| UNITY_STANDALONE
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                _targetDirection = ray.direction;

                myItem.RequestControlState(PlayerState.MoveDirection);

				if(EventSystem.current.IsPointerOverGameObject()) {
					Debug.LogFormat("Event OVER game object {0}", "he");

				} else {
					Debug.Log("Event system not at game object");
				}
                if (false == this.controlFSM.IsState(ControlState.Move))
                {
                    this.controlFSM.GotoState(ControlState.WaitState);
                }
            }
#endif
			bool wasDoubleTap = false;
			Touch doubleTapTouch = new Touch();

			for(int i = 0; i < Input.touchCount; i++ ) {
				var touch = Input.GetTouch (i);
				if( touch.tapCount >= 2 ) {
					wasDoubleTap = true;
					doubleTapTouch = touch;
					break;
				}
			}

			if(wasDoubleTap && (!EventSystem.current.IsPointerOverGameObject())) {
				Ray ray = Camera.main.ScreenPointToRay(doubleTapTouch.position);
				this._targetDirection = ray.direction;
				myItem.RequestControlState(PlayerState.MoveDirection);
				if (!this.controlFSM.IsState(ControlState.Move)) {
					this.controlFSM.GotoState(ControlState.WaitState);
				}
			}

            Move();

            /*
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                if (myItem.ShiftPressed == false)
                {
                    Dbg.Print("SHIFT DOWN");
                    this.Game.RequestShiftDown();
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                if (myItem.ShiftPressed)
                {
                    Dbg.Print("SHIFT UP");
                    this.Game.RequestShiftUp();
                }
            }*/
        }

    }


    


    public void SetTargetDirection(Vector3 direction)
    {
        this._targetDirection = direction;
        if (false == this.controlFSM.IsState(ControlState.Move))
        {
            ((MyItem)this.Item).RequestControlState(PlayerState.MoveDirection);
            this.controlFSM.GotoState(ControlState.WaitState);
        }
    }


    private void Move()
    {
        if (Time.time > _nextMoveTime)
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            if(_lastMovePosition != transform.position || _lastMoveRotation != rotation )
            {
                Game.Avatar.MoveAbsolute(transform.position.toArray(), rotation.toArray());
                _lastMovePosition = transform.position;
                _lastMoveRotation = rotation;
            }
            _nextMoveTime = Time.time + 0.1f;
        }
    }

    private float nextUpdateBonusesTime;

    /*
    private void UpdateProperties()
    {
        if (Time.time > _nextUpdatePropertiesTime)
        {
            _nextUpdatePropertiesTime = Time.time + 0.5f;
            Game.Avatar.GetProperties(new string[] { 
                GroupProps.DEFAULT_STATE, 
                GroupProps.MECHANICAL_SHIELD_STATE, 
                GroupProps.POWER_FIELD_SHIELD_STATE, 
                GroupProps.SHIP_BASE_STATE});
           
        }
        if (Time.time > nextUpdateBonusesTime) {
            nextUpdateBonusesTime = Time.time + 0.3f;
            MmoEngine.Get.Game.Avatar.GetBonuses();
        }
    }*/

    public override void Initialize(NetworkGame game, Item item)
    {
        
        base.Initialize(game, item);
        if (Game.Avatar != null)
        {
            Operations.GetProperties(Game, Game.Avatar.Id, Game.Avatar.Type, null);
        }
    }


    #endregion


    public void MoveToDirection(Vector3 direction)
    {
        if (Game != null)
        {
            MyItem myItem = Item as MyItem;
            _targetDirection = direction;

            myItem.RequestControlState(PlayerState.MoveDirection);
            if (false == this.controlFSM.IsState(ControlState.Move))
            {
                this.controlFSM.GotoState(ControlState.WaitState);
            }
        }
    }

    /// <summary>
    /// Move control state of player to other state from outer call( server )
    /// </summary>
    /// <param name="state"></param>
    public void GotoControlState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                this.controlFSM.GotoState(ControlState.Idle);
                break;
            case PlayerState.JumpToTarget:
                this.controlFSM.GotoState(ControlState.Jump);
                break;
            case PlayerState.MoveDirection:
                this.controlFSM.GotoState(ControlState.Move);
                break;
            case PlayerState.OrbitToTarget:
                this.controlFSM.GotoState(ControlState.Orbit);
                break;
        }
    }


}
