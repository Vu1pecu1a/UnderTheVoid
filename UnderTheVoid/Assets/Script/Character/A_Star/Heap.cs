using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IHeapItem<T> : IComparable<T>
{
    int _heapIndex
    {
        get;set;
    }
}
public class Heap<T> where T : IHeapItem<T>
{
    T[] _items;
    int _currentCount;

    public int _count
    {
        get { return _currentCount; }
    }

    public Heap(int maxHeapSize)
    {
        _items = new T[maxHeapSize];
    }

    void Swap(T itemA,T itemB)
    {
        _items[itemA._heapIndex] = itemB;
        _items[itemB._heapIndex] = itemA;
        int itemAIndex = itemA._heapIndex;
        itemA._heapIndex = itemB._heapIndex;
        itemB._heapIndex = itemAIndex;

    }

    void SortUp(T Item)//추가할때 작동하는 함수
    {
        int parentIndex = (Item._heapIndex - 1) / 2;
        while(true)
        {
            T parentItem = _items[parentIndex];
            if (Item.CompareTo(parentItem) > 0)
            {
                Swap(Item, parentItem);
            }
            else
                break;

            parentIndex = (Item._heapIndex - 1) / 2;
        }
    }

    void SortDown(T Item)//뺄때 작동하는 함수
    {
        while(true)
        {
            int childIndexLeft = Item._heapIndex * 2 + 1;
            int childIndexRight = Item._heapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < _currentCount)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight < _currentCount)
                {
                    if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                        swapIndex = childIndexRight;
                }
                if (Item.CompareTo(_items[swapIndex]) < 0)
                    Swap(Item, _items[swapIndex]);
                else
                    return;

            }
            else
                return;
        }
    }

    public void Add(T item)
    {
        item._heapIndex = _currentCount;
        _items[_currentCount] = item;
        SortUp(item);
        _currentCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = _items[0];
        _currentCount--;
        _items[0] = _items[_currentCount];
        _items[0]._heapIndex = 0;

        SortDown(_items[0]);

        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public bool Contains(T item)
    {
        return Equals(_items[item._heapIndex], item);
    }
}
