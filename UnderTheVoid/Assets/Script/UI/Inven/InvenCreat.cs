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
    InventoryManager inventory;

    void Start()
    {
        Debug.Log("인벤토리 생성 시작");

        var provider = new InventoryProvider(_slotType, _maximumAlowedItemCount, _allowedItem);

        // 인벤토리 생성
        inventory = new InventoryManager(provider, _width, _height);

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

        // 추가된 아이템
        inventory.onItemAdded += (item) =>
        {
            if (item is ItemDefinition)
            {
                if (D_calcuate.i.PlayerData.ContainsKey(GetComponent<InvenRender>()))
                {
                    D_calcuate.i.ItemSkill[(item as ItemDefinition).itemData.itemEffect].SkillOn(D_calcuate.i.PlayerData[GetComponent<InvenRender>()]._pb);
                    Debug.Log((item as ItemDefinition).Name + "(을)를 인벤토리에 추가했다." + (item as ItemDefinition).Name +"의 효과가 발동했다");
                }else
                    Debug.Log((item as ItemDefinition).Name + "(을)를 인벤토리에 추가했다.");
            }
            else
            {
                if (D_calcuate.i.PlayerData.ContainsKey(GetComponent<InvenRender>()))
                {
                    D_calcuate.i.ItemSkill[(item as LoadItem).itemData.itemEffect].SkillOn(D_calcuate.i.PlayerData[GetComponent<InvenRender>()]._pb);
                    Debug.Log((item as LoadItem).Name + "(을)를 인벤토리에 추가했다." + (item as LoadItem).Name + "의 효과가 발동했다");
                }else
                {
                    Debug.Log((item as LoadItem).Name + "(을)를 인벤토리에 추가했다.");
                }
            }
        };

        // 바닥에 떨어뜨린 아이템 기록
        inventory.onItemDropped += (item) =>
        {
            if (item is ItemDefinition)
                Debug.Log((item as ItemDefinition ).Name + "(을)를 땅에 버렸다.");
            else
            Debug.Log((item as LoadItem).Name + "(을)를 땅에 버렸다.");
        };

        inventory.onItemRemoved += (item) =>
         {
             if (item is ItemDefinition)
                 Debug.Log((item as ItemDefinition).Name + "(을)를 제거했다.");
             else
                 Debug.Log((item as LoadItem).Name + "(을)를 를 제거했다.");
         };

        // 아이템을 바닥에 놓을 수 없는 경우(드랍 불가 체크되어있을 경우) 기록.
        inventory.onItemDroppedFailed += (item) =>
        {
            if(item is ItemDefinition)
            Debug.Log($"이 아이템 {(item as ItemDefinition).Name}은(는) 버릴 수 없다");
            else
            Debug.Log($"이 아이템 {(item as LoadItem).Name} + 은(는) 버릴 수 없다");
        };

        // 아이템 추가가 실패했을 경우
        inventory.onItemAddedFailed += (item) =>
        {

            if (item is ItemDefinition)
                Debug.Log($"이 아이템{(item as ItemDefinition).Name} 는 인벤토리에 들어 갈 수 없습니다.");
            else
            Debug.Log($"이 아이템{(item as LoadItem).Name} + 는 인벤토리에 들어 갈 수 없습니다.");
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
