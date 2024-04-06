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
    /// 생성자
    /// </summary>
    /// <param name="높이">The maximum width of the shape</param>
    /// <param name="넓이">The maximum height of the shape</param>
    public InvenShape(int width, int height)
    {
        _width = width;
        _height = height;
        _shape = new bool[_width * _height];
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="모양">A custom shape</param>
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
    /// 오른쪽으로 돌리는 함수
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
    /// 높이와 넓이를 교환
    /// </summary>
    void ChangeWH()
    {
        int h = _height;
        _height = _width;
        _width = h;
    }

    /// <summary>
    /// 방향
    /// </summary>
    public itemRotae rotate { get=>_rotate; set=> _rotate = value; }

    /// <summary>
    /// 가로길이
    /// </summary>
    public int width => _width;

    /// <summary>
    /// 세로길이
    /// </summary>
    public int height => _height;

    /// <summary>
    /// 로컬 포인트가 도형과 겹치는 부분이 있다면 true반환
    /// </summary>
    public bool IsPartOfShape(Vector2Int localPoint)
    {
        if (localPoint.x < 0 || localPoint.x >= _width || localPoint.y < 0 || localPoint.y >= _height)
        {
            return false; // 가로 세로 길이를 벗어남
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
    }//안쓰는 테스트 함수


    /*
    모양과 함께 사용할 인덱스로 X & Y를 변환
    */
    private int GetIndex(int x, int y)
    {
        y = (_height - 1) - y;//세로값
        return x + _width * y;
    }//좌표 0,0 이면 0+(7-1)-0 0+6*5해서 30 1,0 이면 31 x가 5까지 올라가서 최종 적으로 35번째[34]는 4,0
    private int GetIndex90(int x, int y)
    {
        return x *_height +  y ;
    }//좌표 0,0 이면 0,3,1,4,2,5가 나와야 하는대 ? 
}



internal static class InventoryItemExtensions
{
    /// <summary>
    /// 아이템의 왼쪽 하단 모서리 위치를 반환합니다. 
    /// </summary>
    internal static Vector2Int GetMinPoint(this IInventoryItem item)
    {
        return item.position;
    }

    /// <summary>
    /// 아이템의 오른쪽 상단 모서리 위치를 반환합니다. 
    /// </summary>
    internal static Vector2Int GetMaxPoint(this IInventoryItem item)
    {
        return item.position + new Vector2Int(item.width, item.height);
    }

    /// <summary>
    /// 이 아이템이 인벤토리안에 있는 아이템과 겹치면 true 반환
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
    /// 겹치면 true 반환
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
                                if (oPoint == iPoint) { return true; } // 항목이 겹치면 여기서 리턴
                            }
                        }
                    }
                }
            }
        }
        return false; // 겹치는 부분이 없다면 false리턴
    }


}