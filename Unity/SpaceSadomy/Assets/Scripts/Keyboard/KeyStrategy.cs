using UnityEngine;
using System.Collections;

public abstract class KeyStrategy 
{
    private readonly string id;

    public KeyStrategy(string id)
    {
        this.id = id;
    }

    public string Id()
    {
        return this.id;
    }

    public virtual void Handle() { }
    public virtual void HandleDown() { }
    public virtual void HandleUp() { }
}
