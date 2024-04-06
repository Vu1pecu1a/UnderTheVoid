using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class InvenShape
{
    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] bool[] _shape;
    [SerializeField] itemRotae _rotate;
    /// <summary>
    /// ������
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
    /// ���������� ������ �Լ�
    /// </summary>
    public void RotateRight()
    {
        switch (_rotate)
        {
            case itemRotae.up:
                _rotate = itemRotae.right;
                ChangeWH();
                break;
            case (itemRotae)90:
                _rotate = itemRotae.down;
                ChangeWH();
                break;
            case (itemRotae)180:
                _rotate = itemRotae.left;
                ChangeWH();
                break;
            case (itemRotae)(-90):
                _rotate = itemRotae.up;
                ChangeWH();
                break;
        }
    }

    /// <summary>
    /// ���̿� ���̸� ��ȯ
    /// </summary>
    void ChangeWH()
    {
        int h = _height;
        _height = _width;
        _width = h;
    }

    /// <summary>
    /// ����
    /// </summary>
    public itemRotae rotate { get=>_rotate; set=> _rotate = value; }

    /// <summary>
    /// ���α���
    /// </summary>
    public int width => _width;

    /// <summary>
    /// ���α���
    /// </summary>
    public int height => _height;

    /// <summary>
    /// ���� ����Ʈ�� ������ ��ġ�� �κ��� �ִٸ� true��ȯ
    /// </summary>
    public bool IsPartOfShape(Vector2Int localPoint)
    {
        if (localPoint.x < 0 || localPoint.x >= _width || localPoint.y < 0 || localPoint.y >= _height)
        {
            return false; // ���� ���� ���̸� ���
        }
        int index = 0;
        switch (_rotate)
        {
            case (itemRotae)90:
                index = GetIndex90(localPoint.x, localPoint.y);
                return _shape[index];
            case (itemRotae)180:
                index = GetIndex(localPoint.x, localPoint.y);
                bool[] newShape = _shape.ToArray();
                Array.Reverse(newShape);
                return newShape[index];
            case (itemRotae)(-90):
                index = GetIndex90(localPoint.x, localPoint.y);
                bool[] newShape90 = _shape.ToArray();
                Array.Reverse(newShape90);
                return newShape90[index];
            default:
                index = GetIndex(localPoint.x, localPoint.y);
                return _shape[index];
        }
    }
    
    public bool IsPartOfShape90(Vector2Int localPoint)
    {
        if (localPoint.x < 0 || localPoint.x >= _width || localPoint.y < 0 || localPoint.y >= _height)
        {
            return false; // outside of shape width/height
        }

        var index = GetIndex90(localPoint.x, localPoint.y);
        return _shape[index];
    }//�Ⱦ��� �׽�Ʈ �Լ�


    /*
    ���� �Բ� ����� �ε����� X & Y�� ��ȯ
    */
    private int GetIndex(int x, int y)
    {
        y = (_height - 1) - y;//���ΰ�
        return x + _width * y;
    }//��ǥ 0,0 �̸� 0+(7-1)-0 0+6*5�ؼ� 30 1,0 �̸� 31 x�� 5���� �ö󰡼� ���� ������ 35��°[34]�� 4,0
    private int GetIndex90(int x, int y)
    {
        return x *_height +  y ;
    }//��ǥ 0,0 �̸� 0,3,1,4,2,5�� ���;� �ϴ´� ? 
}



internal static class InventoryItemExtensions
{
    /// <summary>
    /// �������� ���� �ϴ� �𼭸� ��ġ�� ��ȯ�մϴ�. 
    /// </summary>
    internal static Vector2Int GetMinPoint(this IInventoryItem item)
    {
        return item.position;
    }

    /// <summary>
    /// �������� ������ ��� �𼭸� ��ġ�� ��ȯ�մϴ�. 
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
                if (item.IsPartOfShape(new Vector2Int(iX, iY)))
                {
                    var iPoint = item.position + new Vector2Int(iX, iY);
                    if (iPoint == inventoryPoint) { return true; }
                }
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