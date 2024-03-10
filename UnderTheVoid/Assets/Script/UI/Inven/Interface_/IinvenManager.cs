using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IinvenManager : IDisposable
{
    /// <summary>
    /// ������ �߰� �׼�
    /// </summary>
    Action<IInventoryItem> onItemAdded { get; set; }

    /// <summary>
    /// ������ �߰��� �Ұ��� �� ���
    /// </summary>
    Action<IInventoryItem> onItemAddedFailed { get; set; }

    /// <summary>
    /// �������� �κ��丮���� ������ ���
    /// </summary>
    Action<IInventoryItem> onItemRemoved { get; set; }

    /// <summary>
    /// �������� �ٴڿ� �����°� ������ ���
    /// </summary>
    Action<IInventoryItem> onItemDropped { get; set; }

    /// <summary>
    /// �������� �ٴڿ� �����°� �����Ͽ��� ���
    /// </summary>
    Action<IInventoryItem> onItemDroppedFailed { get; set; }

    /// <summary>
    /// �κ��丮�� �ٽ� ������ ȣ��
    /// </summary>
    Action onRebuilt { get; set; }

    /// <summary>
    /// �κ��丮 ũ�Ⱑ ���� �� ���
    /// </summary>
    Action onResized { get; set; }

    /// <summary>
    /// �κ��丮 ����
    /// </summary>
    int width { get; }

    /// <summary>
    /// �κ��丮 ����
    /// </summary>
    int height { get; }

    /// <summary>
    /// �κ��丮�� ���̿� ���̸� �缳��
    /// </summary>
    void Resize(int width, int height);

    /// <summary>
    /// �� �κ��丮 ���� ��� �������� ��ȯ.
    /// </summary>
    IInventoryItem[] allItems { get; }

    /// <summary>
    /// ������ �������� �� �κ��丮�� ������ True��ȯ
    /// </summary>
    bool Contains(IInventoryItem item);

    /// <summary>
    /// �κ��丮�� ������ True��ȯ
    /// </summary>
    bool isFull { get; }

    /// <summary>
    /// ������ �������� �߰��� �� ������ True��ȯ
    /// </summary>
    bool CanAdd(IInventoryItem item);

    /// <summary>
    /// ������ �������� �κ��丮�� �߰��Ϸ� �õ��ϰ� ���� �����ߴٸ� True�� ��ȯ
    /// </summary>
    bool TryAdd(IInventoryItem item);

    /// <summary>
    ///  �κ��丮�� Ư�� ��ġ�� �������� �߰��� �� �ִ� ��� true�� ��ȯ
    /// </summary>
    bool CanAddAt(IInventoryItem item, Vector2Int point);

    /// <summary>
    /// ������ �������� �κ��丮�� Ư�� ��ġ�� �߰��Ϸ� �õ��ϰ� ���� �����ߴٸ� True�� ��ȯ
    /// </summary>
    bool TryAddAt(IInventoryItem item, Vector2Int point);

    /// <summary>
    /// �������� ������ �� �ִ� ��� True��ȯ
    /// </summary>
    bool CanRemove(IInventoryItem item);

    /// <summary>
    /// �������� ��ü�� �� �ִ� ��� True��ȯ
    /// </summary>
    bool CanSwap(IInventoryItem item);

    /// <summary>
    /// ������ �������� �κ��丮���� �����Ϸ� �õ��ϰ� ���� �����ߴٸ� True�� ��ȯ
    /// </summary>
    bool TryRemove(IInventoryItem item);

    /// <summary>
    /// �������� ����� �� �ִ� ��� True��ȯ
    /// </summary>
    bool CanDrop(IInventoryItem item);

    /// <summary>
    /// ������ �������� �κ��丮���� ��� �� �����Ϸ� �õ��ϰ� ���� �����ߴٸ� True�� ��ȯ
    /// </summary>
    bool TryDrop(IInventoryItem item);

    /// <summary>
    /// �κ��丮�� ��� �������� ���
    /// </summary>
    void DropAll();

    /// <summary>
    /// �κ��丮�� �ִ� ��� �������� ����
    /// </summary>
    void Clear();

    /// <summary>
    /// �κ��丮 �ٽ� ����
    /// </summary>
    void Rebuild();

    /// <summary>
    /// �� �κ��丮 �� ������ �������� ������ ȹ���ϱ�
    /// </summary>
    IInventoryItem GetAtPoint(Vector2Int point);

    /// <summary>
    /// ������ �簢�� �Ʒ��� ��� �׸��� ��ȯ�մϴ�.
    /// </summary>
    IInventoryItem[] GetAtPoint(Vector2Int point, Vector2Int size);
    
}
