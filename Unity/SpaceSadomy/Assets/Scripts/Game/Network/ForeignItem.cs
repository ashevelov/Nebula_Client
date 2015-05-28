using Common;
using Game.Space;

using System;
using System.Collections;
using UnityEngine;
using Nebula.Client;

namespace Nebula
{
    /// <summary>
    /// The foreign item.
    /// </summary>
    public abstract class ForeignItem : Item
    {

        protected ClientItemTargetInfo targetInfo;
        private bool _exploded = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignItem"/> class.
        /// </summary>
        /// <param name="id">
        /// The item id.
        /// </param>
        /// <param name="type">
        /// The item type.
        /// </param>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        [CLSCompliant(false)]
        public ForeignItem(string id, byte type, NetworkGame game, string name)
            : base(id, type, game, name)
        {
            this.targetInfo = ClientItemTargetInfo.Default;
        }




        /// <summary>
        /// Gets a value indicating whether IsMine.
        /// </summary>
        public override bool IsMine
        {
            get
            {
                return false;
            }
        }

        //public override void CreateView(GameObject prefab)
        //{
        //    base.CreateView(prefab);
        //    this.View.layer = LayerMask.NameToLayer("Player");

        //    if (ShipDestroyed)
        //    {
        //        View.SetActive(false);
        //    }
        //    else 
        //    {
        //        View.SetActive(true);
        //    }
        //}

        public override void Create(GameObject obj)
        {
            base.Create(obj);
            //this.View.layer = LayerMask.NameToLayer("Player");

            if (this.ShipDestroyed)
            {
                this.View.SetActive(false);
            }
            else
            {
                this.View.SetActive(true);
            }
        }

        public override void DestroyView()
        {
            base.DestroyView();
        }


        public override void OnSettedGroupProperties(string group, System.Collections.Hashtable properties)
        {
            base.OnSettedGroupProperties(group, properties);

            if (group == GroupProps.target_info)
            {
                foreach (DictionaryEntry entry in properties)
                {
                    object oldValue = this.GetProperty(group, entry.Key.ToString());
                    this.OnSettedProperty(group, entry.Key.ToString(), entry.Value, oldValue);
                }
            }
        }

        public override void OnSettedProperty(string group, string propName, object newValue, object oldValue)
        {
            base.OnSettedProperty(group, propName, newValue, oldValue);

            switch (group)
            {
                case GroupProps.target_info:
                    {
                        switch (propName)
                        {
                            case Props.has_target:
                                this.targetInfo.SetHasTarget((bool)newValue);
                                break;
                            case Props.target_id:
                                this.targetInfo.SetTargetId((string)newValue);
                                break;
                            case Props.target_type:
                                this.targetInfo.SetTargetType((byte)newValue);
                                break;
                        }
                    }
                    break;
            }
        }

        public ClientItemTargetInfo TargetInfo
        {
            get
            {
                return this.targetInfo;
            }
        }
    }
}