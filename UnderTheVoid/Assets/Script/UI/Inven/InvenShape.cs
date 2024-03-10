using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class InvenShape
{
    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] bool[] _shape;

    /// <summary>
    /// CTOR
    /// </summary>
    /// <param name="����">The maximum width of the shape</param>
    /// <param name="����">The maximum height of the shape</param>
    public InvenShape(int width, int height)
    {
        _width = width;
        _height = height;
        _shape = new bool[_width * _height];
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="���">A custom shape</param>
    public InvenShape(bool[,] shape)
    {
        _width = shape.GetLength(0);
        _height = shape.GetLength(1);
        _shape = new bool[_width * _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _shape[GetIndex(x, y)] = shape[x, y];
            }
        }
       // _shape.ToArray().Reverse();
    }

    /// <summary>
    /// Returns the width of the shapes bounding box
    /// </summary>
    public int width => _width;

    /// <summary>
    /// Returns the height of the shapes bounding box
    /// </summary>
    public int height => _height;

    /// <summary>
    /// Returns true if given local point is part of this shape
    /// </summary>
    public bool IsPartOfShape(Vector2Int localPoint)
    {
        if (localPoint.x < 0 || localPoint.x >= _width || localPoint.y < 0 || localPoint.y >= _height)
        {
            return false; // outside of shape width/height
        }

        var index = GetIndex(localPoint.x, localPoint.y);
        return _shape[index];
    }

   
    
    /*
    Converts X & Y to an index to use with _shape
    */
    private int GetIndex(int x, int y)
    {
        y = (_height - 1) - y;
        return x + _width * y;
    }
}


public interface IInventoryItem
{
    Sprite sprite { get; }
    Vector2Int position { get; set; }

    int width { get; }
    int height { get; }

    bool IsPartOfShape(Vector2Int localPosition);

    bool canDrop { get; }
}
internal static class InventoryItemExtensions
{
    /// <summary>
    /// Returns the lower left corner position of an item 
    /// within its inventory
    /// </summary>
    internal static Vector2Int GetMinPoint(this IInventoryItem item)
    {
        return item.position;
    }

    /// <summary>
    /// Returns the top right corner position of an item 
    /// within its inventory
    /// </summary>
    internal static Vector2Int GetMaxPoint(this IInventoryItem item)
    {
        return item.position + new Vector2Int(item.width, item.height);
    }

    /// <summary>
    /// �� �������� �κ��丮�ȿ� �ִ� �����۰� ��ġ�� true ��ȯ
    /// </summary>
    internal static bool Contains(this IInventoryItem item, Vector2Int inventoryPoint)
    {
        for (var iX = 0; iX < item.width; iX++)
        {
            for (var iY = 0; iY < item.height; iY++)
            {
                var iPoint = item.position + new Vector2Int(iX, iY);
                if (iPoint == inventoryPoint) { return true; }
            }
        }
        return false;
    }

    /// <summary>
    /// ��ġ�� true ��ȯ
    /// </summary>
    internal static bool Overlaps(this IInventoryItem item, IInventoryItem otherItem)
    {
        for (var iX = 0; iX < item.width; iX++)
        {
            for (var iY = 0; iY < item.height; iY++)
            {
                if (item.IsPartOfShape(new Vector2Int(iX, iY)))
                {
                    var iPoint = item.position + new Vector2Int(iX, iY);
                    for (var oX = 0; oX < otherItem.width; oX++)
                    {
                        for (var oY = 0; oY < otherItem.height; oY++)
                        {
                            if (otherItem.IsPartOfShape(new Vector2Int(oX, oY)))
                            {
                                var oPoint = otherItem.position + new Vector2Int(oX, oY);
                                if (oPoint == iPoint) { return true; } // �׸��� ��ġ�� ���⼭ ����
                            }
                        }
                    }
                }
            }
        }
        return false; // ��ġ�� �κ��� ���ٸ� false����
    }
}