using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using Nebula.Client.Inventory;

namespace Game.Space
{
    public class CurrentObjectContainer 
    {
        private List<IInventoryObjectInfo> _containerList;
        private string _containerId;
        private byte _containerType;
        private IInventoryObjectInfo _selectedInfo;

        public CurrentObjectContainer() {
            _containerList = new List<IInventoryObjectInfo>();
            _containerId = string.Empty;
            _containerType = (byte)ItemType.Avatar;
        }

        public void Reset() {
            _containerList = new List<IInventoryObjectInfo>();
            _containerId = string.Empty;
            _containerType = (byte)ItemType.Avatar;
            _selectedInfo = null;
        }

        public void SetSelected(IInventoryObjectInfo obj) {
            _selectedInfo = obj;
        }

        public void SetContainer(string containerId, byte containerType, List<IInventoryObjectInfo> containerList) {
            _containerList = containerList;
            _containerId = containerId;
            _containerType = containerType;
            Debug_PrintContainer();
        }

        public List<IInventoryObjectInfo> List {
            get {
                return _containerList;
            }
        }

        public List<IInventoryObjectInfo> FilteredItems(InventoryObjectType type)
        {
            List<IInventoryObjectInfo> result = new List<IInventoryObjectInfo>();
            foreach (var ce in this._containerList) {
                if (ce.Type == type) {
                    result.Add(ce);
                }
            }
            return result;
        }

        /// <summary>
        /// Current container item id
        /// </summary>
        public string ContainerItemId {
            get {
                return _containerId;
            }
        }

        /// <summary>
        /// Current container item type
        /// </summary>
        public byte ContainerItemType {
            get {
                return _containerType;
            }
        }

        /// <summary>
        /// Verify container equlaity
        /// </summary>
        public bool IsSelectedContainerItem(string containerItemId, byte containerItemType) {
            return (_containerId == containerItemId) && (_containerType == containerItemType);
        }

        public bool IsSelectedObject(string objectID) {
            if (_selectedInfo != null) {
                return _selectedInfo.Id == objectID;
            }
            return false;
        }

        /// <summary>
        /// Remove object by id and return true if object was presented in list, else return false
        /// </summary>
        public bool RemoveObject(string objectId) {
            int targetIndex = -1;
            for (int i = 0; i < _containerList.Count; ++i) {
                if (_containerList[i].Id == objectId) {
                    targetIndex = i;
                    break;
                }
            }
            if (targetIndex >= 0)
            {
                _containerList.RemoveAt(targetIndex);
                return true;
            }
            else
            {
                return false;
            }

            if (_selectedInfo != null) {
                if (_selectedInfo.Id == objectId) {
                    _selectedInfo = null;
                }
            }
        }

        public bool HasSelected {
            get {
                return _selectedInfo != null;
            }
        }

        public IInventoryObjectInfo SelectedObject {
            get {
                return _selectedInfo;
            }
        }
        

        private void Debug_PrintContainer() {
            string result = string.Empty;
            foreach (var e in _containerList) {
                result += string.Format("{0},", e.ToString());
            }
            Dbg.Print("Received container: " + result);
        }
    }
}
