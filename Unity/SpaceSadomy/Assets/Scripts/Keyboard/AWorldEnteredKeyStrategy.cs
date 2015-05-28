using UnityEngine;
using System.Collections;
using Nebula;

public class AWorldEnteredKeyStrategy : KeyStrategy
{
    public AWorldEnteredKeyStrategy(string id)
        : base(id)
    { }


    public override void HandleDown()
    {
        if(G.IsPlayerTargetValid())
        {
            NRPC.RequestFire(Common.ShotType.Light);
        }
    }
}
