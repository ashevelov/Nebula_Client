namespace Nebula.Mmo.Items.Components {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Common;

    public class MmoBonusesComponent : MmoBaseComponent {

        private Dictionary<BonusType, float> mBonuses = new Dictionary<BonusType, float>();

        public Dictionary<BonusType, float> bonuses {
            get {
                return mBonuses;
            }
        }

        public override void Update() {

            Hashtable bonusHash = null;
            item.TryGetProperty<Hashtable>((byte)PS.Bonuses, out bonusHash);
            if(bonusHash == null ) {
                return;
            }

            mBonuses.Clear();

            foreach(DictionaryEntry entry in bonusHash) {
                int bonusType = (int)entry.Key;
                float bonusValue = (float)entry.Value;
                mBonuses.Add((BonusType)(byte)bonusType, bonusValue);
            }

        }
    }
}
