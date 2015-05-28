using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IValue<T>
{
    // модификация паттерна "Наблюдатель" для сообщения об изменениях в данных
    void Attach(System.Action<T> action); // добавить действие которое необходимо выполнить при изменении val 
    void Detach(System.Action<T> action);
    T Val { get; set; }
}
