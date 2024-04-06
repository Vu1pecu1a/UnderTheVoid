using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _Name, _Info, _Effect;

    public void SetInfo(IInventoryItem item)
    {
        _Name.text = item.itemData.name;
        _Info.text = item.itemData.iteminfo;
        _Effect.text = item.itemData.itemEffect;
    }
}
