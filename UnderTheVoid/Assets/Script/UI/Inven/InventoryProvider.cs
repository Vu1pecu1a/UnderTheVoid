using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryProvider : IinvenProvider
{
    
    private List<IInventoryItem> _items = new List<IInventoryItem>();
    private int _maximumAlowedItemCount;
    ItemType _allowedItem;

    /// <summary>
    /// CTOR
    /// </summary>
    public InventoryProvider(ItemSoltType renderMode, int maximumAlowedItemCount = -1, ItemType allowedItem = ItemType.Any)
    {
        invenSlotType = renderMode;
        _maximumAlowedItemCount = maximumAlowedItemCount;
        _allowedItem = allowedItem;
    }
    #region[인터페이스 구현]
    public ItemSoltType invenSlotType { get; private set; }

    public int inventoryItemCount => _items.Count;

    public bool isInventoryFull
    {
        get
        {
            if (_maximumAlowedItemCount < 0) return false;
            return inventoryItemCount >= _maximumAlowedItemCount;
        }
    }
    public bool AddInventoryItem(IInventoryItem item)
    {
        if (!_items.Contains(item))
        {
            _items.Add(item);
            return true;
        }
        return false;
    }

    public bool CanAddInventoryItem(IInventoryItem item)
    {
        if (_allowedItem == ItemType.Any) return true;
        return (item is ItemDefinition) ?
            (item as ItemDefinition).Type == _allowedItem : (item as LoadItem).Type == _allowedItem;
    }

    public bool CanDropInventoryItem(IInventoryItem item)
    {
        return true;
    }

    public bool CanRemoveInventoryItem(IInventoryItem item)
    {
        return true;
    }

    public bool DropInventoryItem(IInventoryItem item)
    {
        return RemoveInventoryItem(item);
    }

    public IInventoryItem GetInventoryItem(int index)
    {
        return _items[index];
    }

    public bool RemoveInventoryItem(IInventoryItem item)
    {
        return _items.Remove(item);
    }

    #endregion[인터페이스 구현]
}
