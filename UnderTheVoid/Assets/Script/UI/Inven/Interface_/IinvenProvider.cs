using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IinvenProvider 
{
    /// <summary>
    /// 이 인벤토리의 슬롯 타입을 반환
    /// </summary>
    ItemSoltType invenSlotType { get; }

    /// <summary>
    /// 이 인벤토리에 있는 아이템의 총량을 반환
    /// </summary>
    int inventoryItemCount { get; }

    /// <summary>
    ///인벤토리가 꽉 찼을 경우 true반환
    /// </summary>
    bool isInventoryFull { get; }

    /// <summary>
    /// 지정된 값을 인벤토리에 반환
    /// </summary>
    IInventoryItem GetInventoryItem(int index);

    /// <summary>
    /// 인벤토리에 지정된 타입의 아이템이 들어갈 수 있을 경우 true반환
    /// </summary>
    bool CanAddInventoryItem(IInventoryItem item);

    /// <summary>
    /// 주어진 인벤토리 항목이 다음을 수행할 수 있는 경우 참을 반환
    /// 인벤토리에서 제거가 가능할 경우
    /// </summary>
    bool CanRemoveInventoryItem(IInventoryItem item);

    /// <summary>
    /// 주어진 인벤토리 항목이 다음을 수행할 수 있는 경우 참을 반환
    /// 이벤토리에서 버리기가 가능할 경우
    /// </summary>
    bool CanDropInventoryItem(IInventoryItem item);

    /// <summary>
    /// 인벤토리 아이템이 추가될 때 호출
    /// 성공하면 true반환
    /// </summary>
    bool AddInventoryItem(IInventoryItem item);

    /// <summary>
    /// 인벤토리에서 아이템을 제거할 때 호출
    /// 성공하면 true반환
    /// </summary>
    bool RemoveInventoryItem(IInventoryItem item);

    /// <summary>
    /// 인벤토리에서 아이템을 제거할 때 호출
    /// 땅에 버리는것이
    /// 성공하면 true반환
    /// </summary>
    bool DropInventoryItem(IInventoryItem item);
}
