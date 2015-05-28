using Nebula.Client;

namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using Common;
    using Nebula;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using Nebula.Client.Res;

    public class ModuleItemView : BaseItemView {

        public Text NameText;
        public Text LevelText;
        public Text WorkshopText;
        public Text DescriptionText;
        public Text ModuleTypeText;
        public Text WeaponSlotsText;
        public Text HealthText;
        public Text SpeedText;
        public Text ResistText;
        public Text InventorySlotsCountText;
        public Text DamageBonusText;
        public Text CooldownBonusText;
        public Text OptimalDistanceBonusText;
        public Text CritChanceText;
        public Text CritDamageText;
        public Text EnergyText;
        public Text EnergyRestorationText;

        public Transform SkillBlockParent;
        public Text SkillNameText;
        public Text SkillDescriptionText;
        public Image SkillIconImage;

        public Transform SetBlockParent;
        public Text SetNameText;
        public Text BonusX2Text;
        public Text BonusX4Text;
        public Text BonusX5Text;
        public Image BonusSkillX5IconImage;


        public override void SetObject(ItemInfoView.ItemContentData contentData) {
            ClientShipModule module = contentData.Data as ClientShipModule;

            var moduleData = DataResources.Instance.ModuleData(module.templateId);

            if(moduleData == null ) {
                Debug.LogError("Not founded module data {0}".f(module.templateId));
                return;
            }

            this.NameText.text = StringCache.Get(moduleData.NameId);
            if(module.color != ObjectColor.white ) {
                this.NameText.color = Utils.GetColor(module.color);
            } 

            this.LevelText.text = module.level.ToString();
            if(G.Game.PlayerInfo.Level < module.level) {
                this.LevelText.color = Color.red;
            } else {
                this.LevelText.color = Color.green;
            }

            string workshopName = StringCache.Workshop(module.workshop);
            string raceName = StringCache.Race(DataResources.Instance.ResRaces().RaceForWorkshop(module.workshop));
            string fullName = "{0}({1})".f(workshopName, raceName);
            if( G.Game.PlayerInfo.Workshop != module.workshop) {
                this.WorkshopText.color = Color.red;
            } else {
                this.WorkshopText.color = Color.white;
            }
            this.WorkshopText.text = fullName;
            this.DescriptionText.text = StringCache.Get(moduleData.DescriptionId);
            this.ModuleTypeText.text = StringCache.ModuleType(module.type);
            this.WeaponSlotsText.text = StringCache.Get("WSC_FMT").f(module.weaponSlotsCount);
            this.HealthText.text = StringCache.Get("HP_FMT").f(module.hp);
            this.SpeedText.text = StringCache.Get("SP_FMT").f(module.speed);
            this.ResistText.text = StringCache.Get("RES_FMT").f(module.resist);
            this.InventorySlotsCountText.text = StringCache.Get("HOLD_FMT").f(module.hold);
            this.DamageBonusText.text = StringCache.Get("DMGBON_FMT").f(module.damageBonus);
            this.CooldownBonusText.text = StringCache.Get("CDBON_FMT").f(module.cooldownBonus);
            this.OptimalDistanceBonusText.text = StringCache.Get("ODBON_FMT").f(module.distanceBonus);
            this.CritChanceText.text = StringCache.Get("CC_FMT").f(module.critChance);
            this.CritDamageText.text = StringCache.Get("CRDMG_FMT").f(module.critDamage);
            this.EnergyText.text = StringCache.Get("EN_FMT").f(module.energy);
            this.EnergyRestorationText.text = StringCache.Get("ENREST_FMT").f(module.energyRestoration);

            //follow skill and set block
            if(!module.HasSkill) {
                this.SkillBlockParent.gameObject.SetActive(false);
            } else {
                var skillData = DataResources.Instance.SkillData(module.skill);
                if(skillData == null ) {
                    Debug.LogError("Not found skill {0}".f(module.skill));
                    return;
                }
                this.SkillIconImage.overrideSprite = SpriteCache.SpriteSkill(module.skill);
                this.SkillNameText.text = StringCache.Get(skillData.name);
                this.SkillDescriptionText.text = StringCache.Get(skillData.description);
            }

            if (!module.IsBelongToSet) {
                this.SetBlockParent.gameObject.SetActive(false);
            } else {
                var setData = DataResources.Instance.SetData(module.set);
                if(setData == null ) {
                    Debug.LogError("Not found set {0}".f(module.set));
                    return;
                }

                List<SetBonusData> bonuses;
                //fill x2 bonus
                if (setData.TryGetBonuses(2, out bonuses)) {
                    if(bonuses.Count > 0 ) {
                        var bonus = bonuses[0];
                        this.BonusX2Text.text = StringCache.SetBonus(bonus.BonusType).f(bonus.GetValue<float>());
                    } else {
                        Debug.LogError("Not found bonus x2 for set {0}".f(module.set));
                        return;
                    }
                } else {
                    Debug.LogError("Not found bonuses x2 for set {0}".f(module.set));
                    return;
                }

                //fil x4 bonus
                if (setData.TryGetBonuses(4, out bonuses)) {
                    if (bonuses.Count > 0) {
                        var bonus = bonuses[0];
                        this.BonusX4Text.text = StringCache.SetBonus(bonus.BonusType).f(bonus.GetValue<float>());
                    } else {
                        Debug.LogError("Not found bonus x4 for set {0}".f(module.set));
                        return;
                    }
                } else {
                    Debug.LogError("Not found bonuses x4 for set {0}".f(module.set));
                    return;
                }

                //fill x5 skill bonus
                if(setData.TryGetBonuses(5, out bonuses)) {
                    if(bonuses.Count > 0 ) {
                        var bonus = bonuses[0];
                        if(bonus.BonusType != ModuleSetBonusType.skill) {
                            Debug.LogError("Set bonus x5 not skill bonus at set {0}".f(module.set));
                            return;
                        }
                        string bonusSkillId = bonus.GetValue<string>();
                        var skillData = DataResources.Instance.SkillData(bonusSkillId);
                        if(skillData == null ) {
                            Debug.LogError("Not found bonus skill {0} for set {1}".f(bonusSkillId, module.set));
                            return;
                        }

                        this.BonusSkillX5IconImage.overrideSprite = SpriteCache.SpriteSkill(bonusSkillId);
                        string desc = StringCache.Get(skillData.name) + System.Environment.NewLine + StringCache.Get(skillData.description);
                        this.BonusX5Text.text = desc;

                    } else {
                        Debug.LogError("Not found bonus x5 for set {0}".f(module.set));
                        return;
                    }
                }
            }
        }
    }
}
