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
    /// ������ ȿ��
    /// </summary>
    string Itemability { get; set; }
    /// <summary>
    /// �־��� ���� ����Ʈ�� �� ������ �Ϻ��� ��� ���� ��ȯ�մϴ�.
    /// </summary>
    bool IsPartOfShape(Vector2Int localPosition);
    void RotateRight();
    void RotateOrigin(itemRotae ro);
}