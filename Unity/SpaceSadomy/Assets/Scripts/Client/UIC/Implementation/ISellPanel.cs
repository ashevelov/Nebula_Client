using System;
using UnityEngine;

namespace Client.UIC
{
    interface ISellPanel
    {
        int GetPrice();
        void SetStartPrice(int price);
        void SetDescrption(string desc);
        void SetIcon(Sprite sprite);
    }
}
