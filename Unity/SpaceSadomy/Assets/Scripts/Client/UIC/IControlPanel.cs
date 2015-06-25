namespace UIC
{
    using UnityEngine;

    public interface IControlPanel
    {
        void UpdateButton(int index, float cooldown, float progress, Sprite icon = null, System.Action<int> action = null);
    }
}
