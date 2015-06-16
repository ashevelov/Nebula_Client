namespace Nebula.Test {
    using UnityEngine;
    using System.Collections;

    public class RandomTest : MonoBehaviour {

        // Use this for initialization
        void Awake() {
            int runCount = 1000;
            while (runCount > 0) {
                int[] stats = new int[] { 0, 0, 0, 0, 0 };
                int points = 16;
                while (points > 0) {
                    for (int i = 0; i < stats.Length; i++) {
                        if (Random.value < 0.5f && points > 0) {
                            stats[i] += 1;
                            points--;
                        }
                    }
                }
                //int max = 6;
                //if (stats[0] > max || stats[1] > max || stats[2] > max || stats[3] > max || stats[4] > max)
                //{
                //    Debug.Log( stats[0] + " " + stats[1] + " " + stats[2] + " " + stats[3] + " " + stats[4]);
                //}
                Debug.Log(ColorDebug(stats[0]) + " " + ColorDebug(stats[1]) + " " + ColorDebug(stats[2]) + " " + ColorDebug(stats[3]) + " " + ColorDebug(stats[4]));
                runCount--;
            }
        }

        private string ColorDebug(int val) {
            switch (val) {
                case 0: return "<color=#0000ff>" + val + "</color>";
                case 1: return "<color=#4444ff>" + val + "</color>";
                case 2: return "<color=#7777ff>" + val + "</color>";
                case 3: return "<color=#aaaaff>" + val + "</color>";
                case 4: return "<color=#ffffff>" + val + "</color>";
                case 5: return "<color=#ffaaaa>" + val + "</color>";
                case 6: return "<color=#ff7777>" + val + "</color>";
                case 7: return "<color=#ff4444>" + val + "</color>";
                case 8: return "<color=#ff0000>" + val + "</color>";
                case 9: return "<color=#00ff00>" + val + "</color>";
                case 10: return "<color=#00ff00>" + val + "</color>";
                case 11: return "<color=#00ff00>" + val + "</color>";
                case 12: return "<color=#00ff00>" + val + "</color>";
                case 13: return "<color=#00ff00>" + val + "</color>";
                case 14: return "<color=#00ff00>" + val + "</color>";
                case 15: return "<color=#00ff00>" + val + "</color>";
            }
            return val.ToString();
        }
    }
}