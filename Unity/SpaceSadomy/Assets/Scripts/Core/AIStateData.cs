﻿using UnityEngine;
using System.Collections;
using Common;
using Game.Network;

namespace Nebula {
    public class AIStateData : IServerPropertyParser {
        private Item _owner;
        public AIStateData(Item owner) {
            _owner = owner;
        }

        private PlayerState _prevControlState;
        private PlayerState _controlState;
        private PlayerState _actionState;



        public PlayerState PrevControlState { get { return _prevControlState; } }
        public PlayerState ControlState { get { return _controlState; } }

        public PlayerState ActionState { get { return _actionState; } }


        public void ParseProp(string propName, object value) {
            switch (propName) {
                case Props.SHIP_BASE_STATE_CONTROL_STATE:
                    _prevControlState = _controlState;
                    _controlState = (PlayerState)(byte)value;
                    if (_owner.Component) {
                        if (_owner.Component is MyPlayer) {
                            MyPlayer myPlayer = _owner.Component as MyPlayer;
                            myPlayer.GotoControlState(_controlState);
                        }
                    }
                    break;
            }
        }

        public void ParseProps(Hashtable properties) {
            foreach (DictionaryEntry entry in properties) {
                ParseProp(entry.Key.ToString(), entry.Value);
            }
        }

        public void ForceControlState(PlayerState state) {
            _controlState = state;
        }
        public void ForceActionState(PlayerState state) {
            _actionState = state;
        }
    }
}