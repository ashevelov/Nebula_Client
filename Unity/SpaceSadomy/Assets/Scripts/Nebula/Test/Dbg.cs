// Dbg.cs
// Nebula
//
// Created by Oleg Zhelestcov on Wednesday, October 29, 2014 2:57:40 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//
using Common;
using Nebula;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Nebula.Test {
    public static class Dbg {
        public class ItemStatistics {
            public ItemType ItemType { get; set; }
            public int ItemCount { get; set; }
            public int ItemViewCount { get; set; }
            public int MissingViewCount { get; set; }

            public int NullItemCount { get; set; }

            public List<Item> MissingViewItems { get; set; }

            public override string ToString() {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format("{0}-> item count: {1}, view count: {2}, missing view count: {3}, null item count: {4}",
                    ItemType, ItemCount, ItemViewCount, MissingViewCount, NullItemCount));

                if (this.MissingViewItems != null) {
                    foreach (var it in this.MissingViewItems) {
                        sb.AppendLine(string.Format("    missing view item-> destroyed: {0} subscribed: {1} subtype: {2}", it.IsDestroyed, it.Subscribed, (it is NpcItem) ? ((NpcItem)it).SubType : BotItemSubType.None));
                    }
                }
                return sb.ToString();
            }
        }


        public static void PrintAction(string action, Hashtable result) {
            action.Color(Color.blue).Bold().Print();
            result.Print(1);
        }

        public static void PrintGenericEvent(CustomEventCode code, Hashtable result) {
            code.ToString().Color(Color.yellow).Bold().Print();
            result.Print(1);
        }

        public static void PrintArray(string title, object[] array) {
            title.Color(Color.cyan).Bold().Print();
            array.Print(1);
        }

        public static void Print(string message, string filter = "DEFAULT") {
            if (MmoEngine.Get && G.Game != null) {
                if (NoLinqUtils.Contains(G.Game.Settings.LogFilters, filter)) {

                }
            }
        }



        public static void GetItemsStatistic(ItemType itemType, out ItemStatistics statistics) {
            if (G.Game == null || G.Game.Items == null) {
                statistics = new ItemStatistics { ItemType = itemType, ItemCount = 0, ItemViewCount = 0, MissingViewCount = 0 };
                return;
            }

            var items = G.Game.Items;
            byte bItemType = itemType.toByte();
            Dictionary<string, Item> typedItems;
            if (!items.TryGetValue(bItemType, out typedItems)) {
                statistics = new ItemStatistics { ItemType = itemType, ItemCount = 0, ItemViewCount = 0, MissingViewCount = 0 };
                return;
            }

            int itemCount = 0;
            int viewCount = 0;
            int missingViewCount = 0;
            int nullItemCount = 0;
            var missingViewItems = new List<Item>();

            foreach (var pair in typedItems) {
                if (pair.Value != null) {
                    itemCount++;
                    if (pair.Value.View)
                        viewCount++;
                    else {
                        missingViewCount++;
                        missingViewItems.Add(pair.Value);
                    }
                } else
                    nullItemCount++;
            }

            statistics = new ItemStatistics {
                ItemType = itemType,
                ItemCount = itemCount,
                ItemViewCount = viewCount,
                MissingViewCount = missingViewCount,
                NullItemCount = nullItemCount,
                MissingViewItems = missingViewItems
            };
        }
    }
}