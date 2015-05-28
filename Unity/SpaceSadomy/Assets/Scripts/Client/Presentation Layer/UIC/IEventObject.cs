using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate bool DEventCondition();
public interface IEventObject
{
    string Name { get; set; }
    void AttachСondition(DEventCondition condition);// добавление условий выполнения ивента 
    void DetachСondition(DEventCondition condition);
    void AttachEvent(System.Action action); // добавление ивентов
    void DetachEvent(System.Action action);
    void StartEvent();
}
