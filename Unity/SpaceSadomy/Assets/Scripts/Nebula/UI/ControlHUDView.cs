using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UButton = UnityEngine.UI.Button;
using USlider = UnityEngine.UI.Slider;
using UToggle = UnityEngine.UI.Toggle;
using Common;
using UnityEngine.EventSystems;
using Game.Space;
using Nebula.Client.Res;
using Nebula.Client;

namespace Nebula.UI {
    public class ControlHUDView : BaseView {

        private const float UPDATE_ATTACK_INTERVAL = .5f;
        private const float UPDATE_SKILL_INTERVAL = 1f;
        private const float UPDATE_MISC_INTERVAL = 0.2f;
        private const float UPDATE_TEXTS_INTERVAL = 1f;

        public UButton LightAttackButton;
        public UButton HeavyAttackButton;
        public Image LightAttackImage;
        public Image HeavyAttackImage;

        public UButton[] SkillButtons;
        public Image[] SkillImages;

        public Image RaceImage;
        public Image WorkshopImage;
        public Text NameLevelText;
        public Text RaceWorkshopText;
        public Text WorldPositionText;
        public Text HpEnText;
        public USlider HpSlider;
        public USlider EnSlider;
        public UToggle MoveToggle;
        public UButton AccelerateButton;

        private float attackTimer;
        private float skillTimer;
        private float miscTimer;
        private float textTimer;

        private StringSubCache<string> stringCache = new StringSubCache<string>();

        void Start() {
            this.attackTimer = UPDATE_ATTACK_INTERVAL;
            this.skillTimer = UPDATE_SKILL_INTERVAL;
            this.miscTimer = UPDATE_MISC_INTERVAL;
            this.textTimer = UPDATE_TEXTS_INTERVAL;

        }
        void Update() {
            this.attackTimer -= Time.deltaTime;
            if (this.attackTimer <= 0f) {
                this.attackTimer += UPDATE_ATTACK_INTERVAL;
                this.UpdateAttackButtons();
            }
            this.skillTimer -= Time.deltaTime;
            if (this.skillTimer <= 0f) {
                this.skillTimer += UPDATE_SKILL_INTERVAL;
                this.UpdateSkillViews();
            }
            this.miscTimer -= Time.deltaTime;
            if (this.miscTimer <= 0f) {
                this.miscTimer += UPDATE_MISC_INTERVAL;
                this.UpdateProgresses();
                //update texts
            }
            this.textTimer -= Time.deltaTime;
            if (this.textTimer <= 0f) {
                this.textTimer += UPDATE_TEXTS_INTERVAL;
                this.UpdatePlayerInfo();
            }
        }

        private void UpdateSkillViews() {
            for (int i = 0; i < this.SkillButtons.Length; i++) {
                var skill = G.GamePlayerSkill(i);
                if (skill.HasSkill && skill.Ready) {
                    this.SkillButtons[i].interactable = true;
                    this.SkillImages[i].fillAmount = 1.0f;
                } else if (skill.HasSkill && (!skill.Ready)) {
                    this.SkillButtons[i].interactable = false;
                    this.SkillImages[i].fillAmount = Mathf.Clamp01(skill.Progress());
                } else if (!skill.HasSkill) {
                    this.SkillButtons[i].interactable = false;
                    this.SkillImages[i].fillAmount = 0f;
                }
            }
        }

        private void UpdateAttackButtons() {
            if (G.GameShipWeaponLightShotTimer01() < 1f || G.GamePlayerShipWeaponShotEnergy(ShotType.Light) > G.GamePlayerShipEnergy()) {
                this.LightAttackButton.interactable = false;
                this.LightAttackImage.fillAmount = G.GameShipWeaponLightShotTimer01();
            } else {
                this.LightAttackButton.interactable = true;
                this.LightAttackImage.fillAmount = 1f;
            }

            if (G.GameShipWeaponHeavyShotTimer01() < 1f || G.GamePlayerShipWeaponShotEnergy(ShotType.Heavy) > G.GamePlayerShipEnergy()) {
                this.HeavyAttackButton.interactable = false;
                this.HeavyAttackImage.fillAmount = G.GameShipWeaponHeavyShotTimer01();
            } else {
                this.HeavyAttackButton.interactable = true;
                this.HeavyAttackImage.fillAmount = 1f;
            }
        }

        private void UpdateProgresses() {
            this.HpSlider.value = G.Game.Ship.Health01;
            this.EnSlider.value = G.Game.Ship.Energy01;
        }

        private void UpdatePlayerInfo() {
            if (PlayerInfo() == null) {
                return;
            }
            this.RaceImage.overrideSprite = SpriteCache.RaceSprite(PlayerInfo().Race);
            this.WorkshopImage.overrideSprite = SpriteCache.WorkshopSprite(PlayerInfo().Workshop);
            this.NameLevelText.text = string.Format("{0}, {1}", PlayerInfo().Name, PlayerInfo().Level.ToString().Color("orange"));
            this.RaceWorkshopText.text = string.Format("{0}, {1}", StringCache.Race(PlayerInfo().Race).Color(Utils.RaceColor(PlayerInfo().Race)), StringCache.Workshop(PlayerInfo().Workshop));

            string coordStr = string.Empty;
            if (G.PlayerComponent != null) {
                coordStr = G.PlayerComponent.transform.position.ToIntString();
            }
            this.WorldPositionText.text = string.Format("{0},Coord:{1}", stringCache.String(CurrentZone().DisplayName(), CurrentZone().DisplayName()).Color("orange"), coordStr);
            this.HpEnText.text = string.Format("HP:{0}/{1},EN:{2}/{3},EXP:{4}/{5}",
                Mathf.RoundToInt(G.Game.CombatStats.CurrentHP),
                Mathf.RoundToInt(G.Game.CombatStats.MaxHP),
                Mathf.RoundToInt(G.Game.CombatStats.CurrentEnergy),
                Mathf.RoundToInt(G.Game.CombatStats.MaxEnergy),
                Mathf.RoundToInt(PlayerInfo().Exp),
                DataResources.Instance.Leveling.ExpForLevel(PlayerInfo().Level + 1));
        }

        public void OnSkillButtonClick(int skillIndex) {
            NRPC.RequestUseSkill(skillIndex);
        }

        public void OnShotButtonClicked(bool isLight) {
            NRPC.RequestFire((isLight) ? ShotType.Light : ShotType.Heavy);
            if (isLight) {
                this.LightAttackButton.interactable = false;
            } else {
                this.HeavyAttackButton.interactable = false;
            }
        }

        public void OnMoveToggleValueChanged() {
            if (!ValidateState()) {
                return;
            }

            if (this.MoveToggle.isOn) {
                G.PlayerItem.RequestMoveDirection();
                G.PlayerItem.RequestLinearSpeed(G.Game.Ship.MaxLinearSpeed);
            } else {
                G.PlayerItem.RequestStop();
            }
        }

        public void OnAccelerateButtonDown(BaseEventData data) {
            if (!this.ValidateState()) {
                return;
            }
            NRPC.RequestShiftDown();
        }

        public void OnAccelerateButtonUp(BaseEventData data) {
            if (!this.ValidateState()) {
                return;
            }
            NRPC.RequestShiftUp();
        }

        private bool ValidateState() {
            if (G.PlayerItem == null) {
                return false;
            }
            if (G.Game.State != GameState.WorldEntered) {
                return false;
            }
            return true;
        }

        private ClientPlayerInfo PlayerInfo() {
            if (G.Game == null)
                return null;
            return G.Game.PlayerInfo;
        }

        private ResZoneInfo CurrentZone() {
            return DataResources.Instance.ZoneForId(G.Game.CurrentWorldId());
        }

        public void OnExpChanged(int oldExp, int newExp) { 
            
        }
    }
}