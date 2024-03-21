using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveData
{
    public static void Save(PlayerData _data, string filePath)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filePath, FileMode.Create);

        Debug.Log("세이브 완료" + filePath);
        formatter.Serialize(stream, _data);
        stream.Close();
    }

    public static PlayerData Load(string filePath)
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + filePath);
            return null;
        }
    }
}
[System.Serializable]
public class PlayerData
{
    public string _sprite { get; } // 스프라이트 이름
    public InvenShape _shape { get; }
    public ItemType _type { get; }
    public bool canDrop { get; }
    public int x{ get; }
    public int y{ get; }
    public PlayerData(string sprite, InvenShape shape, ItemType type, bool canDrop, Vector2Int position)
    {
        _sprite = sprite;
        _shape = shape;
        _type = type;
        this.canDrop = canDrop;
        x = position.x;
        y = position.y;
    }
    public PlayerData(IInventoryItem item)
    {
        _sprite = item.sprite.name;
        _shape = item.shape;
        _type = item.Type;
        this.canDrop = item.canDrop;
        x = item.position.x;
        y = item.position.y;
    }
}