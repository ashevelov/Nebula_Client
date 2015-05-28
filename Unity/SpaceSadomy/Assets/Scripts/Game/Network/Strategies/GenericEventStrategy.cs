using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Common;

public abstract class GenericEventStrategy  
{
    protected Hashtable Properties(EventData eventData)
    {
        return (Hashtable)eventData.Parameters[ParameterCode.EventData.toByte()];
    }
}
