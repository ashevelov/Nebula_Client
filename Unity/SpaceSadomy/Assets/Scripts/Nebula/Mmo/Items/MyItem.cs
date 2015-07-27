using Common;

namespace Nebula.Mmo.Items {
    using Nebula.Mmo.Games;
    using Nebula.UI;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MyItem : Item, IDamagable {
        private string _name;
        private AIStateData _aiState;
        private PlayerTargetState _targetState;
        private BaseSpaceObject _component;
        private bool shiftPressed = false;

        public MyItem(string id, byte type, NetworkGame game, string name, object[] inComponents)
            : base(id, type, game, name, inComponents) {
            _name = name;
            _aiState = new AIStateData(this);
            //_ship = new PlayerShip( this );
            _targetState = new PlayerTargetState(this);
            this._targetState.SetTargetUpdated(game.OnTargetUpdated);
            //_worldEvents = new PlayerEvents();
        }

        public override bool IsMine {
            get { return true; }
        }

        public bool IsMoving { get; set; }

        public string Name {
            get { return _name; }
        }

        //public LoginInfo LoginInfo {
        //    get {
        //        return _loginInfo;
        //    }
        //}

        public AIStateData AI { get { return _aiState; } }
        //public PlayerShip Ship { get { return _ship; } }

        public PlayerTargetState Target { get { return _targetState; } }

        /*
        public PlayerEvents WorldEvents {
            get {
                return _worldEvents;
            }*/

        //public void SetLoginInfo(LoginInfo loginInfo) {
        //    _loginInfo = loginInfo;
        //}

        /*
        public void Destroy()
        {
            this.IsDestroyed = true;
            Operations.DestroyItem(this.Game, this.Id, this.Type);
        }*/

        //public void EnterWorld()
        //{
        //    var position = new float[] { 0.0f, 0.0f, NetworkGame.START_Z };
        //    this.SetPositions(position, position, null, null, 0);
        //    var properties = new Hashtable
        //        {
        //            { Props.DEFAULT_STATE_INTEREST_AREA_ATTACHED, this.InterestAreaAttached }, 
        //            { Props.DEFAULT_STATE_VIEW_DISTANCE_ENTER, Game.Settings.ViewDistanceEnter}, 
        //            { Props.DEFAULT_STATE_VIEW_DISTANCE_EXIT, Game.Settings.ViewDistanceExit  }, 
        //        };
        //    Dictionary<string, Hashtable> dictProps = new Dictionary<string, Hashtable> { { GroupProps.DEFAULT_STATE, properties } };

        //    //Operations.EnterWorld(this.Game, this.Game.World.Name, this.Id, dictProps, this.Position, this.Rotation, Game.Settings.ViewDistanceEnter, Game.Settings.ViewDistanceExit );
        //    Operations.EnterWorld(this.Game, this.Game.World.Name, dictProps, this.Position, this.Rotation, Game.Settings.ViewDistanceEnter, 
        //        Game.Settings.ViewDistanceExit, this.Game.LoginInfo.gameRefId);
        //}

        public bool MoveAbsolute(float[] newPosition, float[] rotation) {
            this.SetPositions(newPosition, this.Position, rotation, this.Rotation, 0);
            Operations.Move(this.Game, this.Id, this.Type, newPosition, rotation, this.Game.Settings.SendReliable);
            return true;
        }

        public bool MoveRelative(float[] offset, float[] rotation) {
            return this.MoveAbsolute(new[] { this.Position[0] + offset[0], this.Position[1] + offset[1] }, rotation);
        }


        public override void SetInterestAreaAttached(bool attached) {
            if (attached != this.InterestAreaAttached) {
                base.SetInterestAreaAttached(attached);
                Hashtable props = new Hashtable();
                props.Add((byte)PS.InterestAreaAttached, attached);
                Operations.SetProperties(this.Game, this.Id, this.Type, props, null, true);
            }
        }

        /*
        public override void SetInterestAreaViewDistance(float[] viewDistanceEnter, float[] viewDistanceExit)
        {
            Hashtable props = new Hashtable { 
                { Props.DEFAULT_STATE_VIEW_DISTANCE_ENTER, viewDistanceEnter },
                {Props.DEFAULT_STATE_VIEW_DISTANCE_EXIT, viewDistanceExit }
            };

            Operations.SetProperties(
                this.Game,
                this.Id,
                this.Type,
                new Dictionary<string, Hashtable> { { GroupProps.DEFAULT_STATE, props } },
                null,
                true);
        }


        public void SetInterestAreaViewDistance(InterestArea camera)
        {
            this.SetInterestAreaViewDistance(camera.ViewDistanceEnter, camera.ViewDistanceExit);
        }
         */


        public void Spawn(float[] position, float[] rotation, int color, bool subscribe) {
            this.SetPositions(position, position, rotation, rotation, 0);
            var properties = new Hashtable
                {
                    { (byte)PS.InterestAreaAttached, false },
                    { (byte)PS.ViewDistanceEnter, Game.Settings.ViewDistanceEnter },
                    { (byte)PS.ViewDistanceExit, Game.Settings.ViewDistanceExit  },
                };
            Operations.SpawnItem(this.Game, this.Id, this.Type, position, rotation, properties, subscribe);
        }


        public void Respawn() {

            if (false == ExistsView) {
                var prefabs = GameData.instance.ship.ShipModel.SlotPrefabs();
                Debug.Log("Prefabs before creation");
                foreach (var prefabPair in prefabs) {
                    Debug.Log("{0}:{1}".f(prefabPair.Key, prefabPair.Value));
                }
                var obj = ShipModel.Init(prefabs, true);
                Debug.Log(obj.name);
                this.Create(obj);
                Debug.Log(this.View.name);

                //G.Game.SetSpawnPosition(this);
                //G.Game.Avatar.SetPositions
                var pos = Game.GetSpawnPosition();
                var posArr = new float[] { pos.x, pos.y, pos.z };
                SetPositions(posArr, posArr, Rotation, Rotation, 0);
                if(ExistsView) { View.transform.position = pos;  }

                MouseOrbitRotateZoom.Get.SetTarget(this.View.transform);
                
                //SetPositions()

                if(Game.CurrentStrategy == GameState.NebulaGameWorldEntered) {
                    //additional actions when ship respawned
                    MouseOrbitRotateZoom.Get.StopGrayScae();
                    //G.UI.RespView.SetVisible(false);

                    if (MainCanvas.Get) {
                        MainCanvas.Get.Show(CanvasPanelType.ControlHUDView);
                        MainCanvas.Get.Show(CanvasPanelType.MenuHUDView);
                    }
                }

            }
        }



        public override void Create(GameObject obj) {
            base.Create(obj);
            _component = _view.AddComponent<MyPlayer>();
            _component.Initialize(Game, this);
        }

        public override BaseSpaceObject Component {
            get { return _component; }
        }

        public void RequestTarget(string targetId, byte targetType, bool hasTarget) {
            if(!hasTarget) {
                targetId = string.Empty;
            }
            if(string.IsNullOrEmpty(targetId)) {
                hasTarget = false;
            }
            Operations.ExecAction(Game, Id, "SetTarget", new object[] { hasTarget, targetId, targetType });
        }

        public void RequestLinearSpeed(float speed) {
            speed = Mathf.Clamp(speed, GameData.instance.ship.MinLinearSpeed, GameData.instance.ship.MaxLinearSpeed);
            Operations.ExecAction(Game, Id, "ChangeLinearSpeed", new object[] { speed });
        }

        public void RequestMoveDirection() {
            Operations.ExecAction(Game, Id, "ChangeControlState", new object[] { (byte)PlayerState.MoveDirection });
        }

        public void RequestControlState(PlayerState controlState) {
            Operations.ExecAction(Game, Id, "ChangeControlState", new object[] { (byte)controlState });
        }



        public void RequestStop() {
            Operations.ExecAction(Game, Id, "ChangeLinearSpeed", new object[] { 0.0f });
            Operations.ExecAction(Game, Id, "ChangeControlState", new object[] { (byte)PlayerState.Idle });
        }


        public override void OnPropertySetted(byte key, object oldValue, object newValue) {
            switch((PS)key) {
                case PS.ShiftPressed:
                    shiftPressed = (bool)newValue;
                    break;
                case PS.ControlState:
                    _aiState.ParseProp(key, newValue);
                    break;
                case PS.CurrentHealth:
                case PS.MaxHealth:
                case PS.Destroyed:
                case PS.CurrentLinearSpeed:
                case PS.Acceleration:
                case PS.MinLinearSpeed:
                case PS.MaxLinearSpeed:
                case PS.AngleSpeed:
                case PS.CurrentEnergy:
                case PS.MaxEnergy:
                    GameData.instance.ship.ParseProp(key, newValue);
                    break;
                case PS.TargetId:
                case PS.HasTarget:
                case PS.TargetType:
                    Target.ParseProp(key, newValue);
                    break;
            }
        }


        public void CreateRaiderAtMe() {
            float[] position = View.transform.position.toArray();
            float[] rotation = View.transform.rotation.eulerAngles.toArray();
            Hashtable properties = new Hashtable {
                {(byte)PS.ViewDistanceEnter, 1000.0f},
                {(byte)PS.ViewDistanceExit, 2000.0f},
                {(byte)PS.Model, 2}
            };
            Operations.ExecAction(Game, Id, "CreateRaider", new object[] { position, rotation, properties });
        }

        public void DestroyAnyRaider() {
            Operations.ExecAction(Game, Id, "DestroyAnyRaider", new object[] { });
        }


        public bool IsDead() {
            return GameData.instance.ship.Destroyed;
        }





        public float GetHealth() {
            return GameData.instance.ship.Health;
        }

        public float GetMaxHealth() {
            return GameData.instance.ship.MaxHealth;
        }

        public float GetHealth01() {
            if (GameData.instance.ship.MaxHealth == 0.0f)
                return 0.0f;
            return Mathf.Clamp01(GameData.instance.ship.Health / GameData.instance.ship.MaxHealth);
        }

        public void SetNewRandomSlotModule(ShipModelSlotType type) {
            Operations.ExecAction(Game, Id, "SetNewRandomSlotModule", new object[] { type.toByte() });
        }


        public float GetOptimalDistance() {
            if (GameData.instance.ship.Weapon.HasWeapon)
                return GameData.instance.ship.Weapon.WeaponObject.OptimalDistance;
            return 0.0f;
        }


        public float GetMaxHitSpeed() {
            return 0f;
        }


        public float GetSpeed() {
            return GameData.instance.ship.LinearSpeed;
        }



        public void GetBonuses() {
            Operations.ExecAction(Game, Id, "GetBonuses", new object[] { });
        }

        public void GetBonusesOnTarget() {
            //System.Net.Sockets.UdpClient client = null;
            Operations.ExecAction(Game, Id, "GetBonusesOnTarget", new object[] { });
        }

        //RPC
        public void TestSendServiceMessage(ServiceMessageType messageType, string message) {
            Operations.ExecAction(Game, Id, "TestSendServiceMessage", new object[] { messageType.toByte(), message });
        }

        public bool ShiftPressed {
            get {
                return this.shiftPressed;
            }
        }

        public override void AdditionalUpdate() {

        }

        public override ObjectInfoType InfoType {
            get {
                return ObjectInfoType.MyPlayer;
            }
        }

        public override string Description {
            get {
                return string.Empty;
            }
        }
    }
}
