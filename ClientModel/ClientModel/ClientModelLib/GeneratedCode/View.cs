using UnityEngine;
using System.Collections;

namespace UIC
{
public class View : MonoBehaviour, IView
{
    IViewProperties _properties;
    public void Init(IViewProperties properties)
    {
        _properties = properties;
    }
}
}

