using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Game.Space.Res
{
    public static class ResSoundsData
    {
        public static Dictionary<GameSoundsType, AudioClip> LoadGameSounds(string text)
        {
            XDocument document = XDocument.Parse(text);
            Dictionary<GameSoundsType, AudioClip> sounds = new Dictionary<GameSoundsType, AudioClip>();
            foreach (var e in document.Element("sounds").Elements("sound"))
            {
                sounds.Add((GameSoundsType)System.Enum.Parse(typeof(GameSoundsType), e.Attribute("name").Value), UnityEngine.Resources.Load(e.Attribute("path").Value) as AudioClip);
            }
            return sounds;
        }
    }
}
