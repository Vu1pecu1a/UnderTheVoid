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
    InventoryManager inventory;

    void Start()
    {
        Debug.Log("�κ��丮 ���� ����");

        var provider = new InventoryProvider(_slotType, _maximumAlowedItemCount, _allowedItem);

        // �κ��丮 ����
        inventory = new InventoryManager(provider, _width, _height);

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

        // �߰��� ������
        inventory.onItemAdded += (item) =>
        {
            if (item is ItemDefinition)
            {
                if (D_calcuate.i.PlayerData.ContainsKey(GetComponent<InvenRender>()))
                {
                    D_calcuate.i.ItemSkill[(item as ItemDefinition).itemData.itemEffect].SkillOn(D_calcuate.i.PlayerData[GetComponent<InvenRender>()]._pb);
                    Debug.Log((item as ItemDefinition).Name + "(��)�� �κ��丮�� �߰��ߴ�." + (item as ItemDefinition).Name +"�� ȿ���� �ߵ��ߴ�");
                }else
                    Debug.Log((item as ItemDefinition).Name + "(��)�� �κ��丮�� �߰��ߴ�.");
            }
            else
            {
                if (D_calcuate.i.PlayerData.ContainsKey(GetComponent<InvenRender>()))
                {
                    D_calcuate.i.ItemSkill[(item as LoadItem).itemData.itemEffect].SkillOn(D_calcuate.i.PlayerData[GetComponent<InvenRender>()]._pb);
                    Debug.Log((item as LoadItem).Name + "(��)�� �κ��丮�� �߰��ߴ�." + (item as LoadItem).Name + "�� ȿ���� �ߵ��ߴ�");
                }else
                {
                    Debug.Log((item as LoadItem).Name + "(��)�� �κ��丮�� �߰��ߴ�.");
                }
            }
        };

        // �ٴڿ� ����߸� ������ ���
        inventory.onItemDropped += (item) =>
        {
            if (item is ItemDefinition)
                Debug.Log((item as ItemDefinition ).Name + "(��)�� ���� ���ȴ�.");
            else
            Debug.Log((item as LoadItem).Name + "(��)�� ���� ���ȴ�.");
        };

        inventory.onItemRemoved += (item) =>
         {
             if (item is ItemDefinition)
                 Debug.Log((item as ItemDefinition).Name + "(��)�� �����ߴ�.");
             else
                 Debug.Log((item as LoadItem).Name + "(��)�� �� �����ߴ�.");
         };

        // �������� �ٴڿ� ���� �� ���� ���(��� �Ұ� üũ�Ǿ����� ���) ���.
        inventory.onItemDroppedFailed += (item) =>
        {
            if(item is ItemDefinition)
            Debug.Log($"�� ������ {(item as ItemDefinition).Name}��(��) ���� �� ����");
            else
            Debug.Log($"�� ������ {(item as LoadItem).Name} + ��(��) ���� �� ����");
        };

        // ������ �߰��� �������� ���
        inventory.onItemAddedFailed += (item) =>
        {

            if (item is ItemDefinition)
                Debug.Log($"�� ������{(item as ItemDefinition).Name} �� �κ��丮�� ��� �� �� �����ϴ�.");
            else
            Debug.Log($"�� ������{(item as LoadItem).Name} + �� �κ��丮�� ��� �� �� �����ϴ�.");
        };

        if(D_calcuate.i.PlayerData.ContainsKey(gameObject.GetComponent<InvenRender>()))
        {
            InvenRender R = gameObject.GetComponent<InvenRender>();
            AddStartItem(R, D_calcuate.i.PlayerData[R]._pb);
            gameObject.transform.parent.parent.gameObject.SetActive(false);
            Managers.instance._C.PlayerStartSkillAdd(D_calcuate.i.PlayerData[R]._pb, 2, D_calcuate.i.AllPassiveSkill[D_calcuate.i.PlayerData[R]._pb._StartSkill]);
        }
    }
    void AddStartItem(InvenRender eqren0, PlayerBase pb)
    {
        //eqren0.transform.parent.parent.gameObject.SetActive(true);
        if (pb._definitions.Length != 0)
            eqren0.gameObject.GetComponent<InvenCreat>().ItemADD(pb._definitions[0]);

    }
    public void ItemADD(ItemDefinition _def)
    {
        inventory.TryAdd(_def.CreateInstance());
    }

    public void DropAll()
    {
        GetComponent<InvenRender>().inventory.DropAll();
    }
    public void _LoadItem()
    {
        PlayerData data = SaveData.Load(Application.streamingAssetsPath + "/1.bin");

        inventory.TryAdd(new LoadItem(data));
    }
    public void A2()
    {
        Debug.Log(GetComponent<InvenRender>().inventory.allItems.Length);
    }
}
