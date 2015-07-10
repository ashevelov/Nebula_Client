using UnityEngine;
using System.Collections;
using Nebula.UI;
using Nebula.Mmo.Items;
using Nebula.Mmo.Items.Components;
using Client.UIC.Implementation;
using Client.UIC;

namespace Client.UIP.Implementation
{
    public class TargatInfoProcess : MonoBehaviour
    {

        public ITargetInfo uicPanel;

        private IObjectInfo objectInfo;

        void OnEnable()
        {
            StartCoroutine(UpdateInfo());
        }

        public void SetObject(IObjectInfo objectInfo)
        {
            this.objectInfo = objectInfo;
            this.gameObject.SetActive(this.objectInfo != null);

            if (this.objectInfo is IIconObjectInfo)
            {
                if (uicPanel == null)
                {
                    uicPanel = FindObjectOfType<TargetInfo>();
                }
                uicPanel.MaxHP = 50000;
                uicPanel.CurentHP = 50000;
                uicPanel.Icon = (this.objectInfo as IIconObjectInfo).Icon;
            }
        }

        private void CombatUpdate(ICombatObjectInfo info)
        {
            uicPanel.Name = info.Name;

            float maxHealth, currentHealth;
            TryGetHP(info, out currentHealth, out maxHealth);
            uicPanel.MaxHP = (int)maxHealth;            //(int)info.MaxHealth;
            uicPanel.CurentHP = (int)currentHealth;     //(int)info.CurrentHealth;
            uicPanel.Distance = info.DistanceTo(G.Game.Avatar);
            uicPanel.Level = info.Level;
        }

        private void TryGetHP(IObjectInfo info, out float ch, out float mh)
        {
            ch = mh = 50000;
            Item item = info as Item;
            if (item == null) { return; }
            var damagable = item.GetMmoComponent(Common.ComponentID.Damagable) as MmoDamagableComponent;
            if (damagable == null) { return; }
            ch = damagable.health;
            mh = damagable.maxHealth;
        }


        private void AsteroidUpdate(IAsteroidObjectInfo info)
        {
            uicPanel.Name = info.Name;
            uicPanel.Distance = info.DistanceTo(G.Game.Avatar);
        }

        IEnumerator UpdateInfo()
        {
            if (CheckCondition())
            {
                if (uicPanel == null)
                {
                    uicPanel = FindObjectOfType<TargetInfo>();
                    uicPanel.MaxHP = 50000;
                    uicPanel.CurentHP = 50000;
                }
                if (objectInfo is ICombatObjectInfo)
                {
                    CombatUpdate(objectInfo as ICombatObjectInfo);
                }
                else if (objectInfo is IAsteroidObjectInfo)
                {
                    AsteroidUpdate(objectInfo as IAsteroidObjectInfo);
                }
            }
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(UpdateInfo());
        }

        public bool CheckCondition()
        {
            if (objectInfo == null)
                return false;
            return true;
        }
    }
}
