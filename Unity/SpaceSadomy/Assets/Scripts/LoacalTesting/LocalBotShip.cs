/*
using UnityEngine;
using System.Collections;
using Game.Space;
using System.Collections.Generic;
using Nebula;

namespace LocalTest
{
    public class LocalBotShip : MonoBehaviour
    {
        public LocalPlayer player;
        private Vector3 _derection = Vector3.forward;
        public LocalPlayerShip _target;
        private float _newDerectionTime = 3;
        private float _lockDistance = 8000;

        private List<NotifLabel> massges = new List<NotifLabel>();
        private List<NotifLabel> removeMassges = new List<NotifLabel>();
        private string _color;
        private Rect _shipRect;
        private Vector3 _currentSize = new Vector2(100, 20);


        public PlayerBonuses bonuses = new PlayerBonuses();
        private GUIStyle style_indicator = new GUIStyle();

        private LocalPlayerShip _realPlayer;
        private Rigidbody _rigidbody;


        public Skill[] skills;

        public bool armed = true;

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
            skills[0].use = MissleEmi;
            skills[1].use = ReactiveRocket;
            skills[2].use = PulsedRocket;
            skills[3].use = DevouringDrones;
            style_indicator.alignment = TextAnchor.MiddleCenter;
            _realPlayer = FindObjectOfType<LocalPlayerShip>();
            player.curent_hp = player.hp;

            _rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            BotController();
            //CheckFire();
            MassagesUpdate();
            player.curent_energy += Time.deltaTime * 0.2f;
            player.curent_energy = Mathf.Clamp(player.curent_energy, 0, player.energy);
            SkillUpdate();
            UpdateBuff();
        }

        void FixedUpdate()
        {
            Move();
        }

        void OnGUI()
        {
            Utils.SaveMatrix();
            int size = (armed) ? 20 : 25;
            string color = (armed) ? "red" : "white";
            GUI.Label(_shipRect, "+".Size(size).Color(color), style_indicator);
            if (_realPlayer != null)
            {
                if (_realPlayer._target == this)
                {
                    GUI.Label(_shipRect, "O".Size(size).Color("yellow"), style_indicator);
                }
            }
            GUI.Label(_shipRect.addPos(0, -40).addSize(new Vector2(0, 10)), player.name.Size(size).Color(color), style_indicator);
            DrawMassage();
            Utils.RestoreMatrix();
        }


        public void SetDamage(float _damage, Color _color)
        {
            _lockDistance = 50000;
            if (gameObject != null)
            {
                float damage = _damage - (_damage * player.resist);
                AddMassage((damage).ToString(), _color);
                player.curent_hp -= damage;
                _realPlayer.DpsMetrAddDamage(damage);

                if (player.curent_hp <= 0)
                {
                    Destroy(gameObject, 1f);
                    GameObject obj = Instantiate(PrefabCache.Get("Prefabs/Effects/Detonator"), transform.position, Quaternion.identity) as GameObject;
                }
            }
        }

        public void BotController()
        {
            if (armed)
            {
                _target = null;
                foreach (LocalPlayerShip p in GameObject.FindObjectsOfType<LocalPlayerShip>())
                {
                    if (Vector3.Distance(transform.position, p.transform.position) < _lockDistance)
                    {
                        _target = p;
                        _fire = true;
                    }
                }

                if (_target == null)
                {
                    if (_newDerectionTime <= 0)
                    {
                        _newDerectionTime = Random.Range(10, 30);
                        _derection = Random.insideUnitSphere * 10000;
                    }
                    _newDerectionTime -= Time.deltaTime;
                }
                else
                {
                    _derection = -(transform.position - _target.transform.position);
                }
                if (_target != null)
                {
                    for (int i = 0; i < skills.Length; i++)
                    {
                        UseSkill(i);
                    }
                }
            }
            else
            {
                if (_newDerectionTime <= 0)
                {
                    _newDerectionTime = Random.Range(10, 30);
                    _derection = Random.insideUnitSphere * 10000;
                }
                _newDerectionTime -= Time.deltaTime;
            }
        }

        private void Move()
        {
            _rigidbody.velocity += transform.forward * player.curent_speed * Time.fixedDeltaTime;
            _rigidbody.velocity -= _rigidbody.velocity / 2 * Time.fixedDeltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_derection), player.maneuverability);
        }

        private bool _fire = false;
        private float _cooldown = 100;
        private void CheckFire()
        {
            int shorted = 1;
            if (bonuses.GetBonus(BonusType.shorted).getValue != 0)
            {
                shorted = 2;
            }
            _cooldown += Time.deltaTime / shorted;
            if (_fire && _target != null)
            {
                if (_cooldown >= (60 / player.attackSpeed))
                {
                    StartCoroutine(cor_Launch(0.2f, "Prefabs/Items/Weapons/Missiles/Missile", _target.transform, 3, true, player.damage, Color.red));
                    _cooldown = 0;
                }
            }
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
                            _target.SetDamage((damage / count) * 2, damagColor);
                        }

                        _target.SetDamage(damage / count, damagColor);
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

        [System.Serializable]
        public class Skill
        {
            public Texture2D icon;
            [HideInInspector]
            public float curentColldown;
            public float cooldown = 10;
            public float energy = 5;
            public System.Action use;
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
                    SetDamage(bonuses.GetBonus(BonusType.devouringDrones).getValue / 8, Color.yellow);
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
                            if (Random.value < 0.1f)
                            {
                                float endTime = _skillTime + 5f;
                                _target.bonuses.ReplaceBuff(BonusType.shorted, "sk001", new Buff(1, () =>
                                {
                                    return (_skillTime <= endTime);
                                }));
                                _target.bonuses.GetBonus(BonusType.shorted).endTime = 5;
                            }
                        }

                        float damage = Random.Range(8.0f, 10.0f) + (player.damage * 1.5f);
                        StartCoroutine(cor_Launch(0.2f, "Prefabs/Items/Weapons/Missiles/Torpedo", _target.transform, 1, isHitted, damage, Color.red));

                    }
                }
            };
        }

        private bool snstantRecharge = false;

        public void ReactiveRocket()
        {
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
                        if (Random.value < 0.2f)
                        {
                            snstantRecharge = true;
                            bonuses.ReplaceBuff(BonusType.snstantRecharge, "sk002", new Buff(1, () =>
                            {
                                return snstantRecharge;
                            }));
                        }

                        float damage = Random.Range(20.0f, 30.0f) + (player.damage * 0.9f);
                        bool isHitted = Random.value < ComputeHitProb(player.optimalRange, player.extraRange, player.precision,
                                                                _target.player.curent_speed, Vector3.Distance(transform.position, _target.transform.position));

                        StartCoroutine(cor_Launch(0.2f, "Prefabs/Items/Weapons/Missiles/bomb", _target.transform, 1, isHitted, damage, Color.red));

                    }
                }
            };
        }

        public void PulsedRocket()
        {
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

                        float damage = Random.Range(12.0f, 15.0f) + (player.damage * 1.5f);
                        if (_target.bonuses.GetBonus(BonusType.shorted).getValue != 0)
                        {
                            damage = damage * 3;
                        }
                        bool isHitted = Random.value < ComputeHitProb(player.optimalRange, player.extraRange, player.precision,
                                                            _target.player.curent_speed, Vector3.Distance(transform.position, _target.transform.position));

                        StartCoroutine(cor_Launch(0.2f, "Prefabs/Items/Weapons/Missiles/Missile", _target.transform, 1, isHitted, damage, Color.red));

                    }
                }
            };
        }

        public void DevouringDrones()
        {
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
                        float endTime = _skillTime + 18f;
                        bool isHitted = Random.value < ComputeHitProb(player.optimalRange, player.extraRange, player.precision,
                                                                _target.player.curent_speed, Vector3.Distance(transform.position, _target.transform.position));
                        if (isHitted)
                        {
                            _target.bonuses.ReplaceBuff(BonusType.devouringDrones, "sk004", new Buff(12 + (player.damage * 2.2f), () =>
                            {
                                return (_skillTime <= endTime);
                            }));
                            _target.bonuses.GetBonus(BonusType.devouringDrones).endTime = 18;
                        }

                        float damage = Random.Range(12.0f, 14.0f) + (player.damage * 2.0f);
                        StartCoroutine(cor_Launch(0.2f, "Prefabs/Items/Weapons/Missiles/bombRed", _target.transform, 1, isHitted, damage, Color.red));

                    }
                }
            };
        }
        //----------------------/Skills-----------------------

    }
}
*/
