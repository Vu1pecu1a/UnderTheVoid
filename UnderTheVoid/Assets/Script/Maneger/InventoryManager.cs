using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : IinvenManager
{
    #region[변수]
    // <inheritdoc /> 주석 상속

    private Vector2Int _size = Vector2Int.one;
    private IinvenProvider _provider;
    private Rect _fullRect;
    public InventoryManager(IinvenProvider provider, int width, int height)
    {
        _provider = provider;
        Rebuild();
        Resize(width, height);
    }
    #endregion[변수]

    #region[인터페이스]
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

        // 인벤토리 크기보다 큰지 검사
        if (!_fullRect.Contains(item.GetMinPoint() + padding) || !_fullRect.Contains(item.GetMaxPoint() - padding))
        {
            item.position = previousPoint;
            return false;
        }

        // 아이템이 다른 인벤토리에 있는 아이템과 겹치는지 검사하기
        if (!allItems.Any(otherItem => item.Overlaps(otherItem))) return true; // 항목 추가 가능

        //실패했을 경우
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
        // 아이템이 딱 1개만 들어가는 칸을 위한 함수
        if (_provider.invenSlotType == ItemSoltType.Single && _provider.isInventoryFull && allItems.Length > 0)
        { //슬롯 타입이 싱글이며 아이템칸이 전부 점유되지 않았으며 아이템 갯수가 1개 이상일때
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
        //아이템 크기가 1 이상인 경우
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
        return posibleItems.Distinct().Where(x => x != null).ToArray();//중복제거한 다음 리턴
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

    //===================시도 함수============================//

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
                throw new NotImplementedException($"인벤토리 슬롯 타입 {_provider.invenSlotType.ToString()}가 구현되지 않았습니다");
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
        if (!CanRemove(item)) return false; //제거 불가시
        if (!_provider.RemoveInventoryItem(item)) return false;//인벤토리에서 아이템 제거에 실패시
        Rebuild(true);
        onItemRemoved?.Invoke(item);
        return true;
    }

    #endregion[인터페이스]

    #region[함수]
    internal bool TryForceDrop(IInventoryItem item)
    {
        if (!item.canDrop)
        {
            onItemDroppedFailed?.Invoke(item);
            return false;
        }
        onItemDropped?.Invoke(item);
        return true;
    }//강제 삭제 함수
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
    }//아이템의 중심좌표 리턴
    private void HandleSizeChanged()
    {
        // 더 이상 인벤토리에 크기에 맞지 않는 모든 아이템 삭제
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
    /// 아이템이 인벤토리크기를 넘어가지 않으면 true반환
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool DoesItemFit(IInventoryItem item) => item.width <= width && item.height <= height;

    /// <summary>
    /// 인벤토리의 가장 하단부터 계산
    /// </summary>
    /// <param name="item"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    private bool GetFirstPointThatFitsItem(IInventoryItem item, out Vector2Int point)
    {
        if (DoesItemFit(item))
        {
            for (var y = height - (item.height - 1); y > 0 ; y--)//위에서부터 넣기
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
    #endregion[함수]
}
