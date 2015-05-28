using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class KeyboardStrategy  
{
    private Dictionary<KeyCode, Dictionary<string, KeyStrategy>> strategies;

    public KeyboardStrategy()
    {
        this.strategies = new Dictionary<KeyCode, Dictionary<string, KeyStrategy>>();
    }

    public virtual void HandleKey(KeyCode code)
    {
        Dictionary<string, KeyStrategy> filteredStrategies = null;
        if(this.strategies.TryGetValue(code, out filteredStrategies))
        {
            foreach(var pStrategy in filteredStrategies)
            {
                pStrategy.Value.Handle();
            }
        }
    }
    public virtual void HandleKeyDown(KeyCode code)
    {
        Dictionary<string, KeyStrategy> filteredStrategies = null;
        if(this.strategies.TryGetValue(code, out filteredStrategies))
        {
            foreach(var pStrategy in filteredStrategies)
            {
                pStrategy.Value.HandleDown();
            }
        }
    }
    public virtual void HandleKeyUp(KeyCode code)
    {
        Dictionary<string, KeyStrategy> filteredStrategies = null;
        if(this.strategies.TryGetValue(code, out filteredStrategies))
        {
            foreach(var pStrategy in filteredStrategies)
            {
                pStrategy.Value.HandleUp();
            }
        }
    }

    public void AddKeyStrategy(KeyCode code, KeyStrategy strategy)
    {
        Dictionary<string, KeyStrategy> filteredStrategies = null;
        if(this.strategies.TryGetValue(code, out filteredStrategies))
        {
            filteredStrategies[strategy.Id()] = strategy;
        }
        else
        {
            this.strategies.Add(code, new Dictionary<string, KeyStrategy> { { strategy.Id(), strategy } });
        }
    }

    public void RemoveKeyStrategy(KeyCode keyCode, string id)
    {
        Dictionary<string, KeyStrategy> filteredStrategies = null;
        if(this.strategies.TryGetValue(keyCode, out filteredStrategies))
        {
            filteredStrategies.Remove(id);
        }
    }
}
