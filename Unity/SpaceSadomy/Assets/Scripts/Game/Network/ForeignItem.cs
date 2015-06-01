using Common;
using Game.Space;

using System;
using System.Collections;
using UnityEngine;
using Nebula.Client;
using Nebula.Mmo.Games;

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

        public override void OnPropertySetted(byte key, object oldValue, object newValue) {
            switch((PS)key) {
                case PS.HasTarget:
                    targetInfo.SetHasTarget((bool)newValue);
                    break;
                case PS.TargetId:
                    targetInfo.SetTargetId((string)newValue);
                    break;
                case PS.TargetType:
                    targetInfo.SetTargetType((byte)newValue);
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