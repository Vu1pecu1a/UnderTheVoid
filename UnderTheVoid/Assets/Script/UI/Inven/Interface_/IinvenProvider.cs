using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IinvenProvider 
{
    /// <summary>
    /// �� �κ��丮�� ���� Ÿ���� ��ȯ
    /// </summary>
    ItemSoltType invenSlotType { get; }

    /// <summary>
    /// �� �κ��丮�� �ִ� �������� �ѷ��� ��ȯ
    /// </summary>
    int inventoryItemCount { get; }

    /// <summary>
    ///�κ��丮�� �� á�� ��� true��ȯ
    /// </summary>
    bool isInventoryFull { get; }

    /// <summary>
    /// ������ ���� �κ��丮�� ��ȯ
    /// </summary>
    IInventoryItem GetInventoryItem(int index);

    /// <summary>
    /// �κ��丮�� ������ Ÿ���� �������� �� �� ���� ��� true��ȯ
    /// </summary>
    bool CanAddInventoryItem(IInventoryItem item);

    /// <summary>
    /// �־��� �κ��丮 �׸��� ������ ������ �� �ִ� ��� ���� ��ȯ
    /// �κ��丮���� ���Ű� ������ ���
    /// </summary>
    bool CanRemoveInventoryItem(IInventoryItem item);

    /// <summary>
    /// �־��� �κ��丮 �׸��� ������ ������ �� �ִ� ��� ���� ��ȯ
    /// �̺��丮���� �����Ⱑ ������ ���
    /// </summary>
    bool CanDropInventoryItem(IInventoryItem item);

    /// <summary>
    /// �κ��丮 �������� �߰��� �� ȣ��
    /// �����ϸ� true��ȯ
    /// </summary>
    bool AddInventoryItem(IInventoryItem item);

    /// <summary>
    /// �κ��丮���� �������� ������ �� ȣ��
    /// �����ϸ� true��ȯ
    /// </summary>
    bool RemoveInventoryItem(IInventoryItem item);

    /// <summary>
    /// �κ��丮���� �������� ������ �� ȣ��
    /// ���� �����°���
    /// �����ϸ� true��ȯ
    /// </summary>
    bool DropInventoryItem(IInventoryItem item);
}
