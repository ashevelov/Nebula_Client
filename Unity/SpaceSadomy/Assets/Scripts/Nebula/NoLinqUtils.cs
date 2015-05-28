using Game.Space.UI;
using Game.Space;
using Nebula.Client;

namespace Nebula {
using Common;
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

        public static List<IStationHoldableObject> FilterByHoldType(List<IStationHoldableObject> inputs, StationHoldableObjectType type) {
            List<IStationHoldableObject> result = new List<IStationHoldableObject>();
            foreach (IStationHoldableObject obj in inputs) {
                if (obj.Type == type) {
                    result.Add(obj);
                }
            }
            return result;
        }

        public static List<IStationHoldableObject> FilterByPredicate(List<IStationHoldableObject> inputs, System.Func<IStationHoldableObject, bool> predicate) {
            List<IStationHoldableObject> result = new List<IStationHoldableObject>();
            foreach (IStationHoldableObject obj in inputs) {
                if (predicate(obj)) {
                    result.Add(obj);
                }
            }
            return result;
        }

        public static List<ClientShipModule> CastToModuleType(List<IStationHoldableObject> inputs) {
            List<ClientShipModule> result = new List<ClientShipModule>();
            foreach (IStationHoldableObject obj in inputs) {
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