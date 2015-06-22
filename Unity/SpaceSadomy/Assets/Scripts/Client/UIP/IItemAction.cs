using System;
using UnityEngine;

namespace UIC
{
    public interface IItemAction
    {
        string Text { get; set; }
        Action action { get; set; }
    }
}
