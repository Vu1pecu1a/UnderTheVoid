using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IinvenManager : IDisposable
{
    /// <summary>
    /// 아이템 추가 액션
    /// </summary>
    Action<IInventoryItem> onItemAdded { get; set; }

    /// <summary>
    /// 아이템 추가가 불가능 할 경우
    /// </summary>
    Action<IInventoryItem> onItemAddedFailed { get; set; }

    /// <summary>
    /// 아이템을 인벤토리에서 제거할 경우
    /// </summary>
    Action<IInventoryItem> onItemRemoved { get; set; }

    /// <summary>
    /// 아이템을 바닥에 버리는게 가능할 경우
    /// </summary>
    Action<IInventoryItem> onItemDropped { get; set; }

    /// <summary>
    /// 아이템을 바닥에 버리는걸 실패하였을 경우
    /// </summary>
    Action<IInventoryItem> onItemDroppedFailed { get; set; }

    /// <summary>
    /// 인벤토리를 다시 만들경우 호출
    /// </summary>
    Action onRebuilt { get; set; }

    /// <summary>
    /// 인벤토리 크기가 변경 될 경우
    /// </summary>
    Action onResized { get; set; }

    /// <summary>
    /// 인벤토리 넓이
    /// </summary>
    int width { get; }

    /// <summary>
    /// 인벤토리 높이
    /// </summary>
    int height { get; }

    /// <summary>
    /// 인벤토리의 넓이와 높이를 재설정
    /// </summary>
    void Resize(int width, int height);

    /// <summary>
    /// 이 인벤토리 내의 모든 아이템을 반환.
    /// </summary>
    IInventoryItem[] allItems { get; }

    /// <summary>
    /// 지정된 아이템이 이 인벤토리에 있으면 True반환
    /// </summary>
    bool Contains(IInventoryItem item);

    /// <summary>
    /// 인벤토리가 꽉차면 True반환
    /// </summary>
    bool isFull { get; }

    /// <summary>
    /// 지정된 아이템을 추가할 수 있으면 True반환
    /// </summary>
    bool CanAdd(IInventoryItem item);

    /// <summary>
    /// 지정된 아이템을 인벤토리에 추가하려 시도하고 만약 성공했다면 True를 반환
    /// </summary>
    bool TryAdd(IInventoryItem item);

    /// <summary>
    ///  인벤토리의 특정 위치에 아이템을 추가할 수 있는 경우 true를 반환
    /// </summary>
    bool CanAddAt(IInventoryItem item, Vector2Int point);

    /// <summary>
    /// 지정된 아이템을 인벤토리의 특정 위치로 추가하려 시도하고 만약 성공했다면 True를 반환
    /// </summary>
    bool TryAddAt(IInventoryItem item, Vector2Int point);

    /// <summary>
    /// 아이템을 제거할 수 있는 경우 True반환
    /// </summary>
    bool CanRemove(IInventoryItem item);

    /// <summary>
    /// 아이템을 교체할 수 있는 경우 True반환
    /// </summary>
    bool CanSwap(IInventoryItem item);

    /// <summary>
    /// 지정된 아이템을 인벤토리에서 제거하려 시도하고 만약 성공했다면 True를 반환
    /// </summary>
    bool TryRemove(IInventoryItem item);

    /// <summary>
    /// 아이템을 드랍할 수 있는 경우 True반환
    /// </summary>
    bool CanDrop(IInventoryItem item);

    /// <summary>
    /// 지정된 아이템을 인벤토리에서 드랍 후 제거하려 시도하고 만약 성공했다면 True를 반환
    /// </summary>
    bool TryDrop(IInventoryItem item);

    /// <summary>
    /// 인벤토리의 모든 아이템을 드랍
    /// </summary>
    void DropAll();

    /// <summary>
    /// 인벤토리에 있는 모든 아이템을 제거
    /// </summary>
    void Clear();

    /// <summary>
    /// 인벤토리 다시 생성
    /// </summary>
    void Rebuild();

    /// <summary>
    /// 이 인벤토리 내 지정된 지점에서 아이템 획득하기
    /// </summary>
    IInventoryItem GetAtPoint(Vector2Int point);

    /// <summary>
    /// 지정된 사각형 아래의 모든 항목을 반환합니다.
    /// </summary>
    IInventoryItem[] GetAtPoint(Vector2Int point, Vector2Int size);
    
}
