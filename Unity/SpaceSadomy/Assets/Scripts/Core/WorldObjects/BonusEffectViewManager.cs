// BonusEffectViewManager.cs
// Nebula
// 
// Created by Oleg Zhelestcov on Wednesday, November 12, 2014 9:21:13 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//

namespace Game.Space
{
    using UnityEngine;
    using System.Collections;
    using Common;
    using System.Collections.Generic;
    using Nebula.Client;

    public delegate GameObject BonusEffectMaker(BaseSpaceObject parent);
    /// <summary>
    /// Control lifetime of bonus effects
    /// </summary>
    public class BonusEffectViewManager
    {
        private Dictionary<BonusType, GameObject> bonusesEffects = new Dictionary<BonusType, GameObject>();
        private Dictionary<BonusType, BonusEffectMaker> effectMakers = new Dictionary<BonusType, BonusEffectMaker>();

        /// <summary>
        /// Delete effect from dictionary and object effect from game
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(BonusType key)
        {
            if(this.bonusesEffects.ContainsKey(key))
            {
                GameObject effectObj = this.bonusesEffects[key];
                this.bonusesEffects.Remove(key);
                if( effectObj )
                {
                    GameObject.Destroy(effectObj);
                    return true;
                }
            }
            return false;
        }

        public void DeleteAll()
        {
            foreach(var p in this.bonusesEffects )
            {
                if(p.Value)
                {
                    GameObject.Destroy(p.Value);
                }
            }
            this.bonusesEffects.Clear();
        }

        public void Update(BaseSpaceObject parent, ActorBonuses actorBonuses)
        {
            /*
            if(parent.Item.IsMine)
            {
                foreach(var b in actorBonuses.Bonuses)
                {
                    if(b.Value > 0 )
                    {
                        Debug.Log("bonus {0} > 0 ".f(b.Key));
                    }
                }
            }*/

            List<BonusType> removedKeys = new List<BonusType>();
            foreach(var p in this.bonusesEffects)
            {
                if( (0f == actorBonuses.Value(p.Key)))
                {
                    removedKeys.Add(p.Key);
                }
            }

            foreach(var key in removedKeys)
            {
                this.Delete(key);
            }
            removedKeys.Clear();

            List<BonusType> addedKeys = new List<BonusType>();
            foreach(var p in actorBonuses.Bonuses )
            {
                if((actorBonuses.Value(p.Key) != 0f) && (false == this.bonusesEffects.ContainsKey(p.Key)))
                {
                    addedKeys.Add(p.Key);
                }
            }

            foreach(var key in addedKeys )
            {
                /*
                if(parent.Item.IsMine)
                {
                    Debug.Log("check maker for key: {0}".f(key));
                }*/

                if(this.effectMakers.ContainsKey(key))
                {
                    if (this.effectMakers[key] != null)
                    {
                        //this.effectMakers[key](parent);
                        this.bonusesEffects[key] = this.effectMakers[key](parent);
                    }
                }
                else
                {
                    /*
                    if(parent.Item.IsMine)
                    {
                        Debug.Log("not contains maker for key: {0}".f(key));
                    }*/
                }
            }
            addedKeys.Clear();
        }

        public void SetEffectMaker(BonusType key, BonusEffectMaker maker )
        {
            this.effectMakers[key] = maker;
        }
    }
}
