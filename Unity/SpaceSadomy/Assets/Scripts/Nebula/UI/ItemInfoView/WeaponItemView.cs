using Game.Space;

namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using Nebula.Client.Inventory.Objects;
    using UnityEngine.UI;
    using Common;
    using UButton = UnityEngine.UI.Button;
    using Nebula.Resources;

    public class WeaponItemView : BaseItemView {

        public Text NameText;
        public Text LevelText;
        public Text WorkshopText;

        public Text DescriptionText;
        public Text LightAttackDamageText;
        public Text LightAttackCooldownText;
        public Text LightAttackCritDamageText;
        public Text HeavyAttackDamageText;
        public Text HeavyAttackCooldownText;
        public Text HeavyAttackCritDamageText;
        public Text HeavyAttackEnergyText;
        public Text CriticalChanceText;
        public Text OptimalDistanceText;
        public Text RangeText;

        public UButton WorkshopHelp;
        public UButton LightAttackDamageHelp;
        public UButton LightAttackCooldownHelp;
        public UButton LightAttackCritDamageHelp;
        public UButton HeavyAttackDamageHelp;
        public UButton HeavyAttackCooldownHelp;
        public UButton HeavyAttackCritDamageHelp;
        public UButton HeavyAttackEnergyHelp;
        public UButton CriticalChanceHelp;
        public UButton OptimalDistanceHelp;
        public UButton RangeHelp;


        public override void SetObject(ItemInfoView.ItemContentData contentData) {

            WeaponInventoryObjectInfo weapon = contentData.Data as WeaponInventoryObjectInfo;

            var weaponTemplate = DataResources.Instance.Weapon(weapon.Template);
            NameText.text = StringCache.Get(weaponTemplate.Name).Color(Utils.GetColor(weapon.Color));

            string levelColor = "green";
            //if (weapon.Level > G.Game.PlayerInfo.Level) {
            //    levelColor = "red";
            //}
            LevelText.text = weapon.Level.ToString().Color(levelColor);

            string workshopName = StringCache.Workshop(weaponTemplate.Workshop);
            string raceName = StringCache.Race(DataResources.Instance.ResRaces().RaceForWorkshop(weaponTemplate.Workshop));

            string fullName = workshopName + "(" + raceName + ")";
            if(GameData.instance.playerInfo.Workshop != weaponTemplate.Workshop) {
                WorkshopText.color = Color.red;
            } else {
                WorkshopText.color = Color.white;
            }
            WorkshopText.text = fullName;

            this.DescriptionText.text = StringCache.Get(weaponTemplate.Description);
            //this.LightAttackDamageText.text = StringCache.Get("LAD_FMT").f(Mathf.RoundToInt(weapon.LightDamage));
            //this.LightAttackCooldownText.text = StringCache.Get("LAC_FMT").f(weapon.LightCooldown);
            //this.LightAttackCritDamageText.text = StringCache.Get("LACD_FMT").f(Mathf.RoundToInt(weapon.LightCritDamage));

            //this.HeavyAttackDamageText.text = StringCache.Get("HAD_FMT").f(Mathf.RoundToInt(weapon.HeavyDamage));
            //this.HeavyAttackCooldownText.text = StringCache.Get("HAC_FMT").f(weapon.HeavyCooldown);
            //this.HeavyAttackCritDamageText.text = StringCache.Get("HACD_FMT").f(weapon.HeavyCritDamage);
            //this.HeavyAttackEnergyText.text = StringCache.Get("HAE_FMT").f(Mathf.RoundToInt(weapon.HeavyEnergy));

            //this.CriticalChanceText.text = StringCache.Get("CC_FMT").f(Mathf.RoundToInt(weapon.CritChance * 100));
            this.OptimalDistanceText.text = StringCache.Get("OD_FMT").f(Mathf.RoundToInt(weapon.OptimalDistance));
            //this.RangeText.text = StringCache.Get("R_FMT").f(Mathf.RoundToInt(weapon.Range));

            this.WorkshopHelp.onClick.RemoveAllListeners();
            this.LightAttackDamageHelp.onClick.RemoveAllListeners();
            this.LightAttackCooldownHelp.onClick.RemoveAllListeners();
            this.LightAttackCritDamageHelp.onClick.RemoveAllListeners();
            this.HeavyAttackDamageHelp.onClick.RemoveAllListeners();
            this.HeavyAttackCooldownHelp.onClick.RemoveAllListeners();
            this.HeavyAttackCritDamageHelp.onClick.RemoveAllListeners();
            this.HeavyAttackEnergyHelp.onClick.RemoveAllListeners();
            this.CriticalChanceHelp.onClick.RemoveAllListeners();
            this.OptimalDistanceHelp.onClick.RemoveAllListeners();
            this.RangeHelp.onClick.RemoveAllListeners();

            this.WorkshopHelp.onClick.AddListener(() => {
                MainCanvas.Get.Show(CanvasPanelType.TooltipView, new TooltipView.TooltipData { TitleId = string.Empty, ContentId = "WORKSHOP_HLP" });
            });
            this.LightAttackDamageHelp.onClick.AddListener(() => {
                MainCanvas.Get.Show(CanvasPanelType.TooltipView, new TooltipView.TooltipData { TitleId = string.Empty, ContentId = "LAD_HLP" });
            });
            this.LightAttackCooldownHelp.onClick.AddListener(() => {
                MainCanvas.Get.Show(CanvasPanelType.TooltipView, new TooltipView.TooltipData { TitleId = string.Empty, ContentId = "LAC_HLP" });
            });
            this.LightAttackCritDamageHelp.onClick.AddListener(() => {
                MainCanvas.Get.Show(CanvasPanelType.TooltipView, new TooltipView.TooltipData { TitleId = string.Empty, ContentId = "LACD_HLP" });
            });
            this.HeavyAttackDamageHelp.onClick.AddListener(() => {
                MainCanvas.Get.Show(CanvasPanelType.TooltipView, new TooltipView.TooltipData { TitleId = string.Empty, ContentId = "HAD_HLP" });
            });
            this.HeavyAttackCooldownHelp.onClick.AddListener(() => {
                MainCanvas.Get.Show(CanvasPanelType.TooltipView, new TooltipView.TooltipData { TitleId = string.Empty, ContentId = "HAC_HLP" });
            });
            this.HeavyAttackCritDamageHelp.onClick.AddListener(() => {
                MainCanvas.Get.Show(CanvasPanelType.TooltipView, new TooltipView.TooltipData { TitleId = string.Empty, ContentId = "HACD_HLP" });
            });
            this.HeavyAttackEnergyHelp.onClick.AddListener(() => {
                MainCanvas.Get.Show(CanvasPanelType.TooltipView, new TooltipView.TooltipData { TitleId = string.Empty, ContentId = "HAE_HLP" });
            });
            this.CriticalChanceHelp.onClick.AddListener(() => {
                MainCanvas.Get.Show(CanvasPanelType.TooltipView, new TooltipView.TooltipData { TitleId = string.Empty, ContentId = "CC_HLP" });
            });
            this.OptimalDistanceHelp.onClick.AddListener(() => {
                MainCanvas.Get.Show(CanvasPanelType.TooltipView, new TooltipView.TooltipData { TitleId = string.Empty, ContentId = "OD_HLP" });
            });
            this.RangeHelp.onClick.AddListener(() => {
                MainCanvas.Get.Show(CanvasPanelType.TooltipView, new TooltipView.TooltipData { TitleId = string.Empty, ContentId = "R_HLP" });
            });
        }
    }
}
