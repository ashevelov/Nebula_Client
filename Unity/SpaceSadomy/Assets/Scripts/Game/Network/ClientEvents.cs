using UnityEngine;

namespace Game.Network
{
    public class ClientEvents
    {
        public void StartEvent(string props, object value)
        {
            switch (props)
            {
                case "use_skill_1":
                    {
                        Debug.Log("use_skill_1");
                        break;
                    }
                default:
                    break;
            }
        }

    }
}
