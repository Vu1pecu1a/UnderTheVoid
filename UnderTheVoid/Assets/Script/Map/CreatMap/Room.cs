using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool Endroom = false;
    public Vector2 roomVector2;
    public int RoomCode= 0;
    public bool isClear = false;

    GameObject newRoom;
    // Start is called before the first frame update
    void Awake()
    {
        MapGenerator.i.mapInstanceEvent += Ins;
    }

    private void OnDestroy()
    {
        MapGenerator.i.mapInstanceEvent -= Ins;
    }

    void Ins()
    {
        if(!Endroom)
        newRoom = Instantiate(MapGenerator.i.Room[RoomCode], MapGenerator.i.transform);
        else
        newRoom = Instantiate(MapGenerator.i.SpecialRoom[RoomCode], MapGenerator.i.transform);

        newRoom.SetActive(false);
    }

    

    public void SetFiled(bool a)
    {
        newRoom.SetActive(a);
    }
}
