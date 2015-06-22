namespace Nebula.Resources
{
    using UnityEngine;
    using System.Collections;

    public class ResObjectIconData
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public IconType Type { get; set; }
    }

    public enum IconType
    {
        planet,
        player,
        asteroid,
        station,
        mob,
        ivent,
        comet,
        base_object
    }
}
