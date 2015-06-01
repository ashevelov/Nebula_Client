using UnityEngine;
using System.Collections;

namespace Nebula
{
    public class WorldTransitionInfo
    {
        private string _prevWorld;
        private string _nextWorld;

        public WorldTransitionInfo(string prev, string next) {
            _prevWorld = prev;
            _nextWorld = next;
        }

        public bool HasNextWorld() {
            return string.IsNullOrEmpty(_nextWorld) == false;
        }

        public string PrevWorld {
            get {
                return _prevWorld;
            }
        }

        public string NextWorld {
            get {
                return _nextWorld;
            }
        }

        public void SetNextWorld(string next) {
            _prevWorld = _nextWorld;
            _nextWorld = next;
        }

        public void SetPrevAndNextWorld(string prev, string next) {
            SetNextWorld(prev);
            SetNextWorld(next);
        }

        public bool HasPrevWorld() {
            return string.IsNullOrEmpty(_prevWorld) == false;
        }
    }
}
