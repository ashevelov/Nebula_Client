using UnityEngine;
using System.Collections;
using Common;
using Nebula.Mmo.Games;

namespace Nebula
{
    public abstract class NpcItem : ForeignItem
    {

        private Common.BotItemSubType _subType;

        public NpcItem(string id, byte type, NetworkGame game, BotItemSubType subType, string name, object[] inComponents) :
            base(id, type, game, name, inComponents)
        {
            _subType = subType;
        }

        public BotItemSubType SubType
        {
            get
            {
                return _subType;
            }
        }
    }
}