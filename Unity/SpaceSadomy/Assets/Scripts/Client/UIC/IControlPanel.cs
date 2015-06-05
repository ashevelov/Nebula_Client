namespace UIC
{
    using UnityEngine;

    public interface IControlPanel
    {
        void UpdateButton(int index, float progress, Sprite icon = null, System.Action action = null);
    }
}
