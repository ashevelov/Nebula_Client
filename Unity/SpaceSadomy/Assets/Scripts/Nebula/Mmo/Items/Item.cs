    
namespace Nebula.Mmo.Items
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Collections;
    using Common;
    using Nebula.Mmo.Games;
    using Nebula.Mmo.Items.Components;
    using Nebula.Resources;

    public abstract class Item
    {
        private readonly NetworkGame game;
        private readonly string id;
        private List<byte> subscribedInterestAreas;
        private readonly byte type;
        private readonly List<byte> visibleInterestAreas;
        public float[] Position { get; private set; }
        public float[] Rotation { get; private set; }
        public float[] PreviousPosition { get; private set; }
        public float[] PreviousRotation { get; private set; }
        public int PropertyRevision { get; set; }
        public event Action<Item> Moved;
        private Dictionary<byte, object> properties;
        protected GameObject _view;
        protected NetworkTransformInterpolation _transformInterpolation;
        protected bool _subscribed;
        private string _name;
        private bool _shipDestroyed;
        private float movedSpeed = 0f;
        public ComponentID[] componentIDS { get; private set; }


        public Race Race
        {
            get
            {
                if(ContainsProperty((byte)PS.Race)) {
                    return (Race)GetProperty<byte>((byte)PS.Race);
                } else {
                    return Race.None;
                }
            }
        }

        public void ReplaceName(string nm)
        {
            this._name = nm;
        }

        public void SetSubscribed(bool subscribed) {
            //"item of [{0}] subscribed [{1}]".f(this.type.toItemType(), subscribed).Bold().Color( subscribed ? Color.green : Color.yellow).Print();
            _subscribed = subscribed;
        }


        public void SetProperty(byte propName, object value )
        {
            object oldValue = null;
            if(properties.ContainsKey(propName)) {
                oldValue = properties[propName];
            }
            properties[propName] = value;
            if(!IsMine) {
                if(propName == (byte)PS.InterestAreaAttached) {
                    SetInterestAreaAttached((bool)value);
                    MakeVisibleToSubscribedInterestAreas();
                }
            }
            OnPropertySetted(propName, oldValue, value);
        }

        public void SetProperties(Hashtable inProperties)
        {
            foreach(DictionaryEntry entry in inProperties) {
                SetProperty((byte)entry.Key, entry.Value);
            }
        }

        public virtual void OnPropertySetted(byte key, object oldValue, object newValue) {

        }

        public bool ContainsProperty(byte name)
        {
            return properties.ContainsKey(name);
        }

        public T GetProperty<T>(byte name)
        {

            if(properties.ContainsKey(name)) {
                return (T)properties[name];
            }
            return default(T);
        }

        public bool TryGetProperty<T>(byte name, out T value) {
            if(properties.ContainsKey(name)) {
                value = (T)properties[name];
                return true;
            }
            value = default(T);
            return false;
        }

        public NetworkGame Game
        {
            get
            {
                return this.game;
            }
        }

        public string Id
        {
            get
            {
                return this.id;
            }
        }

        public bool InterestAreaAttached { get; private set; }
        public bool IsDestroyed { get; set; }
        public abstract bool IsMine { get; }

        public bool IsVisible
        {
            get
            {
                return this.visibleInterestAreas.Count > 0;
            }
        }

        public byte Type
        {
            get
            {
                return this.type;
            }
        }

        /*
        public float[] ViewDistanceEnter { get; private set; }
        public float[] ViewDistanceExit { get; private set; }
        */

        public Hashtable RawProperties
        {
            get
            {
                Hashtable result = new Hashtable();
                foreach(var pair in properties) {
                    result.Add(pair.Key, pair.Value);
                }
                return result;
            }
        }

        public Dictionary<byte, object> props {
            get {
                return properties;
            }
        }


        protected Dictionary<ComponentID, MmoBaseComponent> components { get; set; }

        protected Item(string id, byte type, NetworkGame game, string name, object[] inComponents)
        {
            this.id = id;
            _name = StringCache.Get(name);
            this.game = game;
            this.type = type;
            this.visibleInterestAreas = new List<byte>();
            this.subscribedInterestAreas = new List<byte>();
            properties = new Dictionary<byte, object>();

            if (inComponents != null) {
                componentIDS = new ComponentID[inComponents.Length];
                for(int i = 0; i < inComponents.Length; i++ ) {
                    componentIDS[i] = (ComponentID)(int)inComponents[i];
                }
            } else {
                componentIDS = new ComponentID[] { };
            }

            components = new Dictionary<ComponentID, MmoBaseComponent>();
            foreach(var cID in componentIDS) {
                var component = MmoBaseComponent.CreateNew(cID);
                if (component != null) {
                    components.Add(cID, component);
                    component.SetItem(this);
                }
            }
        }

        public MmoBaseComponent GetMmoComponent(ComponentID cID ) {
            if(components.ContainsKey(cID)) {
                return components[cID];
            }
            return null;
        }



        public bool AddSubscribedInterestArea(byte cameraId)
        {
            if (this.subscribedInterestAreas.Contains(cameraId))
            {
                return false;
            }
            this.subscribedInterestAreas.Add(cameraId);
            return true;
        }

        public bool AddVisibleInterestArea(byte cameraId)
        {
            if (this.visibleInterestAreas.Contains(cameraId))
            {
                return false;
            }
            this.visibleInterestAreas.Add(cameraId);
            return true;
        }

        public void GetInitialProperties()
        {
            Operations.GetProperties(this.game, this.id, this.type, null );
        }

        public void GetProperties( )
        {
            Operations.GetProperties(this.game, this.id, this.type, this.PropertyRevision);
        }

        public void MakeVisibleToSubscribedInterestAreas()
        {
            this.subscribedInterestAreas.ForEach(b => this.AddVisibleInterestArea(b));
        }
        public bool RemoveSubscribedInterestArea(byte cameraId)
        {
            return this.subscribedInterestAreas.Remove(cameraId);
        }
        public bool RemoveVisibleInterestArea(byte cameraId)
        {
            return this.visibleInterestAreas.Remove(cameraId);
        }
        public void ResetPreviousPosition()
        {
            this.PreviousPosition = null;
        }
        public virtual void SetInterestAreaAttached(bool attached)
        {
            this.InterestAreaAttached = attached;
        }

        /*
        public virtual void SetInterestAreaViewDistance(float[] viewDistanceEnter, float[] viewDistanceExit)
        {
            this.ViewDistanceEnter = viewDistanceEnter;
            this.ViewDistanceExit = viewDistanceExit;
        }

        public virtual void SetViewDistanceEnter(float[] viewDistanceEnter) { 
            this.ViewDistanceEnter = viewDistanceEnter; 
        }

        public virtual void SetViewDistanceExit(float[] viewDistanceExit) { 
            this.ViewDistanceExit = viewDistanceExit; 
        }*/


        public void SetPositions(float[] position, float[] previousPosition, float[] rotation, float[] previousRotation, float speed)
        {
            this.Position = position;
            this.PreviousPosition = previousPosition;
            this.Rotation = rotation;
            this.PreviousRotation = previousRotation;
            this.movedSpeed = speed;
            this.OnMoved();
        }

        private void OnMoved()
        {
            if (IsMine == false) {
                if (_transformInterpolation) {
                    _transformInterpolation.ReceivedData(new ExtrapolationData { Position = this.Position.toVector(), Rotation = this.Rotation.toVector(), Time = Time.time, Speed = this.movedSpeed });
                }
            }
            if (this.Moved != null)
            {
                this.Moved(this);
            }
        }

        public virtual void Create(GameObject obj)
        {
            if (false == this.ExistsView)
            {
                obj.transform.position = (Position != null) ? Position.toVector() : Vector3.zero;
                obj.transform.rotation = (Rotation != null ? Quaternion.Euler(Rotation.toVector()) : Quaternion.identity);
                _view = obj;
                _view.name = "A_player" + (this.IsMine ? "MY" : this.Id.Substring(0, 3)) + "(" + this.Type.toItemType().ToString() + ")";
                _transformInterpolation = _view.AddComponent<NetworkTransformInterpolation>();
            }
        }

        public virtual void DestroyView() 
        {
            if (true == this.ExistsView)
            {
                if (IsMine)
                {
                    Debug.Log("Destroy mine view");
                }
                GameObject.Destroy(_view);
                _view = null;
            }
        }

        public bool ExistsView 
        { 
            get 
            {
                return this._view;
            } 
        }

        public GameObject View { 
            get { 
                return _view;  
            } 
        }

        public bool HasView()
        {
            return (bool)this._view;
        }

        public virtual BaseSpaceObject Component { get; private set; }

        public void SetComponent(BaseSpaceObject spaceObject )
        {
            this.Component = spaceObject;
        }
        

        public virtual void SkillUsed(Hashtable skillProperties) {
            if(View) {
                View.GetComponent<BaseSpaceObject>().UseSkill(skillProperties);
            }
        }

        public Vector3 GetPosition() 
        {
            if (Position == null) {
                return Vector3.zero;
            }
            return new Vector3(Position[0], Position[1], Position[2]);
        }

        public bool Subscribed {
            get {
                return _subscribed;
            }
        }

        public string Name {
            get {
                return _name;
            }
        }

        public bool ShipDestroyed 
        {
            get 
            {
                return _shipDestroyed;
            }
        }

        public virtual void SetShipDestroyed(bool shipDetroyed)
        {
            bool oldDestroyed = this._shipDestroyed;
            _shipDestroyed = shipDetroyed;

            if ((false == oldDestroyed) && (true == this._shipDestroyed))
            {
                if (this.Component)
                {
                    this.Component.OnShipWasDestroyed();
                }
            }
        }

        public abstract void AdditionalUpdate();
    }
}
