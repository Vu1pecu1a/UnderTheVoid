using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem
{
    Sprite sprite { get; }
    Vector2Int position { get; set; }
    ItemType Type { get; }
    InvenShape shape { get; }
    int width { get; }
    int height { get; }
    bool canDrop { get; }
    itemRotae Rotate { get; set; }
    /// <summary>
    /// 아이템 효과
    /// </summary>
    string Itemability { get; set; }
    /// <summary>
    /// 주어진 로컬 포인트가 이 도형의 일부인 경우 참을 반환합니다.
    /// </summary>
    bool IsPartOfShape(Vector2Int localPosition);
    void RotateRight();
    void RotateOrigin(itemRotae ro);
}