using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenCreat : MonoBehaviour
{
    [SerializeField] private ItemSoltType _slotType = ItemSoltType.Grid;
    [SerializeField] private int _maximumAlowedItemCount = -1;
    [SerializeField] private ItemType _allowedItem = ItemType.Any;
    [SerializeField] private int _width = 8;
    [SerializeField] private int _height = 4;
    [SerializeField] private ItemDefinition[] _definitions = null;
    [SerializeField] private bool _fillRandomly = true; //랜덤으로 아이템 채우기
    [SerializeField] private bool _fillEmpty = false; //꽉 채우기


    void Start()
    {
        var provider = new InventoryProvider(_slotType, _maximumAlowedItemCount, _allowedItem);

        // 인벤토리 생성
        var inventory = new InventoryManager(provider, _width, _height);

        // 랜덤하게 아이템 채우기
        if (_fillRandomly)
        {
            var tries = (_width * _height) / 3;
            for (var i = 0; i < tries; i++)
            {
                inventory.TryAdd(_definitions[Random.Range(0, _definitions.Length)].CreateInstance());
            }
        }

        // 빈 슬롯에 1x1 짜리로 채우기
        if (_fillEmpty)
        {
            for (var i = 0; i < _width * _height; i++)
            {
                inventory.TryAdd(_definitions[0].CreateInstance());
            }
        }

        //  렌더러의 인벤토리가 드로잉을 트리거하도록 설정합니다.
        GetComponent<InvenRender>().SetInventory(inventory, provider.invenSlotType);

        // 바닥에 떨어뜨린 아이템 기록
        inventory.onItemDropped += (item) =>
        {
            Debug.Log((item as ItemDefinition).Name + "(을)를 땅에 버렸다.");
        };

        // 아이템을 바닥에 놓을 수 없는 경우(캔드롭이 거짓으로 설정되어 있음) 기록합니다.
        inventory.onItemDroppedFailed += (item) =>
        {
            Debug.Log($"이 아이템 {(item as ItemDefinition).Name}은(는) 버릴 수 없다");
        };

        // 아이템 추가가 실패했을 경우
        inventory.onItemAddedFailed += (item) =>
        {
            Debug.Log($"이 아이템{(item as ItemDefinition).Name} 는 인벤토리에 들어 갈 수 없습니다.");
        };
    }


    public void DropAll()
    {
        GetComponent<InvenRender>().inventory.DropAll();
    }

    public void A2()
    {
        Debug.Log(GetComponent<InvenRender>().inventory.allItems.Length);
    }
}
