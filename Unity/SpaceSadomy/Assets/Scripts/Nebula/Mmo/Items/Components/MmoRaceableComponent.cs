namespace Nebula.Mmo.Items.Components {
    using UnityEngine;
    using System.Collections;
    using Common;

    public class MmoRaceableComponent : MmoBaseComponent{

        public Race race {
            get {
                byte bRace;
                if(item != null) {
                    if ( item.TryGetProperty<byte>((byte)PS.Race, out bRace) ) {
                        return (Race)bRace;
                    }
                }
                return Race.None;
            }
        }
    }
}
