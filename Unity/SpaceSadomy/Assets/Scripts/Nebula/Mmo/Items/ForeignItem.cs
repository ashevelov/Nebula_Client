

namespace Nebula.Mmo.Items
{
    using Common;
    using UnityEngine;
    using Nebula.Client;
    using Nebula.Mmo.Games;

    public abstract class ForeignItem : Item
    {

        protected ClientItemTargetInfo targetInfo;
        private bool _exploded = false;

        public ForeignItem(string id, byte type, NetworkGame game, string name, object[] inComponents)
            : base(id, type, game, name, inComponents) {
            this.targetInfo = ClientItemTargetInfo.Default;
        }

        public override bool IsMine {
            get {
                return false;
            }
        }


        public override void Create(GameObject obj) {
            base.Create(obj);

            if (this.ShipDestroyed) {
                this.View.SetActive(false);
            } else {
                this.View.SetActive(true);
            }
        }

        public override void DestroyView() {
            base.DestroyView();
        }

        public override void OnPropertySetted(byte key, object oldValue, object newValue) {
            switch ((PS)key) {
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

        public ClientItemTargetInfo TargetInfo {
            get {
                return this.targetInfo;
            }
        }
    }
}