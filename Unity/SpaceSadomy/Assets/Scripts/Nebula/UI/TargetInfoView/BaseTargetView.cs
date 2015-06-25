using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Game.Space.UI;
using System.Collections.Generic;
using Common;

namespace Nebula.UI {
    public class BaseTargetView : MonoBehaviour {

        public const float UPDATE_INTERVAL = 2.0f;

        public Text NameText;
        public Image IconImage;
        public Text DistanceText;
        public ObjectInfoType[] ObjectType;

        protected IObjectInfo objectInfo;
        protected float updateTimer = UPDATE_INTERVAL;
        
        public virtual void SetObject(IObjectInfo objectInfo) {
            this.objectInfo = objectInfo;

            if (this.objectInfo == null) {
                this.gameObject.SetActive(false);
                return;
            }
            if (!ValidObjectType(this.objectInfo.InfoType)) {
                this.gameObject.SetActive(false);
                return;
            }

            this.gameObject.SetActive(true);
            this.NameText.text = objectInfo.Name;

            if (this.objectInfo is IIconObjectInfo) {
                this.IconImage.overrideSprite = (this.objectInfo as IIconObjectInfo).Icon;
            }


            this.UpdateDistanceText();
        }

        public virtual bool CheckObjectInfo() {
            return true;
        }

        private bool ValidObjectType(ObjectInfoType type) {
            foreach (var ot in this.ObjectType) {
                if (ot == type) {
                    return true;
                }
            }
            return false;
        }

        public virtual void Update() {
            updateTimer -= Time.deltaTime;
            if (updateTimer <= 0f) {
                updateTimer += UPDATE_INTERVAL;
                if (this.objectInfo == null) {
                    return;
                }
                this.UpdateDistanceText();
            }
        }

        protected void UpdateDistanceText() {
            //this.DistanceText.text = string.Format("Distnce: {0} n.u.", Mathf.RoundToInt(this.objectInfo.DistanceToPlayer));
        }
    }

    public enum ObjectInfoType { StandardCombatNpc, Asteroid, Event, PirateStation, Chest, MmoItem, MyPlayer, Planet, ProtactionStation, Activator }

    public interface IObjectInfo {

        ObjectInfoType InfoType { get; }

        string Name { get; }

        string Description { get; }
    }

    public interface IIconObjectInfo : IObjectInfo {
        Sprite Icon { get; }
    }

    public interface ICombatObjectInfo : IIconObjectInfo {

        int Level { get; }

        Race Race { get; }

        float CurrentHealth { get; }

        float MaxHealth { get; }

        float HitProb { get; }
    }

    public interface IAsteroidObjectInfo : IIconObjectInfo {
        List<Sprite> ContentSprites { get; }
    }

    public interface IEventObjectInfo : IIconObjectInfo {

    }
}
