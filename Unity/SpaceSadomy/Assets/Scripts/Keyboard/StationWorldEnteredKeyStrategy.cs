using UnityEngine;
using System.Collections;
using Game.Space.UI;
using Common;

public class StationWorldEnteredKeyStrategy : KeyStrategy 
{
    public StationWorldEnteredKeyStrategy(string id)
        : base(id)
    { }

    public override void HandleDown()
    {
        G.Game.EnterWorkshop( WorkshopStrategyType.Angar );
    }
}
