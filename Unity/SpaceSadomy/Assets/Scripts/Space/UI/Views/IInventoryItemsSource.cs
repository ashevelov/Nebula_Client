// IInventoryItemsSource.cs
// Nebula
// 
// Created by Oleg Zhelestcov on Sunday, November 23, 2014 6:25:36 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//

namespace Nebula.UI
{
    using Common;
    using Nebula.Client.Inventory;
    using System.Collections.Generic;

    /// <summary>
    /// Relized by objects which sources for InventorySourceContentView
    /// </summary>
    public interface IInventoryItemsSource 
    {
        List<ClientInventoryItem> Items { get; }

        string Id { get; }

        void SetItems(List<ClientInventoryItem> items);

        ItemType SourceType { get;  }

        bool RemoveItem(string itemId);
    }

}

