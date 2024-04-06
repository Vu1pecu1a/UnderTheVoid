using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �������̽�
/// </summary>
public interface IInventoryItem
{
    Sprite sprite { get; }
    Vector2Int position { get; set; }
    ItemType Type { get; }
    InvenShape shape { get; }
    ItemData itemData { get; }
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

public class ItemData
{
    public string name;
    public string iteminfo;
    public string itemEffect;
}