using UnityEngine;
using System.Collections;

public class ConnectedKeyboardStrategy : KeyboardStrategy 
{
    public override void HandleKey(KeyCode code)
    {
        base.HandleKey(code);
    }
    public override void HandleKeyDown(KeyCode code)
    {
        base.HandleKeyDown(code);
    }
    public override void HandleKeyUp(KeyCode code)
    {
        base.HandleKeyUp(code);
    }
}
