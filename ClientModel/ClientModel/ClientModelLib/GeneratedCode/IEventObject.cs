using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate bool DEventCondition();
public interface IEventObject
{
    string Name1 { get; set; }
    void AttachСondition(DEventCondition condition); 
    void DetachСondition(DEventCondition condition);
    void AttachEvent(System.Action action);
    void DetachEvent(System.Action action);
    void StartEvent();
}
