using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs; // 방 프리팹 배열

    public int mapWidth = 10; // 맵의 가로 길이
    public int mapHeight = 10; // 맵의 세로 길이
    public int MaxRoom,MinRoom; // 최대값 최소값

    [SerializeField]
    private List<Vector2Int> takenPositions = new List<Vector2Int>(); // 이미 배치된 위치 목록

    public int RoomCount;
    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        Vector2Int startPosition = new Vector2Int(mapWidth / 2, mapHeight / 2); // 시작 위치는 맵의 중심

        takenPositions.Add(startPosition); // 시작 위치를 이미 배치된 위치에 추가
        int i = 0;
        StartCoroutine(_newRoom(i));
        // 방 배치
        // 방 연결 등의 추가 로직을 구현할 수 있음
    }

    IEnumerator _newRoom(int i)
    {
        yield return new WaitForSeconds(1f);
        i++;
        Vector2Int newPos = NewPosition(); // 새로운 위치를 찾음
        GameObject newRoom = Instantiate(roomPrefabs[0], new Vector3(newPos.x, 0, newPos.y), Quaternion.identity); // 방 생성
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
            // 위치가 이미 배치된 위치에 있거나 시도 횟수가 100번을 넘으면 중지

        return newPos;
    }

    Vector2Int RandomDirection()
    {
        int dir = Random.Range(0, 4); // 랜덤한 방향 선택 (0: 위, 1: 오른쪽, 2: 아래, 3: 왼쪽)

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