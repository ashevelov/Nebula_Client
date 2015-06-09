namespace UIC
{
    using UnityEngine;

    public interface ITargetInfo
    {
        string Name { set; }
        Sprite Icon { set; }
        int MaxHP { set; }
        int CurentHP { set; }
        float Distance { set; }
        int Level { set; }

    }
}
