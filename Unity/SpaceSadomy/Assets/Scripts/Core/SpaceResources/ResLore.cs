using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Game.Space.Res
{
    public static class ResLore
    {
        public static List<ResLoreData> LoadLore(string text)
        {
            XDocument document = XDocument.Parse(text);
            List<ResLoreData> lore = new List<ResLoreData>();
            foreach (var e in document.Element("lore").Elements("data"))
            {
                lore.Add(new ResLoreData { title = e.Attribute("title").Value, text = e.Attribute("text").Value });
            }
            return lore;
        }
    }

    public class ResLoreData
    {
        public string title;
        public string text;
    }
}
