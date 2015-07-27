using Game.Space.UI;
using Game.Space;
using Nebula.Client;

namespace Nebula {
    using Common;
    using Nebula.Client.Inventory;
    using System.Collections.Generic;

    public static class NoLinqUtils {

        public class ClientShipModuleTypeComparer : IComparer<ClientShipModule> {
            public int Compare(ClientShipModule x, ClientShipModule y) {
                return x.Type.CompareTo(y.Type);
            }
        }

        public static List<ClientShipModule> OrderByType(List<ClientShipModule> inputs) {
            inputs.Sort(new ClientShipModuleTypeComparer());
            return inputs;
        }

        //public static int[] SortedKeys(Dictionary<int, List<UIEntity>> dict) {
        //    int[] result = new int[dict.Count];
        //    int index = 0;
        //    foreach (var kv in dict) {
        //        result[index++] = kv.Key;
        //    }
        //    System.Array.Sort(result);
        //    return result;
        //}

        public static List<IInventoryObjectInfo> FilterByHoldType(List<IInventoryObjectInfo> inputs, InventoryObjectType type) {
            List<IInventoryObjectInfo> result = new List<IInventoryObjectInfo>();
            foreach (IInventoryObjectInfo obj in inputs) {
                if (obj.Type == type) {
                    result.Add(obj);
                }
            }
            return result;
        }

        public static List<IInventoryObjectInfo> FilterByPredicate(List<IInventoryObjectInfo> inputs, System.Func<IInventoryObjectInfo, bool> predicate) {
            List<IInventoryObjectInfo> result = new List<IInventoryObjectInfo>();
            foreach (IInventoryObjectInfo obj in inputs) {
                if (predicate(obj)) {
                    result.Add(obj);
                }
            }
            return result;
        }

        public static List<ClientShipModule> CastToModuleType(List<IInventoryObjectInfo> inputs) {
            List<ClientShipModule> result = new List<ClientShipModule>();
            foreach (IInventoryObjectInfo obj in inputs) {
                if (obj is ClientShipModule) {
                    result.Add(obj as ClientShipModule);
                }
            }
            return result;
        }

        public static bool Contains(string[] inputs, string search) {
            foreach (var s in inputs) {
                if (s == search) {
                    return true;
                }
            }
            return false;
        }
    }
}