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
    [SerializeField]
    private List<Vector2Int> endrooms = new List<Vector2Int>(); //엔드룸
    [SerializeField]
    private Dictionary<Vector2Int, GameObject> roomsdic = new Dictionary<Vector2Int, GameObject>();//실제 딕셔너리 목록
    public int RoomCount;
    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        Vector2Int startPosition = new Vector2Int(mapWidth / 2, mapHeight / 2); // 시작 위치는 맵의 중심
        GameObject newRoom = Instantiate(roomPrefabs[0], new Vector3(startPosition.x, 0, startPosition.y), Quaternion.identity);
        newRoom.gameObject.GetComponent<Renderer>().material.color = Color.black;
        roomsdic.Add(startPosition, newRoom);
        takenPositions.Add(startPosition); // 시작 위치를 이미 배치된 위치에 추가
        int i = 0;
        StartCoroutine(_newRoom(i,startPosition));//방생성 시도
        // 방 배치
    }
    void NomalroomToEndRoom()
    {
        for (int alfa = 0; alfa < takenPositions.Count; alfa++)
        {
            int end =0;
            for (int i = 0; i < 4; i++)
            {
                if (PosContains(i, takenPositions[alfa]) == true)//주변 방 검사
                    end++;
            }
            if (end == 1)
            {
                if(!endrooms.Contains(takenPositions[alfa]) && alfa!=0)
                endrooms.Add(takenPositions[alfa]);//엔드룸 추가
            }
        }
        foreach (Vector2Int v in endrooms)
        {
            if (takenPositions.Contains(v))
                roomsdic[v].gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        
        if(takenPositions.Count<MinRoom)
        {
            StopCoroutine("_newRoom");
            ResetMap();
        }
    }



    IEnumerator _newRoom(int k,Vector2Int startPosition)
    {
        yield return new WaitForSeconds(0.1f);
        k++;

        int t = takenPositions.Count;

        if (endrooms.Contains(startPosition))
        {
            if (k < takenPositions.Count)//시도 
                StartCoroutine(_newRoom(k, takenPositions[k]));
            else
                NomalroomToEndRoom();
        }

        NewPosition(startPosition + Vector2Int.up); // 방생성 시도
        NewPosition(startPosition + Vector2Int.down);
        NewPosition(startPosition + Vector2Int.left);
        NewPosition(startPosition + Vector2Int.right);
        
        if(t==takenPositions.Count)
        {
          //  k--;
        }

        int end =0;
        
        for (int i = 0; i < 4; i++)
        {
            if (PosContains(i, startPosition) == true)//주변 방 검사
                end++;
        }

        if (end == 1)
        {
            endrooms.Add(startPosition);//엔드룸 추가
            roomsdic[startPosition].gameObject.GetComponent<Renderer>().material.color = Color.red;
        }

        if (takenPositions.Count<MaxRoom && k<100&& takenPositions.Count>k)//시도 
            StartCoroutine(_newRoom(k, takenPositions[k]));
        else if (k<MinRoom)
        {
            StartCoroutine(_newRoom(k, takenPositions[0]));
        }
        else if(MaxRoom<takenPositions.Count)
        {
            NomalroomToEndRoom();
        }
        else
            NomalroomToEndRoom();
    }

    public void ResetMap()
    {
        GameObject[] findall = GameObject.FindGameObjectsWithTag("MapPicea");
        foreach (GameObject a in findall)
        {
            Destroy(a);
        }
        takenPositions.Clear();
        endrooms.Clear();
        roomsdic.Clear();
        GenerateMap();
    }

    void NewPosition(Vector2Int curpos)
    {
        Vector2Int newPos = curpos;
        int attempts = 0;
        
        if(!takenPositions.Contains(newPos))
        {
            for(int i=0;i<4;i++)
            {
                if (PosContains(i, newPos) == true)
                    attempts++;
            }

            if (attempts == 1&&Random.Range(0,9)<5)
            {
                takenPositions.Add(newPos);//방생성 시도
                GameObject newRoom = Instantiate(roomPrefabs[0], new Vector3(newPos.x, 0, newPos.y), Quaternion.identity);
                roomsdic.Add(newPos, newRoom);
            }                
        }
    }
    bool PosContains(int i,Vector2Int newPos)
    {
        switch (i)
        {
            case 0:
                return takenPositions.Contains(newPos+Vector2Int.up);
            case 1:
                return takenPositions.Contains(newPos + Vector2Int.right);
            case 2:
                return takenPositions.Contains(newPos + Vector2Int.down);
            case 3:
                return takenPositions.Contains(newPos + Vector2Int.left);
            default:
                return takenPositions.Contains(newPos);
        }
    }
    Vector2Int RandomDirection(int dir)
    {

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