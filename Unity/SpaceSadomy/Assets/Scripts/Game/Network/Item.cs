    
namespace Nebula
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Collections;
    using Common;

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
        private Dictionary<string, Hashtable> _properties;
        protected GameObject _view;
        protected NetworkTransformInterpolation _transformInterpolation;
        protected bool _subscribed;
        private string _name;
        private bool _shipDestroyed;
        private float movedSpeed = 0f;


        public Race Race
        {
            get
            {
                object objRace = this.GetProperty(GroupProps.DEFAULT_STATE, Props.DEFAULT_STATE_RACE);
                return (objRace == null) ? Race.None : (Race)(byte)objRace;
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

        private void HandleDefaultProperties(string propName, object newValue) {
            if (IsMine == false)
            {
                if (propName == Props.DEFAULT_STATE_INTEREST_AREA_ATTACHED)
                {
                    SetInterestAreaAttached((bool)newValue);
                    MakeVisibleToSubscribedInterestAreas();
                }
            }
        }

        public virtual void OnSettedProperty(string group, string propName, object newValue, object oldValue) {
            HandleDefaultProperties(propName, newValue);
        }

        public virtual void OnSettedGroupProperties(string group, Hashtable properties) {
            if (group == GroupProps.DEFAULT_STATE) {
                foreach (DictionaryEntry entry in properties) {
                    HandleDefaultProperties(entry.Key.ToString(), entry.Value);
                }
            }
        } 

        public void SetProperty(string group, string propName, object value )
        {
            if (_properties.ContainsKey(group))
            {
                Hashtable existingProperties = _properties[group];
                object oldValue = null;
                if (existingProperties.ContainsKey(propName))
                {
                    oldValue = existingProperties[propName];
                    existingProperties.Remove(propName);
                }
                existingProperties.Add(propName, value);
                OnSettedProperty(group, propName, value, oldValue);
            }
            else
            {
                Hashtable newProperties = new Hashtable { { propName, value } };
                _properties.Add(group, newProperties);
                OnSettedProperty(group, propName, value, null);
            }
        }

        public void SetProperties(string group, Hashtable properties)
        {
            /*if (IsMine) {
                Debug.Log(string.Format("set mine properties, group: {0}  count: {1}", group, properties.Count));
            }*/

            if (_properties.ContainsKey(group))
            {
                Hashtable existingProperties = _properties[group];
                foreach (DictionaryEntry entry in properties)
                {
                    object oldValue = null;
                    if (existingProperties.ContainsKey(entry.Key))
                    {
                        oldValue = existingProperties[entry.Key];
                        existingProperties.Remove(entry.Key);
                    }
                    existingProperties.Add(entry.Key, entry.Value);
                    //OnSettedProperty(group, entry.Key.ToString(), entry.Value, oldValue);
                }
                OnSettedGroupProperties(group, existingProperties);
            }
            else
            {
                Hashtable newProperties = new Hashtable(properties);
                _properties.Add(group, newProperties);
                /*foreach(DictionaryEntry entry in newProperties )
                {
                    OnSettedProperty(group, entry.Key.ToString(), entry.Value, null);
                }*/
                OnSettedGroupProperties(group, properties);
            }
        }

        public bool ContainsProperty(string group, string name)
        {
            if (this._properties.ContainsKey(group))
            {
                Hashtable props = this._properties[group];
                if (props.ContainsKey(name))
                    return true;
            }
            return false;
        }

        public object GetProperty(string group, string name)
        {
            if (false == this.ContainsProperty(group, name))
                return null;
            else
            {
                Hashtable props = this._properties[group];
                foreach (DictionaryEntry e in props)
                {
                    if (e.Key.ToString() == name)
                    {
                        return e.Value;
                    }
                }
                return null;
            }
        }

        [CLSCompliant(false)]
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
                int counter = 0;
                Hashtable rawProperties = new Hashtable();
                foreach(var pair in _properties )
                {
                    foreach (DictionaryEntry entry in pair.Value)
                    {
                        string key = string.Empty;
                        if (rawProperties.ContainsKey(entry.Key))
                        {
                            key = entry.Key.ToString() + counter.ToString();
                            counter++;
                        }
                        else
                        {
                            key = entry.Key.ToString();
                        }
                        rawProperties.Add(key, entry.Value);
                    }
                }
                return rawProperties;
            }
        }


        [CLSCompliant(false)]
        protected Item(string id, byte type, NetworkGame game, string name)
        {
            this.id = id;
            _name = StringCache.Get(name);
            this.game = game;
            this.type = type;
            this.visibleInterestAreas = new List<byte>();
            this.subscribedInterestAreas = new List<byte>();
            _properties = new Dictionary<string, Hashtable>();
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

        public void GetInitialProperties(string[] groups)
        {
            Operations.GetProperties(this.game, this.id, this.type, null, (groups == null ) ? new string[]{} : groups );
        }

        public void GetProperties(string[] groups )
        {
            Operations.GetProperties(this.game, this.id, this.type, this.PropertyRevision, (groups == null) ? new string[] { } : groups);
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


        //don't use now
        /*
        public virtual void CreateView(GameObject prefab)
        {
            if (!_existView)
            {
                if (_view)
                {
                    GameObject.Destroy(_view);
                    _view = null;
                }

                _view = GameObject.Instantiate(prefab,
                    (Position != null) ? Position.toVector() : Vector3.zero,
                    (Rotation != null ? Quaternion.Euler(Rotation.toVector()) : Quaternion.identity)) as GameObject;

                if (false == string.IsNullOrEmpty(this.Id) )
                {
                    _view.name = "1A_player" + (this.IsMine ? "MY" : ((this.Id.Length >= 3 ) ?  this.Id.Substring(0, 3) : this.Id ) ) + "(" + this.Type.toItemType().ToString() + ")";
                    if (this.type.toItemType() == ItemType.Bot)
                    {
                        var npc = this as NpcItem;
                        _view.name += "_" + npc.SubType.ToString();
                    }
                }
                else
                {
                    Debug.Log("id of view is null or empty");
                    _view.name = "1A_playerNULL" + "(" + this.Type.toItemType().ToString() + ")"; 
                }

                _transformInterpolation = _view.AddComponent<NetworkTransformInterpolation>();
                _existView = true;
            }
        }*/

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
        

        public abstract void UseSkill(Hashtable skillProperties);

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
