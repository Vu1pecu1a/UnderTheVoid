using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : IinvenManager
{
    #region[����]
    // <inheritdoc /> �ּ� ���

    private Vector2Int _size = Vector2Int.one;
    private IinvenProvider _provider;
    private Rect _fullRect;
    public InventoryManager(IinvenProvider provider, int width, int height)
    {
        _provider = provider;
        Rebuild();
        Resize(width, height);
    }
    #endregion[����]

    #region[�������̽�]
    /// <inheritdoc />
    public int width => _size.x;

    /// <inheritdoc />
    public int height => _size.y;
    /// <inheritdoc />
    public Action<IInventoryItem> onItemAdded { get; set; }
    /// <inheritdoc />
    public Action<IInventoryItem> onItemAddedFailed { get; set; }
    /// <inheritdoc />
    public Action<IInventoryItem> onItemRemoved { get; set; }
    /// <inheritdoc />
    public Action<IInventoryItem> onItemDropped { get; set; }
    /// <inheritdoc />
    public Action<IInventoryItem> onItemDroppedFailed { get; set; }
    /// <inheritdoc />
    public Action onRebuilt { get; set; }
    /// <inheritdoc />
    public Action onResized { get; set; }
    /// <inheritdoc />
    public IInventoryItem[] allItems { get; private set; }

    /// <inheritdoc />
    public bool isFull
    {
        get
        {
            if (_provider.isInventoryFull) return true;

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (GetAtPoint(new Vector2Int(x, y)) == null)
                        return false; 
                }
            }
            return true;
        }
    }

    /// <inheritdoc />
    public bool CanAdd(IInventoryItem item)
    {
        Vector2Int point;
        if (!Contains(item) && GetFirstPointThatFitsItem(item, out point))
        {
            return CanAddAt(item, point);
        }
        return false;
    }

    /// <inheritdoc />
    public bool CanAddAt(IInventoryItem item, Vector2Int point)
    {
        if (!_provider.CanAddInventoryItem(item) || _provider.isInventoryFull)
        {
            return false;
        }

        if (_provider.invenSlotType == ItemSoltType.Single)
        {
            return true;
        }

        var previousPoint = item.position;
        item.position = point;
        var padding = Vector2.one * 0.01f;

        // �κ��丮 ũ�⺸�� ū�� �˻�
        if (!_fullRect.Contains(item.GetMinPoint() + padding) || !_fullRect.Contains(item.GetMaxPoint() - padding))
        {
            item.position = previousPoint;
            return false;
        }

        // �������� �ٸ� �κ��丮�� �ִ� �����۰� ��ġ���� �˻��ϱ�
        if (!allItems.Any(otherItem => item.Overlaps(otherItem))) return true; // �׸� �߰� ����

        //�������� ���
        item.position = previousPoint;
        return false;
    }

    /// <inheritdoc />
    public bool CanDrop(IInventoryItem item) => Contains(item) && _provider.CanDropInventoryItem(item) && item.canDrop;

    /// <inheritdoc />
    public bool CanRemove(IInventoryItem item) => Contains(item) && _provider.CanRemoveInventoryItem(item);

    /// <inheritdoc />
    public bool CanSwap(IInventoryItem item)
    {
        return _provider.invenSlotType == ItemSoltType.Single &&
                DoesItemFit(item) &&
                _provider.CanAddInventoryItem(item);
    }

    /// <inheritdoc />
    public void Clear()
    {
        foreach (var item in allItems)
        {
            TryRemove(item);
        }
    }

    /// <inheritdoc />
    public bool Contains(IInventoryItem item) => allItems.Contains(item);

    /// <inheritdoc />
    public void Dispose()
    {
        _provider = null;
        allItems= null;
    }

    /// <inheritdoc />
    public void DropAll()
    {
        var itemsToDrop = allItems.ToArray();
        foreach (var item in itemsToDrop)
        {
            TryDrop(item);
        }
    }

    /// <inheritdoc />
    public IInventoryItem GetAtPoint(Vector2Int point)
    {
        // �������� �� 1���� ���� ĭ�� ���� �Լ�
        if (_provider.invenSlotType == ItemSoltType.Single && _provider.isInventoryFull && allItems.Length > 0)
        { //���� Ÿ���� �̱��̸� ������ĭ�� ���� �������� �ʾ����� ������ ������ 1�� �̻��϶�
            return allItems[0];
        }

        foreach (var item in allItems)
        {
            if (item.Contains(point))
                 return item; 
        }
        return null;
    }

    /// <inheritdoc />
    public IInventoryItem[] GetAtPoint(Vector2Int point, Vector2Int size)
    {
        //������ ũ�Ⱑ 1 �̻��� ���
        var posibleItems = new IInventoryItem[size.x * size.y];
        var c = 0;
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                posibleItems[c] = GetAtPoint(point + new Vector2Int(x, y));
                c++;
            }
        }
        return posibleItems.Distinct().Where(x => x != null).ToArray();//�ߺ������� ���� ����
    }

    /// <inheritdoc />
    public void Rebuild()
    {
        Rebuild(false);
    }
    /// <inheritdoc />
    private void Rebuild(bool silent)
    {
        allItems = new IInventoryItem[_provider.inventoryItemCount];
        for (var i = 0; i < _provider.inventoryItemCount; i++)
        {
            allItems[i] = _provider.GetInventoryItem(i);
        }
        if (!silent) onRebuilt?.Invoke();
    }
    /// <inheritdoc />
    public void Resize(int width, int height)
    {
        _size.x = width;
        _size.y = height;
        RebuildRect();
    }

    //===================�õ� �Լ�============================//

    /// <inheritdoc />
    public bool TryAdd(IInventoryItem item)
    {
        if (!CanAdd(item)) return false;
        Vector2Int point;
        return GetFirstPointThatFitsItem(item, out point) && TryAddAt(item, point);
    }

    /// <inheritdoc />
    public bool TryAddAt(IInventoryItem item, Vector2Int point)
    {
        if (!CanAddAt(item, point) || !_provider.AddInventoryItem(item))
        {
            onItemAddedFailed?.Invoke(item);
            return false;
        }
        switch (_provider.invenSlotType)
        {
            case ItemSoltType.Single:
                item.position = GetCenterPosition(item);
                break;
            case ItemSoltType.Grid:
                item.position = point;
                break;
            default:
                throw new NotImplementedException($"�κ��丮 ���� Ÿ�� {_provider.invenSlotType.ToString()}�� �������� �ʾҽ��ϴ�");
        }
        Rebuild(true);
        onItemAdded?.Invoke(item);
        return true;
    }

    /// <inheritdoc />
    public bool TryDrop(IInventoryItem item)
    {
        if (!CanDrop(item) || !_provider.DropInventoryItem(item))
        {
            onItemDroppedFailed?.Invoke(item);
            return false;
        }
        Rebuild(true);
        onItemDropped?.Invoke(item);
        return true;
    }

    /// <inheritdoc />
    public bool TryRemove(IInventoryItem item)
    {
        if (!CanRemove(item)) return false; //���� �Ұ���
        if (!_provider.RemoveInventoryItem(item)) return false;//�κ��丮���� ������ ���ſ� ���н�
        Rebuild(true);
        onItemRemoved?.Invoke(item);
        return true;
    }

    #endregion[�������̽�]

    #region[�Լ�]
    internal bool TryForceDrop(IInventoryItem item)
    {
        if (!item.canDrop)
        {
            onItemDroppedFailed?.Invoke(item);
            return false;
        }
        onItemDropped?.Invoke(item);
        return true;
    }//���� ���� �Լ�
    private void RebuildRect()
    {
        _fullRect = new Rect(0, 0, _size.x, _size.y);
        HandleSizeChanged();
        onResized?.Invoke();
    }
    public Vector2Int GetCenterPosition(IInventoryItem item)
    {
        return new Vector2Int(
            (_size.x - item.width) / 2,
            (_size.y - item.height) / 2);
    }//�������� �߽���ǥ ����
    private void HandleSizeChanged()
    {
        // �� �̻� �κ��丮�� ũ�⿡ ���� �ʴ� ��� ������ ����
        for (int i = 0; i < allItems.Length;)
        {
            var item = allItems[i];
            var shouldBeDropped = false;
            var padding = Vector2.one * 0.01f;

            if (!_fullRect.Contains(item.GetMinPoint() + padding) || !_fullRect.Contains(item.GetMaxPoint() - padding))
            {
                shouldBeDropped = true;
            }

            if (shouldBeDropped)
            {
                TryDrop(item);
            }
            else
            {
                i++;
            }
        }
    }

    /// <summary>
    /// �������� �κ��丮ũ�⸦ �Ѿ�� ������ true��ȯ
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool DoesItemFit(IInventoryItem item) => item.width <= width && item.height <= height;

    /// <summary>
    /// �κ��丮�� ���� �ϴܺ��� ���
    /// </summary>
    /// <param name="item"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    private bool GetFirstPointThatFitsItem(IInventoryItem item, out Vector2Int point)
    {
        if (DoesItemFit(item))
        {
            for (var y = height - (item.height - 1); y > 0 ; y--)//���������� �ֱ�
            {
                for (var x = 0; x < width - (item.width - 1); x++)
                {
                    point = new Vector2Int(x, y);
                    if (CanAddAt(item, point)) return true;
                }
            }
        }
        point = Vector2Int.zero;
        return false;
    }
    #endregion[�Լ�]
}
