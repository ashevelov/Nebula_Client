using System;
using System.Collections.Generic;

public class Value<T> : IValue<T> {

    event Action<T> action;

    public void Attach(Action<T> action)
    {
        this.action += action;
    }

    public void Detach(Action<T> action)
    {
        this.action -= action;
    }

    T _value;

    public T Val
    {
        get
        {
            return _value;
        }
        set
        {
            if (!Equals(_value, value))
            {
                if (action != null)
                    action(_value);
                _value = value;
            }
        }
    }
}
