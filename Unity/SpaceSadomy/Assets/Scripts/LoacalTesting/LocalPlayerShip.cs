/*
using UnityEngine;
using System.Collections;
using Game.Space;
using System.Collections.Generic;
using Nebula;

namespace LocalTest
{
    [ExecuteInEditMode]
    public class LocalPlayerShip : MonoBehaviour
    {
        public LocalPlayer player;
        private Vector3 _derection = Vector3.forward;
        public LocalBotShip _target;
        public Transform guns;
        private Vector3 _gunsDerection = Vector3.forward;
        private Rigidbody _rigidbody;

        
        public MyGUI ui_my;
        public MyGUI ui_target;

        public PlayerBonuses bonuses = new PlayerBonuses();


        private List<NotifLabel> massges = new List<NotifLabel>();
        private List<NotifLabel> removeMassges = new List<NotifLabel>();
        private string _color;
        private Rect _shipRect;
        private Vector3 _currentSize = new Vector2(100, 20);
        private GUIStyle text_style = new GUIStyle();


        public Skill[] skills;


        private void MassagesUpdate()
        {
            massges.ForEach((m) =>
            {
                m.style.normal.textColor = m.style.normal.textColor * (new Color(1, 1, 1, 1 - (Time.deltaTime * 0.5f)));
                m.pos -= new Vector2(0, 80 * Time.deltaTime);

                if (m.style.normal.textColor.a <= 0)
                {
                    removeMassges.Add(m);
                }
            });

            removeMassges.ForEach((rm) =>
            {
                massges.Remove(rm);
            });
            removeMassges.Clear();

            _shipRect = Utils.WorldPos2ScreenRect(transform.position, _currentSize);
        }

        public void AddMassage(string text, Color color)
        {
            NotifLabel temp = new NotifLabel();
            temp.Setup(text, color);
            temp.rect = new Rect(0, 0, 0, 0);
            temp.pos = new Vector2(Random.Range(-20, 20), Random.Range(-20, 20));
            massges.Add(temp);
        }

        private void DrawMassage()
        {
            massges.ForEach((m) =>
            {
                m.rect = _shipRect.addPos(0, 20).addPos(m.pos);
                GUI.color = m.color;
                m.Draw();
            });
        }


        void Start()
        {
            text_style.alignment = TextAnchor.MiddleCenter;
            text_style.wordWrap = true;
            text_style.normal.textColor = Color.white;
            skills[0].use = MissleEmi;
            skills[0].description = string.Format(" Ракета ЭМИ. \n Cтоимость: {0} энергии.\n Время заряда: 3 сек.\n Урон: {1} - {2}.\n Эффект: С шансом 10% вызывает короткое замыкание в системе противника, которое длится 5 секунд. \n\n Короткое замыкание - увеличивает время перезарядки всех орудий на 50%.", player.energy * 0.05f, 8 + (player.damage * 1.5f), 10 + (player.damage * 1.5f));
            skills[1].use = ReactiveRocket;
            skills[1].description = string.Format(" Реактивная ракета. \n Cтоимость: {0} энергии.\n Время заряда: мгновенно.\n Урон: {1} - {2}.\n Эффект: С шансом 20% может вызвать эффект мгновенной перезарядки. \n\n Мгновенная перезарядка - делает любой выстрел, для заряда которого требуется меньше трех секунд мгновенным. ", player.energy * 0.02f, 25 + (player.damage * 0.9f), 30 + (player.damage * 0.9f));
            skills[2].use = PulsedRocket;
            skills[2].description = string.Format(" Импульсная ракета. \n Cтоимость: {0} энергии.\n Время заряда: мгновенно.\n Урон: {1} - {2}.\n Эффект:Если цель находится под воздействием короткого замыкания наносимый урон увеличивается в 3 раза.", player.energy * 0.01f, 12 + (player.damage * 0.4f), 15 + (player.damage * 0.4f));
            skills[3].use = DevouringDrones;
            skills[3].description = string.Format(" Нано дроны.\n Cтоимость: {0} энергии.\n Время заряда: 3.5 сек.\n Урон: {1} - {2}.\n Эффект: Пожирающие дроны на 18 секунд на противника. \n\n Пожирающие дроны - наносят {3} урона раз в 2 сек.", player.energy * 0.05f, 12 + (player.damage * 2.0f), 14 + (player.damage * 2.0f), (12 + (player.damage * 2.2f)) / 8);

            player.curent_hp = player.hp;

            _rigidbody = GetComponent<Rigidbody>();

            Cursor.visible = !_battleMod;
        }

        void Update()
        {
            InputController();
            CheckFire();
            MassagesUpdate();
            player.curent_energy += Time.deltaTime * 0.2f;
            player.curent_energy = Mathf.Clamp(player.curent_energy, 0, player.energy);
            SkillUpdate();
            UpdateBuff();
            ui_my.speedProgress.Update();
            DpsMetrUpdate();
            CheckTarget();

        }

        void FixedUpdate()
        {
            Move();
        }

        void OnGUI()
        {            
            SkillsGUI();
            Utils.SaveMatrix();
            ui_my.Draw(player, bonuses);
            if (_target != null)
            {
                LocalPlayer temp_player = _target.player;
                ui_target.Draw(temp_player, _target.bonuses);
            }
            GUI.Label(new Rect((Screen.width / 2)/GUI.matrix.m00, (Screen.height / 2)/GUI.matrix.m00, 5, 5), "•".Size(20).Color("white"), text_style);

            Vector3 _gunsDer = guns.forward * 10000;
            _gunsDer += transform.position;
            GUI.Label(Utils.WorldPos2ScreenRect(_gunsDer, new Vector2(5, 5)), "O".Size(20).Color("white"), text_style);
            DrawMassage();
            DpsMetrGUI();
            Utils.RestoreMatrix();

        }

        

        public void SetDamage(float _damage, Color _color)
        {
            if (gameObject != null)
            {
                float damage = _damage - (_damage * player.resist);
                AddMassage((damage).ToString(), _color);
                player.curent_hp -= damage;

                if (player.curent_hp <= 0)
                {
                    Destroy(gameObject, 1);
                    GameObject obj = Instantiate(PrefabCache.Get("Prefabs/Effects/Detonator"), transform.position, Quaternion.identity) as GameObject;
                }
            }
        }

        public bool GetBattleMod()
        {
            return _battleMod;
        }

        private bool _battleMod = true;
        public void InputController()
        {
            //if (Input.GetKeyDown(KeyCode.CapsLock))
            //{
            //    _battleMod = !_battleMod;
            //    Screen.showCursor = !_battleMod;
            //}
            //if(Input.mou)
            if (_battleMod)
            {
                Ray rey = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                _gunsDerection = rey.direction;

                if (Input.GetMouseButtonDown(0))
                {
                    Fire();
                }
            }
            if (Input.GetMouseButton(1))
            {
                Ray rey = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2)); //Camera.main.ScreenPointToRay(Input.mousePosition);
                    _derection = rey.direction;
            }


            if (Input.GetKey(KeyCode.W))
            {
                //ui_my.speedProgress.progress = Mathf.Clamp(ui_my.speedProgress.progress + Time.deltaTime * 0.5f, 0, 1);
                player.curent_speed = player.speed * ((player.fastTravel) ? 10f : 1f);
                
            }
            else if (Input.GetKey(KeyCode.S))
            {
                //ui_my.speedProgress.progress = Mathf.Clamp(ui_my.speedProgress.progress - Time.deltaTime * 0.5f, 0, 1);
                player.curent_speed = - player.speed * ((player.fastTravel) ? 10f : 1f) *0.3f;
            }
            else
            {
                player.curent_speed -= player.curent_speed / 2 * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.eulerAngles -= new Vector3(0, 10*Time.deltaTime, 0);
                _derection = transform.forward;
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.eulerAngles += new Vector3(0, 10*Time.deltaTime, 0);
                _derection = transform.forward;
            }

            player.fastTravel = Input.GetKey(KeyCode.LeftShift);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                UseSkill(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                UseSkill(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                UseSkill(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                UseSkill(3);
            }


        }


        private void CheckTarget()
        {
            
            if (_target != null)
            {
                float angle = Vector3.Angle(guns.forward, _target.transform.position - guns.position);
                if (angle > 5)
                {
                    _target = null;
                }
            }
            RaycastHit hit;
            if (Physics.Raycast(guns.position, guns.forward, out hit, 100000))
            {
                if (hit.transform != transform)
                {
                    _target = hit.transform.GetComponent<LocalBotShip>();
                }
            }
        }

        private void Move()
        {
            transform.position += transform.forward * player.curent_speed * Time.fixedDeltaTime;
            _rigidbody.velocity += transform.forward * player.curent_speed * Time.fixedDeltaTime;
            _rigidbody.velocity -= _rigidbody.velocity * Time.fixedDeltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_derection), player.maneuverability);
            guns.rotation = Quaternion.RotateTowards(guns.rotation, Quaternion.LookRotation(_gunsDerection), player.tracking * Time.fixedDeltaTime);
        }

        private bool _fire = false;
        private float _cooldown = 100;

        private void Fire()
        {
           // if (_fire)
            {
                if (_cooldown >= (60 / player.attackSpeed))
                {
                    GameObject plasma;
                    if (_target != null)
                    {
                        plasma = Plasma.Init(_target.transform, _target != null, guns);
                    }
                    else
                    {
                        plasma = Plasma.Init(null, _target != null, guns);
                    }
                    plasma.transform.position = guns.position;
                    plasma.transform.rotation = guns.rotation;
                    if (_target != null)
                    {
                        float damage = player.damage;
                        if (Random.value < 0.1f)
                        {
                            _target.SetDamage(damage * 2, Color.yellow);
                        }
                        else
                        {
                            _target.SetDamage(damage, Color.yellow);
                        }

                    }

                    _cooldown = 0;
                }
            }
        }
        private void CheckFire()
        {
            int shorted = 1;
            if (bonuses.GetBonus(BonusType.shorted).getValue != 0)
            {
                shorted = 2;
            }
            _cooldown += Time.deltaTime / shorted;
            
        }


        private IEnumerator cor_Launch(float time, string path, Transform target, int count, bool isHitted, float damage, Color damagColor)
        {
            bool powerShield = false;

            for (int i = 0; i < count; i++)
            {
                if (_target != null)
                {
                    GameObject missile = Instantiate(PrefabCache.Get(path), transform.position, transform.rotation) as GameObject;

                    if (count > 1)
                    {
                        isHitted = Random.value < ComputeHitProb(player.optimalRange, player.extraRange, player.precision,
                                                                    _target.player.curent_speed, Vector3.Distance(transform.position, _target.transform.position));
                    }
                    if (isHitted)
                    {
                        missile.GetComponent<Missile>().SetTarget(target, false, 20, powerShield);
                        if (Random.value < 0.1f)
                        {
                            _target.SetDamage((damage / count)*2, damagColor);
                        }
                        else
                        {
                            _target.SetDamage(damage / count, damagColor);
                        }
                    }
                    else
                    {
                        if (target)
                        {
                            GameObject go = new GameObject();

                            go.transform.position = target.position + new Vector3(Random.Range(-250, 250),
                                                                                  Random.Range(-250, 250),
                                                                                  Random.Range(-250, 250));

                            missile.GetComponent<Missile>().SetTarget(go.transform, false, 20, powerShield);
                            _target.AddMassage("miss", damagColor);
                            Destroy(go, 8);
                        }
                    }
                }
                yield return new WaitForSeconds(time);
            }
        }



// ------------------------GUI-------------------------------

        [System.Serializable]
        public class MyGUI
        {
            public Rect group = new Rect(0, 0, 200, 100);
            public Rect name = new Rect(0, 0, 100, 20);
            public Progress hp = new Progress(0, 10, 100, 20);
            public Progress energy = new Progress(0, 20, 100, 20);
            public Rect buffs;
            public Texture2D[] buffIcons;
            public DragProgress speedProgress;
            public void Draw(LocalPlayer player, PlayerBonuses bonuses)
            {
                GUI.BeginGroup(group, GUI.skin.box);
                GUI.Label(name, player.name.Size(20));

                float _hp = player.curent_hp / player.hp;
                hp.progress = _hp;
                hp.Draw(("hp = " + (int)(_hp * 100) + "%").Color("black").Size(20));

                float _energy = player.curent_energy / player.energy;
                energy.progress = _energy;
                energy.Draw(("energy = " + (int)(_energy * 100) + "%").Color("black").Size(20));
                GUI.EndGroup();
                buffs = new Rect(group.x + 5, group.height + 5, 40, 40);
                foreach (KeyValuePair<BonusType, Bonus> bonus in bonuses.Bonuses)
                {
                    if (bonus.Value.getValue != 0)
                    {
                        GUI.DrawTexture(buffs, buffIcons[(int)bonus.Key]);
                        GUI.Label(buffs, ((int)bonus.Value.endTime).ToString().Size(30));
                        buffs.x += 45;
                    }
                }
                speedProgress.rect.y = (Screen.height / Utils.GameMatrix().m00) - speedProgress.rect.height;
                string speedProg = "speed: "+((int)(player.curent_speed)) + " / " + (int)player.speed;
                //player.curent_speed = player.speed * speedProgress.Draw(speedProg.Color("black").Size(20)) * ((player.fastTravel)? 10f : 1f);
            }
        }

        [System.Serializable]
        public class Progress
        {
            public float progress = 0.5f;
            public Progress(int x, int y, int width, int height)
            {
                this.rect = new Rect(x, y, width, height);
            }
            public Progress(Rect rct)
            {
                this.rect = rct;
            }
            public Texture2D full;
            public Texture2D empty;
            public Rect rect;

            public void Draw(string text)
            {
                GUI.DrawTexture(rect, empty);
                Rect fullRct = rect;
                fullRct.width = rect.width * progress;
                GUI.DrawTexture(fullRct, full);
                GUI.Label(rect, text);
            }
        }

        [System.Serializable]
        public class DragProgress
        {
            public float progress = 0.5f;
            public DragProgress(int x, int y, int width, int height)
            {
                this.rect = new Rect(x, y, width, height);
            }
            public DragProgress(Rect rct)
            {
                this.rect = rct;
            }
            public Texture2D full;
            public Texture2D empty;
            public Rect rect;
            private bool _dragStarted = false;

            public void Update()
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePosition = new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y);
                    mousePosition = mousePosition / Utils.GameMatrix().m00;
                    if (rect.Contains(mousePosition))
                    {
                        _dragStarted = true;
                    }
                }

                if (_dragStarted && Input.GetMouseButton(0))
                {
                    Vector2 mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                    mousePosition = mousePosition / Utils.GameMatrix().m00;
                    float dx = mousePosition.x - rect.x;
                    float xRation = dx / rect.width;
                    float newValue = Mathf.Clamp01(xRation);
                    float oldValue = progress;
                    progress = newValue;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (_dragStarted)
                        _dragStarted = false;
                }
            }

            public float Draw(string text)
            {
                GUI.DrawTexture(rect, empty);
                Rect fullRct = rect;
                fullRct.width = rect.width * progress;
                GUI.DrawTexture(fullRct, full);
                GUI.Label(rect, text);
                return progress;
            }
        }

//-----------------------/GUI--------------------------



        private float ComputeHitProb(float optimal, float extraRange, float precision, float targetSpeed, float targetDistance)
        {
            float num = 1f;

            float dist = Mathf.Abs(optimal - targetDistance);
            dist = extraRange - dist;
            dist = (dist < 0) ? 0 : dist;
            dist = extraRange - dist;
            dist = (dist < extraRange) ? dist : extraRange;
            num = 1 - (dist / extraRange);
            float prec = targetSpeed - precision;
            prec = (prec < 0) ? 0 : prec;
            prec = (prec < precision) ? prec : precision;
            prec = 1 - (prec / precision);
            num += prec;
            num = num - 0.5f;
            num = (num > 1f) ? 1f : num;

            return num;
        }
//----------------------Skills------------------------

        private float _castTime = 0;
        private float _endCast = 0;
        private bool _cast = false;
        private System.Action _skillAction;
        private float _skillTime = 0;
        public Progress castProgress;

        [System.Serializable]
        public class Skill
        {
            public Texture2D icon;
            [HideInInspector]
            public float curentColldown;
            public float cooldown = 10;
            public float energy = 5;
            public float minDamag = 10;
            public float maxDamag = 15;
            public float damageFactor = 0.9f;
            public float chance = 0.1f;
            public float twoMinDamag = 12;
            public float twoDamageFactor = 2.2f;
            public string description;
            public System.Action use;
            public Progress cooldownProgress = new Progress(0, 45, 100, 20);
        }

        public void SkillUpdate()
        {
            _skillTime += Time.deltaTime;
            int shorted = 1;
            if (bonuses.GetBonus(BonusType.shorted).getValue != 0)
            {
                shorted = 2;
            }
            if (_cast)
            {
                _castTime += Time.deltaTime / shorted;
                if (_skillAction != null)
                {
                    _skillAction();
                }
            }
            for (int i = 0; i < skills.Length; i++)
            {
                skills[i].curentColldown = Mathf.Clamp(skills[i].curentColldown - (Time.deltaTime / shorted), 0, skills[i].cooldown);
            }
        }


        private void SkillsGUI()
        {
            Rect rct = new Rect((Screen.width - (80 * skills.Length ))/ 2, Screen.height - 75, 75, 75);
            for (int i = 0; i < skills.Length; i++)
            {
                GUI.enabled = player.curent_energy >= skills[i].energy && !_cast && skills[i].curentColldown == 0;
                if (GUI.Button(rct, skills[i].icon))
                {
                    UseSkill(i);
                }
                GUI.enabled = true;

                skills[i].cooldownProgress.rect.x = rct.x;
                skills[i].cooldownProgress.rect.y = rct.y - 10;
                float progress = 1 - (skills[i].curentColldown / skills[i].cooldown);
                skills[i].cooldownProgress.progress = progress;
                skills[i].cooldownProgress.Draw(((int)(progress*100) + "%").Size(7).Color("black"));

                Rect descRect = rct.addPos(-180, -220);
                descRect.width = 360;
                descRect.height = 200;
                Vector2 mousePosition = new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y);
                if (rct.Contains(mousePosition))
                {
                    GUI.Box(descRect, GUIContent.none);
                    GUI.Label(descRect, skills[i].description, text_style);
                }

                rct.x += 80;
            }
            if(_cast)
            {
                castProgress.rect = new Rect((Screen.width - 200) / 2, Screen.height - 140, 200, 20);
                float progress = _castTime / _endCast;
                castProgress.progress = progress;
                castProgress.Draw(((int)(progress * 100) + "%").Size(14).Color("black"));
            }
        }

        private void UseSkill(int i)
        {
            {
                if (player.curent_energy >= skills[i].energy && !_cast)
                    if (skills[i].use != null && !_cast)
                    {
                        if (skills[i].curentColldown == 0)
                        {
                            for (int j = 0; j < skills.Length; j++)
                            {
                                skills[j].curentColldown += 1.5f;
                            }
                            player.curent_energy -= skills[i].energy;
                            skills[i].curentColldown = skills[i].cooldown;
                            skills[i].use();
                        }
                    }
            }
        }

        private float devouringDronesTime = 0;
        public void UpdateBuff()
        {
            if (bonuses.GetBonus(BonusType.devouringDrones).getValue != 0)
            {
                if (devouringDronesTime > 2f)
                {
                    devouringDronesTime = 0;
                    SetDamage(bonuses.GetBonus(BonusType.devouringDrones).getValue / 8, Color.red);
                }
                devouringDronesTime += Time.deltaTime;
            }

            foreach (KeyValuePair<BonusType, Bonus> bonus in bonuses.Bonuses)
            {
                if (bonus.Value.getValue != 0)
                {
                    bonus.Value.endTime = Mathf.Clamp(bonus.Value.endTime - Time.deltaTime, 0, 1000);
                }
            }
        }

        
        public void MissleEmi()
        {
            Skill skill = skills[0];
            _cast = true;

            if (snstantRecharge)
            {
                _endCast = 0;
                snstantRecharge = false;
            }
            else
            {
                _endCast = 3f;
            }
            _fire = true;
            _skillAction = () =>
                {
                    if (_castTime >= _endCast)
                    {
                        _cast = false;
                        _castTime = 0;
                        if (_target != null)
                        {
                            bool isHitted = Random.value < ComputeHitProb(player.optimalRange, player.extraRange, player.precision,
                                                                _target.player.curent_speed, Vector3.Distance(transform.position, _target.transform.position));
                            if (isHitted)
                            {
                                if (Random.value < skill.chance)
                                {
                                    float endTime = _skillTime + 5f;
                                    _target.bonuses.ReplaceBuff(BonusType.shorted, "sk001", new Buff(1, () =>
                                            {
                                                return (_skillTime <= endTime);
                                            }));
                                    _target.bonuses.GetBonus(BonusType.shorted).endTime = 5;
                                }
                            }

                            float damage = Random.Range(skill.minDamag, skill.maxDamag) + (player.damage * skill.damageFactor);
                            StartCoroutine(cor_Launch(0.2f, "Prefabs/Items/Weapons/Missiles/Torpedo", _target.transform, 1, isHitted, damage, Color.yellow));

                        }
                    }
                };
        }

        private bool snstantRecharge = false;

        public void ReactiveRocket()
        {
            Skill skill = skills[1];
            _cast = true;
            _endCast = 0;
            _fire = true;
            _skillAction = () =>
            {
                if (_castTime >= _endCast)
                {
                    _cast = false;
                    _castTime = 0;
                    if (_target != null)
                    {
                        if (Random.value < skill.chance)
                        {
                            snstantRecharge = true;
                            bonuses.ReplaceBuff(BonusType.snstantRecharge, "sk002", new Buff(1, () =>
                            {
                                return snstantRecharge;
                            }));
                        }

                        float damage = Random.Range(skill.minDamag, skill.maxDamag) + (player.damage * skill.damageFactor);
                        bool isHitted = Random.value < ComputeHitProb(player.optimalRange, player.extraRange, player.precision,
                                                                _target.player.curent_speed, Vector3.Distance(transform.position, _target.transform.position));

                        StartCoroutine(cor_Launch(0.2f, "Prefabs/Items/Weapons/Missiles/bomb", _target.transform, 1, isHitted, damage, Color.yellow));

                    }
                }
            };
        }

        public void PulsedRocket()
        {
            Skill skill = skills[2];
            _cast = true;

            _endCast = 0;

            _fire = true;
            _skillAction = () =>
                {
                    if (_castTime >= _endCast)
                    {
                        _cast = false;
                        _castTime = 0;
                        if (_target != null)
                        {

                            float damage = Random.Range(skill.minDamag, skill.maxDamag) + (player.damage * skill.damageFactor);
                            if(_target.bonuses.GetBonus(BonusType.shorted).getValue != 0)
                            {
                                damage = damage*3;
                            }
                            bool isHitted = Random.value < ComputeHitProb(player.optimalRange, player.extraRange, player.precision,
                                                                _target.player.curent_speed, Vector3.Distance(transform.position, _target.transform.position));

                            StartCoroutine(cor_Launch(0.2f, "Prefabs/Items/Weapons/Missiles/Missile", _target.transform, 1, isHitted, damage, Color.yellow));

                        }
                    }
                };
        }

        public void DevouringDrones()
        {
            Skill skill = skills[3];
            _cast = true;

            if (snstantRecharge)
            {
                _endCast = 0;
                snstantRecharge = false;
            }
            else
            {
                _endCast = 3.5f;
            }
            _fire = true;
            _skillAction = () =>
            {
                if (_castTime >= _endCast)
                {
                    _cast = false;
                    _castTime = 0;
                    if (_target != null)
                    {
                        float endTime = _skillTime + 18f;
                        bool isHitted = Random.value < ComputeHitProb(player.optimalRange, player.extraRange, player.precision,
                                                                _target.player.curent_speed, Vector3.Distance(transform.position, _target.transform.position));
                        if (isHitted)
                        {
                            if (Random.value < skill.chance)
                            {
                                _target.bonuses.ReplaceBuff(BonusType.devouringDrones, "sk004", new Buff(skill.twoMinDamag + (player.damage * skill.twoDamageFactor), () =>
                                {
                                    return (_skillTime <= endTime);
                                }));
                                _target.bonuses.GetBonus(BonusType.devouringDrones).endTime = 18;
                            }
                        }


                        float damage = Random.Range(skill.minDamag, skill.maxDamag) + (player.damage * skill.damageFactor);
                        StartCoroutine(cor_Launch(0.2f, "Prefabs/Items/Weapons/Missiles/bombRed", _target.transform, 1, isHitted, damage, Color.yellow));

                    }
                }
            };
        }
//----------------------/Skills-----------------------

//-------------dpsMetr---------------


        private float _dpsMetr_Value;
        private float _dpsMetr_Damage;
        private float _dpsMetr_Time;
        private bool _dpsMetrStart = false;
        public float updateDpsTime = 20;

        public void DpsMetrAddDamage(float damage)
        {
            _dpsMetrStart = true;
            _dpsMetr_Damage += damage;
        }
        private void DpsMetrUpdate()
        {
            if (_dpsMetrStart)
            {
                _dpsMetr_Time += Time.deltaTime;
                if (_dpsMetr_Time > updateDpsTime)
                {
                    _dpsMetrStart = false;
                    _dpsMetr_Value = _dpsMetr_Damage / _dpsMetr_Time;
                    _dpsMetr_Time = 0;
                    _dpsMetr_Damage = 0;
                }
            }
        }
        private void DpsMetrGUI()
        {
            Rect rct = new Rect((Screen.width/Utils.GameMatrix().m00)-200, 0, 200,50);
            GUI.Box(rct, ("dps: " + _dpsMetr_Value.ToString()).Size(20).Color("white"));
        }


//-------------/dpsMetr--------------
    }
}
*/
