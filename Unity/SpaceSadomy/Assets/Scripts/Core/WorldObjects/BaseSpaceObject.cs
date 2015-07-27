using Common;
using Nebula;
using Game.Space;
using Nebula.UI;
using System.Collections;
using UnityEngine;
using ServerClientCommon;
using Nebula.Mmo.Games;
using Nebula.Test;
using Nebula.Resources;
using Nebula.Mmo.Items;
using Nebula.Effects;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class BaseSpaceObject : MonoBehaviour, ICachedPosition
{
    //Item for this space object
    private Item item;
    //Game object
    private NetworkGame game;
    //Selection object
    private GameObject selectionObject;
    //Self transform cached for optimization
    protected Transform selfTransform;
    //Shield object
	private ShieldEffect mShield;
	public ShieldEffect GetShield(){ return mShield; }
	public void SetShield(ShieldEffect eff){mShield = eff;}
    //Screen view gui class for this object
    //protected ObjectScreenSelection screenView;

    private BonusEffectViewManager bonusEffectViewManager = new BonusEffectViewManager();

    private bool childrensActive = true;

	public string GetItemID(){return item.Id;}

    private string[] explosionPath = new string[]
    {
        "Prefabs/Effects/Explosion08"
    };

    private PrefabSubCache<string> prefabSubCache = new PrefabSubCache<string>();

    private string RandomExplosionPath()
    {
        return this.explosionPath[Random.Range(0, this.explosionPath.Length - 1)];
    }

    private GameObject RandomExplosionPrefab()
    {
        string explosionPath = RandomExplosionPath();
        return prefabSubCache.Prefab(explosionPath, explosionPath);
    }

    
    //Skill effects, which drived by this object
    private Dictionary<string, GameObject> skillEffects = new Dictionary<string, GameObject>();

    //Bonuses effects drived by this objects
    private Dictionary<BonusType, GameObject> bonusesEffects = new Dictionary<BonusType, GameObject>();
   
    #region Public Members
    /// <summary>
    /// Virtual start, overrides by derived classes
    /// </summary>
    public virtual void Start() {

        this.selfTransform = transform;
        this.childrensActive = true;

       /* BonusEffectMaker speedDebuffMaker = (parent) => {
			PlayerSkillList.UseBuff("000003f5".FromHex());
			return inst;
        };

        BonusEffectMaker damageDebuffMaker = (parent) => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Effects/DamageDebuff")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.transform.localPosition = Vector3.zero;
            return inst;
        };
*/
        BonusEffectMaker damageImmunityMaker = (parent) => {
			PlayerSkillList.UseSkill(null,item,"000003FB".FromHex());
			return null;
        };

        BonusEffectMaker speedBuffMaker = (parent) => {
			PlayerSkillList.UseSkill(null,item,"00000400".FromHex());
			return null;
		};
		
		/*   BonusEffectMaker damageBuffMaker = (parent) => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Effects/DamageBuff")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.transform.localPosition = Vector3.zero;
            return inst;
        };*/

      /*  bonusEffectViewManager.SetEffectMaker(BonusType.decrease_speed_on_cnt, speedDebuffMaker);
        bonusEffectViewManager.SetEffectMaker(BonusType.decrease_speed_on_pc, speedDebuffMaker);
        bonusEffectViewManager.SetEffectMaker(BonusType.decrease_damage_on_cnt, damageDebuffMaker);
        bonusEffectViewManager.SetEffectMaker(BonusType.decrease_damage_on_pc, damageDebuffMaker);*/
        bonusEffectViewManager.SetEffectMaker(BonusType.damage_immunity, damageImmunityMaker);
        bonusEffectViewManager.SetEffectMaker(BonusType.increase_speed_on_cnt, speedBuffMaker);
        bonusEffectViewManager.SetEffectMaker(BonusType.increase_speed_on_pc, speedBuffMaker);
      /*  bonusEffectViewManager.SetEffectMaker(BonusType.increase_damage_on_cnt, damageBuffMaker);
        bonusEffectViewManager.SetEffectMaker(BonusType.increase_damage_on_pc, damageBuffMaker);
*/
        //bonusEffectViewManager.SetEffectMaker(BonusType.decrease_speed_on_cnt, BonusEffectMaker)
        //bonusEffectViewManager.SetEffectMaker(BonusType.decrease_speed_on_cnt)
        /*
        #region BONUS EFFECTS
        this.bonusEffectViewManager.SetEffectMaker(BonusType.blockWeapon, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA010003")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.transform.localPosition = Vector3.zero;
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.blockSkill, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA010004")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.transform.localPosition = Vector3.zero;
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.increaseResist, parent => {
            Debug.Log("called effect maker for increaseResist");
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/INCR_RESIST")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.transform.localPosition = Vector3.zero;
            inst.transform.localRotation = Quaternion.identity;
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.resurrectHP_Points, parent => {
            Debug.Log("called effect maker for resurrectHP");
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/RESURRECT_HP")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.transform.localPosition = new Vector3(20, 0, 0);
            inst.transform.localRotation = Quaternion.identity;
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.resurrectHP_Percent_By_Shoot, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA010006")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.transform.localPosition = Vector3.zero;
            inst.transform.localRotation = Quaternion.identity;
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.decreseMaxSpeedOnArea, parent => {
            var skill = DataResources.Instance.SkillData("SA020001");
            float range = skill.inputs.GetValue<float>("range", 0.0f);

            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA020001_AREA")) as GameObject;
            inst.transform.position = parent.transform.position;
            inst.transform.localScale = Vector3.one * 2 * range;
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.decreaseMaxSpeed, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA020001")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.transform.localPosition = Vector3.zero;
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.increaseMaxSpeed, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA020002")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.transform.localPosition = Vector3.zero;
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.decreaseOptimalDistance, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA030001")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.transform.localPosition = Vector3.zero;
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.decreaseOptimalDistanceOnArea, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA030001_USE"), parent.transform.position, parent.transform.rotation) as GameObject;
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.increaseCooldown, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA030002_1_Increase_Cooldown")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.transform.localPosition = new Vector3(0, 0, -1.5f);
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.decreaseDamage, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA030002_2_Decrease_Damage")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.transform.localPosition = new Vector3(0, 0, 1.5f);
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.increasePrecision, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA030004_1_Increase_Precision")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.ResetTransform();
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.increaseDamage, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA030004_2_Increase_Damage")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.ResetTransform();
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.decreaseCooldownOnArea, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA030005_USE"), parent.transform.position, parent.transform.rotation) as GameObject;
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.decreaseCooldown, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA030005_Decrease_Cooldown"), parent.transform.position, parent.transform.rotation) as GameObject;
            inst.transform.parent = parent.transform;
            inst.ResetTransform();
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.additionalDamage2EnemiesOnAreaPerFire, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA030006_USE"), parent.transform.position, parent.transform.rotation) as GameObject;
            inst.transform.parent = parent.transform;
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.increaseCriticalChance, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA040002_Increase_Crit_Chance")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.ResetTransform();
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.decreaseCriticalChance, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/Decrease_Crit_Chance")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.ResetTransform();
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.increaseDamageOnPlayerAndAlliesOnArea_Persistent, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA040005_USE")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.ResetTransform();
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.increaseGlobalCooldown, parent => {
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/Increase_Global_Cooldown")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.ResetTransform();
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.decreaseResists, parent => {
            Debug.Log("called effect maker for increaseResist");
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/DECR_RESIST")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.ResetTransform();
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.increaseInputDamage, parent => {
            var inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/Increase_Input_Damage")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.ResetTransform();
            return inst;
        });
        this.bonusEffectViewManager.SetEffectMaker(BonusType.increaseMaxHP, parent => {
            Debug.Log("called effect maker for resurrectHP");
            GameObject inst = GameObject.Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/RESURRECT_HP")) as GameObject;
            inst.transform.parent = parent.transform;
            inst.transform.localPosition = new Vector3(20, 0, 0);
            inst.transform.localRotation = Quaternion.identity;
            return inst;
        }); 
        #endregion
        */
    }

    /// <summary>
    /// Initialize object after creation item, overrides derived classes
    /// </summary>
    public virtual void Initialize(NetworkGame game, Item item)
    {
        this.item = item;
        this.game = game;

        if (item.IsMine)
        {
			ShipModel shipModel = item.GetShipModel();
			
			GameObject effectInstance =
				(MonoBehaviour.Instantiate(PrefabCache.Get("Effects/SkillSpeedAlways"),shipModel.transform.position, shipModel.transform.rotation)) as GameObject;
			
			
			SpeedEffect speed = effectInstance.GetComponentInChildren<SpeedEffect>();
			speed.Init(item.View,true);
			speed.transform.parent = shipModel.gameObject.transform;
			speed.transform.Rotate(new Vector3(1,0,0),180.0f,UnityEngine.Space.Self);
        }
    }

    public string GetItemTypeName() {

        switch (this.Item.Type.toItemType()) {
            case ItemType.Asteroid:
                return ItemTypeName.ASTEROID;
            case ItemType.Avatar:
                return ItemTypeName.PLAYER;
            case ItemType.Chest:
                return ItemTypeName.MISC;
            case ItemType.Bot:
                {
                    NpcItem npc = this.Item as NpcItem;
                    if(npc == null) {
                        return ItemTypeName.UNKNOWN;
                    }
                    switch(npc.SubType) {
                        case BotItemSubType.Activator:
                            return ItemTypeName.ACTIVATOR;
                        case BotItemSubType.Drill:
                            return ItemTypeName.MISC;
                        case BotItemSubType.Misc:
                            return ItemTypeName.MISC;
                        case BotItemSubType.None:
                            return ItemTypeName.UNKNOWN;
                        case BotItemSubType.PirateStation:
                            return ItemTypeName.MISC;
                        case BotItemSubType.Planet:
                            return ItemTypeName.PLANET;
                        case BotItemSubType.StandardCombatNpc:
                            return ItemTypeName.BOT_ENEMY;
                            
                    }
                    return ItemTypeName.UNKNOWN;
                }
            case ItemType.Event: return ItemTypeName.EVENT;
        }
        return ItemTypeName.UNKNOWN;
    }

    /*
    /// <summary>
    /// Initialize UI after creation object
    /// </summary>
    public virtual void InitializeUI()
    {
        if (this.screenView == null)
            this.screenView = new ObjectScreenSelection();

        this.screenView.Initialize(this, this.item, "Textures/Icons/ring", true, Vector2.one * 50, () =>
        {
            if (this.item != null && (false == G.UI.MouseOnGUI()))
            {
                //Debug.Log("REQUEST TARGET CALLED");
                MmoEngine.Get.Game.Avatar.RequestTarget(this.item.Id, this.item.Type, true);
            }
        });
    }*/

    /// <summary>
    /// Update for current object, overrides derived classes
    /// </summary>
    public virtual void Update()
    {
        if (this.item.IsMine)
        {
            //print("called update on my player");
            this.bonusEffectViewManager.Update(this, GameData.instance.bonuses);
        }
        else
        {
            if (this.item is IBonusHolder)
            {
                this.bonusEffectViewManager.Update(this, (this.item as IBonusHolder).Bonuses);
            }
        }
    }

    /// <summary>
    /// Called when this object fire 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="fireProperties"></param>
    public virtual void Fire(BaseSpaceObject target, Hashtable fireProperties)
    {
        float damage = fireProperties.GetValue<float>((int)SPC.ActualDamage, 0.0f);
        bool hitted = fireProperties.GetValue<bool>((int)SPC.IsHitted, true);

        byte workshop = fireProperties.GetValue<byte>((int)SPC.Workshop, Workshop.DarthTribe.toByte());
        int skillID = fireProperties.Value<int>((int)SPC.Skill, -1);

		bool isCrit = fireProperties.Value<bool>((int)SPC.IsCritical, false);

		this.EmitAmmo(fireProperties, hitted, damage, workshop, skillID);
		AddDamageMassage(target, damage, hitted,false,isCrit);

        if(Item.IsMine) {
            DebugView.Get.SetShot(fireProperties);
        }
    }

    //called when item ship destroyed
    public void OnShipWasDestroyed()
    {
        if(this.item.IsMine)
        {
            if(game.CurrentStrategy == GameState.NebulaGameWorldEntered) {
                MouseOrbitRotateZoom.Get.StartGrayScale();
                if (MainCanvas.Get) {
                    MainCanvas.Get.Destroy(CanvasPanelType.ControlHUDView);
                    MainCanvas.Get.Destroy(CanvasPanelType.MenuHUDView);
                    MainCanvas.Get.Destroy(CanvasPanelType.ChatView);
                    MainCanvas.Get.Destroy(CanvasPanelType.TargetObjectView);
                    MainCanvas.Get.Destroy(CanvasPanelType.SelectedObjectContextMenuView);

                    MainCanvas.Get.ToggleView(CanvasPanelType.ShipDestroyView);
                }
            }
        }
        else
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        this.ExplodeShip();
        this.item.DestroyView();
    }

    private void ExplodeShip()
    {
        Instantiate(RandomExplosionPrefab(), transform.position, Quaternion.identity);
    }

    /// <summary>
    /// Overrides derived classes, called when object destroyed
    /// </summary>
    public abstract void OnDestroy();


    private GameObject uiInstance;

    public virtual void OnEnable() {
        if (MainCanvas.ObjectIconsEnabled()) {
            StartCoroutine(CorOnEnable());
        }
    }

    private IEnumerator CorOnEnable() {
        while (this.Item == null) {
            yield return new WaitForSeconds(.2f);
        }

        if(this.Item.GetType() == typeof(MyItem)) {
            Debug.LogFormat("<color=green>Is Mine property of MyItem: {0}</color>", this.Item.IsMine);
        }

        if (false == this.Item.IsMine) {
            var uiPrefab = MainCanvas.Get.GetMiscPrefab("Prefabs/UI/Screen_Object_View");
            uiInstance = Instantiate(uiPrefab);
            uiInstance.GetComponent<Nebula.UI.ScreenObjectView>().Setup(this);
        }
    }

    public GameObject UI () {
        return this.uiInstance;
    }
    /// <summary>
    /// Called when disable object, overrides derived classes
    /// </summary>
    /// 
    public virtual void OnDisable()
    {
        if (MainCanvas.ObjectIconsEnabled()) {
            if (uiInstance != null && (false == this.Item.IsMine)) {
                Destroy(uiInstance);
                uiInstance = null;
            }
        }
        this.bonusEffectViewManager.DeleteAll();
    }

    /// <summary>
    /// Create shiled on this object, make overrides by derived classes
    /// </summary>
   /* public virtual void CreateShield()
    {
        if (this.shield)
        {
            Destroy(this.shield);
            this.shield = null;
        }
        this.shield = Instantiate(PrefabCache.Get("Prefabs/Effects/Shields/Shield")) as GameObject;
        this.shield.transform.parent = transform;
        this.shield.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// Destroy shield on this space object, can overrides by derived classes
    /// </summary>
    public virtual void DestroyShield()
    {
        if (this.shield)
        {
            Destroy(this.shield);
            this.shield = null;
        }
    }
*/

    /// <summary>
    /// Use skill by this space object(yet not implemented)
    /// </summary>
    /// <param name="skillProperties"></param>
    public virtual void UseSkill(Hashtable skillProperties)
    {	
		Hashtable skillData         = skillProperties.GetValue<Hashtable>((int)SPC.Data, new Hashtable());

		int skillId                 = skillData.Value<int>((int)SPC.Id, -1);

		if (skillId != -1 && PlayerSkillList.GetSkillType(skillId) == PlayerSkillList.SkillType.Support)
		{
			Debug.LogFormat("UseSkill used2 = {0}", skillId.ToString("X8"));
			PlayerSkillList.UseSkill(skillProperties,item,skillId);
		}

       if(Item.IsMine) {
            DebugView.Get.SetSkill(skillProperties);
        }
		

        /*
        switch(skillId)
        {
            case "SA010001":
                {
                    GameObject inst = (GameObject)Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA010001"));
                    inst.transform.parent = transform;
                    inst.transform.localPosition = Vector3.zero;
                }
                break;
            case "SA010003":
                {
                    Item targetItem;
                    if (G.Game.TryGetItem(targetType, targetId, out targetItem))
                    {
                        if (targetItem.Component)
                        {
                            if (isOn)
                            {
                                GameObject inst = Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA010003_USE"), transform.position, transform.rotation) as GameObject;
                                var moveComponent = inst.GetComponent<MoveFromSourceToTargetBySpeed>();
                                moveComponent.Move(transform, targetItem.Component.transform);
                            }
                        }
                    }

                }
                break;
            case "SA010004":
                {
                    Item targetItem;
                    if(G.Game.TryGetItem(targetType, targetId, out targetItem))
                    {
                        if(targetItem.Component)
                        {
                            if(isOn)
                            {
                                GameObject inst = Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA010004_USE"), transform.position, transform.rotation) as GameObject;
                                var moveComponent = inst.GetComponent<MoveFromSourceToTargetBySpeed>();
                                moveComponent.Move(transform, targetItem.Component.transform);
                            }
                        }
                    }
                }
                break;
            case "SA010005":
                {
                    if(isOn)
                    {
                        GameObject inst = Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA010005_USE"), transform.position, transform.rotation) as GameObject;
                        inst.transform.parent = transform;
                    }
                }
                break;
            case "SA030002":
                {
                    if (isOn)
                    {
                        Item targetItem;
                        if (G.Game.TryGetItem(targetType, targetId, out targetItem))
                        {
                            if (targetItem.Component)
                            {
                                GameObject inst = Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA030002_USE")) as GameObject;
                                inst.GetComponent<SetLineRendererEndsToTargets>().SetStartAndEnd(transform, targetItem.Component.transform);
                            }
                        }
                    }
                }
                break;
            case "SA030003":
                {
                    if(isOn)
                    {
                        Item targetItem;
                        if(G.Game.TryGetItem(targetType, targetId, out targetItem))
                        {
                            if(targetItem.Component)
                            {
                                GameObject inst = Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA030003_USE"), targetItem.Component.transform.position, targetItem.Component.transform.rotation)
                                    as GameObject;

                            }
                        }
                    }
                }
                break;
            case "SA030004":
                {
                    if(isOn)
                    {
                        Item targetItem;
                        if (G.Game.TryGetItem(targetType, targetId, out targetItem))
                        {
                            if (targetItem.Component)
                            {
                                GameObject inst = Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA030004_USE"), targetItem.Component.transform.position, targetItem.Component.transform.rotation)
                                    as GameObject;
                                inst.GetComponent<MoveFromSourceToTargetBySpeed>().Move(transform, targetItem.Component.transform);
                            }
                        }
                    }
                }
                break;
            case "SA040001":
                {
                    if (isOn)
                    {
                        Item targetItem;
                        if (G.Game.TryGetItem(targetType, targetId, out targetItem))
                        {
                            if (targetItem.Component)
                            {
                                GameObject inst = Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA040001_USE")) as GameObject;
                                inst.GetComponent<SetLineRendererEndsToTargets>().SetStartAndEnd(transform, targetItem.Component.transform);
                            }
                        }
                    }
                }
                break;
            case "SA040003":
                {
                    if (isOn)
                    {
                        Item targetItem;
                        if (G.Game.TryGetItem(targetType, targetId, out targetItem))
                        {
                            if (targetItem.Component)
                            {
                                GameObject inst = Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA040001_USE")) as GameObject;
                                inst.GetComponent<SetLineRendererEndsToTargets>().SetStartAndEnd(transform, targetItem.Component.transform);
                            }
                        }
                    }
                }
                break;
            case "SA040004":
                {
                    if (isOn)
                    {
                        Item targetItem;
                        if (G.Game.TryGetItem(targetType, targetId, out targetItem))
                        {
                            if (targetItem.Component)
                            {
                                GameObject inst = Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA040004_USE"), targetItem.Component.transform.position, targetItem.Component.transform.rotation)
                                    as GameObject;
                                inst.GetComponent<MoveFromSourceToTargetBySpeed>().Move(transform, targetItem.Component.transform);
                            }
                        }
                    }
                }
                break;
            case "SA050001":
                {
                    if(isOn)
                    {
                        Item targetItem;
                        if(G.Game.TryGetItem(targetType, targetId, out targetItem))
                        {
                            if(targetItem.Component)
                            {
                                GameObject inst = Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA050001_USE")) as GameObject;
                                inst.GetComponent<SetLineRendererEndsToTargets>().SetStartAndEnd(transform, targetItem.Component.transform);
                            }
                        }
                    }
                }
                break;
            case "SA050003":
                {
                    if (isOn)
                    {
                        Item targetItem;
                        if (G.Game.TryGetItem(targetType, targetId, out targetItem))
                        {
                            if (targetItem.Component)
                            {
                                GameObject inst = Instantiate(PrefabCache.Get("Prefabs/Effects/Skills/SA050003_USE")) as GameObject;
                                inst.GetComponent<SetLineRendererEndsToTargets>().SetStartAndEnd(transform, targetItem.Component.transform);
                            }
                        }
                    }
                }
                break;
        }*/

    }

    public GameObject GetObject() {
        if (item != null && item.IsDestroyed == false) {
            return gameObject;
        } else {
            return null;
        }
    }

    public float Speed()
    {
        if (this.item is IDamagable)
            return ((IDamagable)this.item).GetSpeed();
        return 0;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Get game object
    /// </summary>
    public NetworkGame Game 
    { 
        get 
        { 
            return this.game; 
        } 
    }

    /// <summary>
    /// Get item for this game object
    /// </summary>
    public Item Item 
    { 
        get 
        { 
            return this.item; 
        } 
    }

   /* /// <summary>
    /// Currently exist or not power shield on this object
    /// </summary>
    public bool PowerFieldExist
    {
        get
        {
            return this.shield;
        }
    }
*/
    /// <summary>
    /// Get position
    /// </summary>
    public Vector3 Position
    {
        get { return transform.position; }
    }
    #endregion

    #region Protected Members
    /// <summary>
    /// Make emitting action. overrides derived classes if needed
    /// </summary>
	protected virtual void EmitAmmo(Hashtable skillProperties, bool isHitted, float damage, byte sourceWorkshop, int skillID = -1)
    {
		/*
        string targetID = skillProperties.Value<string>((int)SPC.Target, string.Empty);
        byte targetType = skillProperties.Value<byte>((int)SPC.TargetType, (byte)ItemType.Avatar);
        byte sourceType = skillProperties.Value<byte>((int)SPC.SourceType, (byte)ItemType.Avatar);
*/
		int skillId                 = skillProperties.Value<int>((int)SPC.Skill, -1);

		if (skillId == -1 || PlayerSkillList.GetSkillType(skillId) == PlayerSkillList.SkillType.Weapon)
		{
			Debug.LogFormat("EmminAmmo used2 = {0}", skillID.ToString("X8"));
			PlayerSkillList.UseSkill(skillProperties,item,skillId);
		}

    }
    #endregion

    #region Private Members

    /*
    /// <summary>
    /// Remove Skill effect
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="skillId"></param>
    /// <returns></returns>
    private IEnumerator RemoveEffect(float delay, string skillId)
    {
        yield return new WaitForSeconds(delay);
        if (this.skillEffects.ContainsKey(skillId))
        {
            GameObject obj = this.skillEffects[skillId];
            this.skillEffects.Remove(skillId);
            if (obj)
            {
                Destroy(obj);
            }
        }
    }*/

    /// <summary>
    /// Try found skill target space object in scene
    /// </summary>
    private bool TryGetSkillTarget(Hashtable properties, out BaseSpaceObject result)
    {
        result = null;
        string targetId = properties.GetValue<string>((int)SPC.Target, string.Empty);
        byte targetType = properties.GetValue<byte>((int)SPC.TargetType, (byte)0);
        Item targetItem;
        if (Game.TryGetItem(targetType, targetId, out targetItem))
        {
            if (targetItem.View && targetItem.Component)
            {
                result = targetItem.Component;
                return true;
            }
        }
        return false;
    }

    [System.Obsolete("Obsolete function, when skills have additional properties")]
    private Hashtable GetAdditionalSkillProperty(Hashtable properties)
    {
        return new Hashtable();
    }

    private void ShowWeaponDamageSkill(Hashtable skillProperties)
    {
        string targetId = skillProperties.GetValue<string>((int)SPC.Target, string.Empty);
        byte targetType = skillProperties.GetValue<byte>((int)SPC.TargetType, (byte)0);
        float damage = skillProperties.GetValue<float>((int)SPC.ActualDamage, 0.0f);

        Item targetItem;
        if (G.Game.TryGetItem(targetType, targetId, out targetItem))
        {

            if (ValidateTarget(targetItem))
            {

                if (Item.IsMine)
                {
                    //i use skill
                    EmitAmmo(skillProperties, true, damage, Workshop.DarthTribe.toByte());

                }
                else
                {
                    //other use skill
                    switch ((ItemType)Item.Type)
                    {
                        case ItemType.Avatar:
                            {
                                ForeignPlayerItem foreignItem = Item as ForeignPlayerItem;
								EmitAmmo(skillProperties, true, damage, Workshop.DarthTribe.toByte());
                            }
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Check target for skill
    /// </summary>
    private bool ValidateTarget(Item item)
    {

        if (item is IDamagable)
        {
            IDamagable d = item as IDamagable;
            if (false == d.IsDead() && (false == item.IsDestroyed) && item.Component && item.Component.transform)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Launch laser autoattack
    /// </summary>
    private IEnumerator cor_LaunchLaser(float duration, string path, Transform target, int count, bool isHitted)
    {
        GameObject laserLeft = (GameObject)Instantiate(PrefabCache.Get(path), transform.position + transform.TransformDirection(Vector3.left) * 2, transform.rotation);
        GameObject laserRight = (GameObject)Instantiate(PrefabCache.Get(path), transform.position + transform.TransformDirection(Vector3.right) * 2, transform.rotation);
        laserLeft.GetComponent<NLaser>().StartShot(transform, target);
        laserRight.GetComponent<NLaser>().StartShot(transform, target);
        yield break;
        /*
        GameObject laserInstance = (GameObject)Instantiate(PrefabCache.Get(path));
        laserInstance.transform.parent = transform;
        laserInstance.transform.localPosition = Vector3.zero;
        laserInstance.transform.localRotation = Quaternion.identity;


        var laserComponent = laserInstance.GetComponent<WSP_LaserBeamWS>();
        laserComponent.AssignNewTarget(target);
        laserComponent.FireLaser();
        yield return new WaitForSeconds(duration);
        laserComponent.StopLaserFire();
        Destroy(laserInstance);
        laserInstance = null;*/
	}
	/// <summary>
	/// Launch plasma autoattack
	/// </summary>
    private IEnumerator cor_LaunchPlasma(float time, string path, Transform target, int count, bool isHitted)
	{
		bool gunsFind = false;
        //Transform[] allChildren = GetComponentsInChildren<Transform>();
        //foreach(Transform child in allChildren)
        //{
        //    if (!child || !child.gameObject)
        //    {
        //        yield break;
        //    }
        //    if(child.gameObject.name == "GunSlot")
        //    {
        //        gunsFind = true;
        //        if (isHitted)
        //        {
        //            Plasma.Init(target, isHitted, child, path);
        //        }
        //        else
        //        {
        //            if (target)
        //            {
        //                GameObject go = new GameObject();
						
        //                go.transform.position = target.position + new Vector3(Random.Range(-2.5f, 2.5f),
        //                                                                      Random.Range(-2.5f, 2.5f),
        //                                                                      Random.Range(-2.5f, 2.5f));
        //                Plasma.Init(go.transform, isHitted, child, path);
        //                Destroy(go, 10);
        //            }
        //        }
        //        yield return new WaitForSeconds(Random.Range(0, time));
        //    }
        //}
		if(!gunsFind)
		{
			for (int i = 0; i < count; i++)
			{
				if (isHitted)
				{
                    Plasma.Init(target, isHitted, transform, path);
				}
				else
				{
					if (target)
					{
						GameObject go = new GameObject();
						
						go.transform.position = target.position + new Vector3(Random.Range(-2.5f, 2.5f),
						                                                      Random.Range(-2.5f, 2.5f),
						                                                      Random.Range(-2.5f, 2.5f));
                        Plasma.Init(go.transform, isHitted, transform, path);
						Destroy(go, 10);
					}
				}
				yield return new WaitForSeconds(Random.Range(0, time));
			}
		}
	}

    /// <summary>
    /// Launch missile 1 autoattack
    /// </summary>
    private IEnumerator cor_Launch(float time, string path, Transform target, int count, bool isHitted)
    {
        bool isTorpedo = false;

        for (int i = 0; i < count; i++)
        {
            GameObject missile = Instantiate(PrefabCache.Get(path), transform.position, transform.rotation) as GameObject;
            if (isHitted)
            {
                missile.GetComponent<Missile>().SetTarget(target, isTorpedo, 30, false);
            }
            else
            {
                if (target)
                {
                    GameObject go = new GameObject();

					go.transform.position = target.position + new Vector3(Random.Range(-2.5f, 2.5f),
					                                                      Random.Range(-2.5f, 2.5f),
					                                                      Random.Range(-2.5f, 2.5f));
                    missile.GetComponent<Missile>().SetTarget(go.transform, isTorpedo, 30, false);
                    Destroy(go, 8);
                }
            }
            yield return new WaitForSeconds(time);
        }
    }

    /// <summary>
    /// Launch missile 2 autoattack
    /// </summary>
    private IEnumerator cor_Launch2(string path, Transform target, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var missile = Instantiate(PrefabCache.Get(path), transform.position, transform.rotation) as GameObject;
            missile.GetComponent<Missile2>().Emit(this, target);
        }
        yield break;
    }

    /// <summary>
    /// WTF
    /// </summary>
    private IEnumerator cor_PulseLaser(float time, string path, Transform target, int count, bool isHitted)
    {
        for (int i = 0; i < count; i++)
        {

            GameObject laser_turel = new GameObject();
            laser_turel.transform.position = transform.position + new Vector3(Random.Range(-10, 10),
                                                                  Random.Range(-10, 10),
                                                                  Random.Range(-10, 10));
            laser_turel.transform.parent = transform;
            //missile.GetComponent<Missile>().SetTarget(go.transform, 20, powerShield);
            Destroy(laser_turel, 8);

            if (isHitted)
            {
                PulseLaser.Init(laser_turel.transform, target, false, isHitted);
            }
            else
            {
                GameObject go = new GameObject();
                go.transform.position = target.position + new Vector3(Random.Range(-250, 250),
                                                                      Random.Range(-250, 250),
                                                                      Random.Range(-250, 250));
                //missile.GetComponent<Missile>().SetTarget(go.transform, 20, powerShield);
                PulseLaser.Init(laser_turel.transform, go.transform, false, isHitted);
                Destroy(go, 8);
            }
            yield return new WaitForSeconds(time);
        }
    }

    /// <summary>
    /// Add damage gui text
    /// </summary>
	public IEnumerator AddDamageMassage(BaseSpaceObject targetComponent, float damage,bool isHitted, bool isHeal = false,bool isCrit = false)
    {

		string msg;
		Color _color;

	
		if (damage == 0 && isHitted)
		{
			msg = "RESIST";
			_color = new Color(1,1,0,1);
		}else
		{
			if (isHeal)
			{
				msg = "+"+damage.ToString();
				_color = new Color(0,1,0,1);
			}
			else
			{
				if (!isHitted)
				{
					msg = "MISS";
					_color = new Color(1,1,0,1);
				}else
				{
					msg = "-"+damage.ToString("0.0");
					_color = new Color(1,0,0,1);
				}
			}
		}


		SkillMessage.AddMessage(msg,targetComponent,_color,isCrit);
			
		return null;
           // yield return new WaitForSeconds(time);
        
            
    }
    #endregion

    /*
    #region OBSOLETE
    private IEnumerator cor_Jamp()
    {
        Vector3 endPos = transform.position + transform.forward * 10000;
        JumpGate.Init(transform.position, endPos);
        yield return new WaitForSeconds(0.4f);
        transform.position = endPos;
    }



    //applying heavy rocket skill effect to target, isHitted - hit to target occur or not 
    private void ApplyHeavyRocketSkillEffect(Transform target, bool isHitted)
    {
        Debug.Log("apply heavy rocket from this transform => target");
        if (target.gameObject != null && target.gameObject.activeSelf)
        {
            StartCoroutine(cor_Launch(0.2f, "Prefabs/Items/Weapons/Missiles/Torpedo", target, 1, isHitted, ShotType.Heavy));
        }

    }

    //apply acid charge skill effect to target, isHItted - hit to target occured or not
    private void ApplyAcidChargeSkillEffect(Transform target, bool isHitted)
    {
        Debug.Log("apply acid charge from this.transform => target");
        if (target.gameObject != null && target.gameObject.activeSelf)
        {
            StartCoroutine(cor_Launch(0.2f, "Prefabs/Items/Weapons/Missiles/bomb", target, 10, isHitted, ShotType.Heavy));
        }
    }

    //apply light rocket skill effect to target, isHitted - hit to target occured or not
    private void ApplyLightRocketSkillEffect(Transform target, bool isHitted)
    {
        Debug.Log("apply light rocket from this.transform => target");
        if (target.gameObject != null && target.gameObject.activeSelf)
        {
            StartCoroutine(cor_Launch(0.2f, "Prefabs/Items/Weapons/Missiles/Missile", target, 5, isHitted, ShotType.Heavy));
        }

    }

    //apply decrease precision effect to target
    private void ApplyDecreasePrecisionSkillEffect(Transform target)
    {
        Debug.Log("apply decrease precision from this.transform => target");
        GameObject go = Instantiate(PrefabCache.Get("Prefabs/Effects/SkillsEffects/SK0102")) as GameObject;
        go.transform.parent = target;
        go.transform.localPosition = Vector3.zero;
        Destroy(go, 10);

        GameObject sound = Instantiate(PrefabCache.Get("Prefabs/Effects/SkillsEffects/SK0102_UseSound")) as GameObject;
        sound.transform.parent = target;
        sound.transform.localPosition = Vector3.zero;
        Destroy(sound, 10);
    }

    //apply increase damage effect to target
    private void ApplyIncreaseDamageSkillEffect()
    {
        Debug.Log("apply increase damage skill effect");

        GameObject sound = Instantiate(PrefabCache.Get("Prefabs/Effects/SkillsEffects/SK0104_UseSound")) as GameObject;
        sound.transform.parent = transform;
        sound.transform.localPosition = Vector3.zero;
        Destroy(sound, 10);
    }

    //apply jum on 100 km skill effect
    private void ApplyJumpSkillEffect()
    {
        Debug.Log("apply jump skill effect");
        StartCoroutine(cor_Jamp());
    }
    #endregion
    */

    /// <summary>
    /// Как проверить что объект принадлежит ивенту или не принадлжежит
    /// </summary>
    public void Sample_CheckForEventedItem()
    {
        if(!Item.IsMine)
        {
            if(Item.Type.toItemType() == ItemType.Bot )
            {
                NpcItem npcItem = Item as NpcItem;
                if(npcItem.SubType == BotItemSubType.StandardCombatNpc )
                {
                    StandardNpcCombatItem combatItem = npcItem as StandardNpcCombatItem;
                    if (combatItem.EventInfo.FromEvent)
                        Debug.Log("this item connected to some event");
                    else
                        Debug.Log("this iitem without event");
                }
            }
        }
    }

    /// <summary>
    /// Как проверить текущую цель итема
    /// </summary>
    public void Sample_CheckForItemTarget()
    {
        if(!Item.IsMine)
        {
            if(Item.Type.toItemType().AnyFrom<ItemType>(ItemType.Bot, ItemType.Avatar))
            {
                ForeignItem foreignItem = Item as ForeignItem;

                if(foreignItem.TargetInfo.HasTarget )
                {
                    if(foreignItem.TargetInfo.TargetId == G.Game.Avatar.Id )
                    {
                        Debug.Log("I am a target for this item");
                    }
                    else
                    {
                        Debug.Log("Target for item some other item");
                    }
                }
                else
                {
                    Debug.Log("Currenty item don't has target");
                }
            }
        }
    }

    public void DeactivateChildrens()
    {
        foreach(Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
        this.childrensActive = false;
    }

    public void ActivateChildrens()
    {
        foreach(Transform t in transform)
        {
            t.gameObject.SetActive(true);
        }
        this.childrensActive = true;
    }

    public bool ChildrensActive
    {
        get { return this.childrensActive; }
    }
}

public interface ICachedPosition 
{
    Vector3 Position { get; }
    GameObject GetObject();
}