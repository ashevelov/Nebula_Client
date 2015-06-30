using Common;
using ServerClientCommon;
using System.Collections;

namespace Nebula.Client {
    public class ClientWorkhouseStation {
        private ClientWorkhouseStationHold hold;
        private ClientInventory stationInventory;


        public ClientWorkhouseStation(Hashtable info) {
            this.LoadInfo(info);
        }

        public void LoadInfo(Hashtable info) {
            //int holdMaxSlots = info.GetValue<int>(GenericEventProps.hold_max_slots, 0);
            if (this.hold == null)
                this.hold = new ClientWorkhouseStationHold(0);
            this.hold.Clear();
            this.hold.ParseInfo(info.GetValue<Hashtable>((int)SPC.Hold, new Hashtable()));

            if (this.stationInventory == null)
                this.stationInventory = new ClientInventory();
            this.stationInventory.Clear();
            this.stationInventory.ParseInfo(info.GetValue<Hashtable>((int)SPC.Inventory, new Hashtable()));
        }


        public ClientWorkhouseStationHold Hold {
            get { return this.hold; }
        }

        public ClientInventory StationInventory {
            get { return this.stationInventory; }
        }

        public void Clear() {
            hold.Clear();
            stationInventory.Clear();
        }
    }
}
