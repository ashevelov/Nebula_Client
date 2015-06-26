using System;
using UnityEngine;

namespace Client.UIP
{
    public interface IItemAction
    {
        string Text { get; set; }
        Action action { get; set; }
    }
}
