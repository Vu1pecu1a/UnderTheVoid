using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs; // �� ������ �迭

    public int mapWidth = 10; // ���� ���� ����
    public int mapHeight = 10; // ���� ���� ����
    public int MaxRoom,MinRoom; // �ִ밪 �ּҰ�

    [SerializeField]
    private List<Vector2Int> takenPositions = new List<Vector2Int>(); // �̹� ��ġ�� ��ġ ���

    public int RoomCount;
    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        Vector2Int startPosition = new Vector2Int(mapWidth / 2, mapHeight / 2); // ���� ��ġ�� ���� �߽�

        takenPositions.Add(startPosition); // ���� ��ġ�� �̹� ��ġ�� ��ġ�� �߰�
        int i = 0;
        StartCoroutine(_newRoom(i));
        // �� ��ġ
        // �� ���� ���� �߰� ������ ������ �� ����
    }

    IEnumerator _newRoom(int i)
    {
        yield return new WaitForSeconds(1f);
        i++;
        Vector2Int newPos = NewPosition(); // ���ο� ��ġ�� ã��
        GameObject newRoom = Instantiate(roomPrefabs[0], new Vector3(newPos.x, 0, newPos.y), Quaternion.identity); // �� ����
        takenPositions.Add(newPos);
        if(i < RoomCount)
        StartCoroutine(_newRoom(i));
    }

    Vector2Int NewPosition()
    {
        Vector2Int newPos = Vector2Int.zero;
        int attempts = 0;
        
        if(takenPositions.Contains(newPos) && attempts < 100 && Random.Range(0,9)<5)
        {
            
        }
            // ��ġ�� �̹� ��ġ�� ��ġ�� �ְų� �õ� Ƚ���� 100���� ������ ����

        return newPos;
    }

    Vector2Int RandomDirection()
    {
        int dir = Random.Range(0, 4); // ������ ���� ���� (0: ��, 1: ������, 2: �Ʒ�, 3: ����)

        switch (dir)
        {
            case 0:
                return Vector2Int.up;
            case 1:
                return Vector2Int.right;
            case 2:
                return Vector2Int.down;
            case 3:
                return Vector2Int.left;
            default:
                return Vector2Int.zero;
        }
    }
}