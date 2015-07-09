namespace Nebula.Mmo.Items.Components {
    using UnityEngine;
    using System.Collections;
    using Common;
    using System;
    using System.Collections.Generic;

    public class MmoBaseComponent  {

        public static MmoBaseComponent CreateNew(ComponentID id) {
            switch(id) {
                case ComponentID.Activator: return new MmoActivatorComponent();
                case ComponentID.Asteroid: return new MmoAsteroidComponent();
                case ComponentID.Bonuses: return new MmoBonusesComponent();
                case ComponentID.Bot: return new MmoBotComponent();
                case ComponentID.Character: return new MmoCharacterComponent();
                case ComponentID.Chest: return new MmoChestComponent();
                case ComponentID.CombatAI: return new MmoCombatAIComponent();
                case ComponentID.Damagable: return new MmoDamagableComponent();
                case ComponentID.Energy: return new MmoEnergyComponent();
                case ComponentID.Event: return new MmoEventComponent();
                case ComponentID.EventedObject: return new MmoEventedObjectComponent();
                case ComponentID.Model: return new MmoModelComponent();
                case ComponentID.PirateStation: return new MmoPirateStationComponent();
                case ComponentID.Planet: return new MmoPlanetComponent();
                case ComponentID.Player: return new MmoPlayerComponent();
                case ComponentID.PlayerAI: return new MmoPlayerAIComponent();
                case ComponentID.Raceable: return new MmoRaceableComponent();
                case ComponentID.Ship: return new MmoShipComponent();
                case ComponentID.Skills: return new MmoSkillsComponent();
                case ComponentID.Target: return new MmoTargetComponent();
                case ComponentID.Weapon: return new MmoWeaponComponent();
                case ComponentID.Movable: return new MmoMovableComponent();
                default: return null;
            }
        }

        protected Item item { get; private set; }

        public void SetItem(Item item) {
            this.item = item;
        }

        public virtual void Update() { }
    }
}
