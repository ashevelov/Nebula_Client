//using UnityEngine;
//using System.Collections;
//using Photon.Mmo.Client;
//using Common;
//using Game.Space;
//using System.Collections.Generic;
//using Nebula.Client;

//public class GameKeyboard : Singleton<GameKeyboard>
//{

//    private Dictionary<GameState, KeyboardStrategy> strategies;
//    private KeyCode[] keyCodes;

//    void Start()
//    {
//        this.strategies = new Dictionary<GameState, KeyboardStrategy>
//        {
//            {GameState.Connected, new ConnectedKeyboardStrategy() },
//            {GameState.Disconnected, new DisconnectedKeyboardStrategy() },
//            {GameState.Login, new LoginKeyboardStrategy() },
//            {GameState.SelectCharacter, new SelectCharacterKeyboardStrategy() },
//            {GameState.WaitForConnect, new WaitForConnectKeyboardStrategy() },
//            {GameState.WaitingForChangeWorld, new WaitForChangeWorldKeyboardStrategy() },
//            {GameState.Workshop, new WorkshopKeyboardStrategy() },
//            {GameState.WorldEntered, new WorldEnteredKeyboardStrategy() }
//        };
//        this.keyCodes = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));

//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.C, new CWorldEnteredKeyStrategy(typeof(CWorldEnteredKeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.T, new TWorldEnteredKeyStrategy(typeof(TWorldEnteredKeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.Escape, new EscapeWorldEnteredKeyStrategy(typeof(EscapeWorldEnteredKeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.I, new InventoryWorldEnteredKeyStrategy(typeof(InventoryWorldEnteredKeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.Workshop, KeyCode.I, new InventoryWorldEnteredKeyStrategy(typeof(InventoryWorldEnteredKeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.Workshop, KeyCode.H, new HoldWorldEnteredKeyStrategy(typeof(HoldWorldEnteredKeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.M, new MapWorldEnteredKeyStrategy(typeof(MapWorldEnteredKeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.F, new ShipInfoWorldEnteredKeyStrategy(typeof(ShipInfoWorldEnteredKeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.Workshop, KeyCode.F, new ShipInfoWorldEnteredKeyStrategy(typeof(ShipInfoWorldEnteredKeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.H, new WorldEnteredHelpKeyStrategy(typeof(WorldEnteredHelpKeyStrategy).FullName));


//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.P, new P_TempWorldEnteredKeyboardStrategy(typeof(P_TempWorldEnteredKeyboardStrategy).FullName));
//        //this.AddKeyStrategy(GameState.Workshop, KeyCode.P, new P_TempWorldEnteredKeyboardStrategy(typeof(P_TempWorldEnteredKeyboardStrategy).FullName));

//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.Alpha1, new LightShotKeyStrategy(typeof(LightShotKeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.Alpha2, new HeavyShotKeyStrategy(typeof(HeavyShotKeyStrategy).FullName));        
//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.Alpha3, new UseSkill_0_KeyStrategy(typeof(UseSkill_0_KeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.Alpha4, new UseSkill_1_KeyStrategy(typeof(UseSkill_1_KeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.Alpha5, new UseSkill_2_KeyStrategy(typeof(UseSkill_2_KeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.Alpha6, new UseSkill_3_KeyStrategy(typeof(UseSkill_3_KeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.Alpha7, new UseSkill_4_KeyStrategy(typeof(UseSkill_4_KeyStrategy).FullName));
//        //this.AddKeyStrategy(GameState.WorldEntered, KeyCode.Alpha8, new UseSkill_5_KeyStrategy(typeof(UseSkill_5_KeyStrategy).FullName));
//    }

//    public void AddKeyStrategy(GameState state, KeyCode keyCode, KeyStrategy strategy)
//    {
//        KeyboardStrategy stateStrategy = null;
//        if(!this.strategies.TryGetValue(state, out stateStrategy))
//        {
//            throw new NebulaException("Not found keyboard strategy for state: {0}".f(state));
//        }
//        stateStrategy.AddKeyStrategy(keyCode, strategy);
//    }

//    public void RemoveKeyStrategy(GameState state, KeyCode keyCode, string id)
//    {
//        KeyboardStrategy stateStrategy = null;
//        if(!this.strategies.TryGetValue(state, out stateStrategy))
//        {
//            throw new NebulaException("Not found keyboard strategy for state: {0}".f(state));
//        }
//        stateStrategy.RemoveKeyStrategy(keyCode, id);
//    }

//    void Update() 
//    {
//        if (Input.anyKeyDown)
//        {
//            foreach (var code in keyCodes)
//            {
//                if (Input.GetKey(code))
//                {
//                    if (G.UI)
//                    {
//                        G.UI.OnKeyDown(code);
//                    }

//                    this.strategies[G.Game.State].HandleKeyDown(code);
//                }
//            }
//        }
//        if(Input.anyKey)
//        {
//            foreach(var code in keyCodes)
//            {
//                if(Input.GetKey(code))
//                {
//                    this.strategies[G.Game.State].HandleKey(code);
//                }
//            }
//        }

//        foreach(var keyCode in keyCodes)
//        {
//            if(Input.GetKeyUp(keyCode))
//            {
//                this.strategies[G.Game.State].HandleKeyUp(keyCode);
//            }
//        }
//    }
//}
