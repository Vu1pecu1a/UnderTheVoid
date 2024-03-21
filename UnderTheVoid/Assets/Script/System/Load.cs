using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour
{
    private PlayerData data;

    private string filePath = string.Empty;

    [SerializeField]
    InvenShape i;

    [SerializeField]
    Vector2Int a;
    void Awake()
    {
        filePath = Application.streamingAssetsPath + "/Uk.bin";
    }

    public void Save()
    {
        data = new PlayerData("7",i,ItemType.relic,true, a);
        SaveData.Save(data, filePath);
    }

    public void LoadFile()
    {
        data = SaveData.Load(filePath);
        Debug.Log(data._sprite);
    }
}
