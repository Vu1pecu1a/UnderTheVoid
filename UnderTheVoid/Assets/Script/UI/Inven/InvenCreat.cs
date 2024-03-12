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
    [SerializeField] private bool _fillRandomly = true; //�������� ������ ä���
    [SerializeField] private bool _fillEmpty = false; //�� ä���


    void Start()
    {
        var provider = new InventoryProvider(_slotType, _maximumAlowedItemCount, _allowedItem);

        // �κ��丮 ����
        var inventory = new InventoryManager(provider, _width, _height);

        // �����ϰ� ������ ä���
        if (_fillRandomly)
        {
            var tries = (_width * _height) / 3;
            for (var i = 0; i < tries; i++)
            {
                inventory.TryAdd(_definitions[Random.Range(0, _definitions.Length)].CreateInstance());
            }
        }

        // �� ���Կ� 1x1 ¥���� ä���
        if (_fillEmpty)
        {
            for (var i = 0; i < _width * _height; i++)
            {
                inventory.TryAdd(_definitions[0].CreateInstance());
            }
        }

        //  �������� �κ��丮�� ������� Ʈ�����ϵ��� �����մϴ�.
        GetComponent<InvenRender>().SetInventory(inventory, provider.invenSlotType);

        // �ٴڿ� ����߸� ������ ���
        inventory.onItemDropped += (item) =>
        {
            Debug.Log((item as ItemDefinition).Name + "(��)�� ���� ���ȴ�.");
        };

        // �������� �ٴڿ� ���� �� ���� ���(ĵ����� �������� �����Ǿ� ����) ����մϴ�.
        inventory.onItemDroppedFailed += (item) =>
        {
            Debug.Log($"�� ������ {(item as ItemDefinition).Name}��(��) ���� �� ����");
        };

        // ������ �߰��� �������� ���
        inventory.onItemAddedFailed += (item) =>
        {
            Debug.Log($"�� ������{(item as ItemDefinition).Name} �� �κ��丮�� ��� �� �� �����ϴ�.");
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
