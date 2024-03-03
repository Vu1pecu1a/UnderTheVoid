using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public enum RoomType
{
    Normal,
    Boss,
    Gold,
    Max
}


public class MapGenerator : MonoBehaviour // 방관련 함수는 전부 여기서 처리
{
    #region[설정값]
    [SerializeField]
    Sprite[] images;
    public GameObject[] roomPrefabs,Room,SpecialRoom,Monstergen; // 방 프리팹 배열
    [SerializeField]
    public static Vector2Int PlayerV2;//플레이어 위치
    [SerializeField]
    float MapSacle = 1;
    public int mapWidth = 10; // 맵의 가로 길이
    public int mapHeight = 10; // 맵의 세로 길이
    public int MaxRoom,MinRoom; // 최대값 최소값
    Node[,] _grid;
    [SerializeField]
    GameObject[] Doors;

    [SerializeField]
    int tilex = 100, tiley=100;
    [SerializeField]
    private List<Vector2Int> takenPositions = new List<Vector2Int>(); // 이미 배치된 위치 목록
    [SerializeField]
    private List<Vector2Int> endrooms = new List<Vector2Int>(); //엔드룸
    private List<Vector2Int> specialrooms = new List<Vector2Int>(); //엔드룸
    [SerializeField]
    private Dictionary<Vector2Int, GameObject> roomsdic = new Dictionary<Vector2Int, GameObject>();//미니맵 딕셔너리 목록 
    [SerializeField]
    Vector2[] _path;
    [SerializeField]
    Vector2Int curendrooms;
    public Dictionary<Vector2Int, int> endroomsDistance = new Dictionary<Vector2Int, int>();//끝방 거리 리스트
    public static MapGenerator i;

    public delegate void MapInstance();
    public event MapInstance mapInstanceEvent;

    int Pathcount = 0;
    public int RoomCount;

    [SerializeField]
    GameObject Canvas,Loding;
    #endregion[설정값]
    #region [맵 생성 함수]
    private void Awake()
    {
        i = this;
        CreatNode();
        Loding.SetActive(true);
        StartCoroutine(LodingText(0));
    }
    void Start()
    {
        mapInstanceEvent += Debug_error_Event;
        GenerateMap();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayerMoveToMap(2);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayerMoveToMap(1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayerMoveToMap(3);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayerMoveToMap(0);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            RoomClearTrue();
        }
    }

    public void RoomClearTrue()
    {
        i.roomsdic[PlayerV2].GetComponent<Room>().isClear = true;
        i.IsClearRoom();
    }//방 클리어 판정

    public int _maxSize
    {
        get { return mapHeight*mapWidth; }
    }

    IEnumerator LodingText(int i)
    {
        i++;
        if (i > 9) i = 0;
        yield return new WaitForSeconds(0.1f);
        Loding.transform.GetChild(0).GetComponent<TMP_Text>().text = "로딩중" + new string('.',i);
        StartCoroutine(LodingText(i));
    }

    


    void GenerateMap()
    {
        endrooms.Clear();
        Vector2Int startPosition = new Vector2Int(mapWidth / 2, mapHeight / 2); // 시작 위치는 맵의 중심
        GameObject newRoom = RoomCreat(startPosition);
        newRoom.GetComponent<Image>().color = Color.white;
        newRoom.GetComponent<Room>().RoomCode = 0;
        roomsdic.Add(startPosition, newRoom);
        takenPositions.Add(startPosition); // 시작 위치를 이미 배치된 위치에 추가
        int i = 0;
        int t = 0;
        StartCoroutine(_newRoom(i,t,startPosition));//방생성 시도
        // 방 배치
    }
    public void OnPathFound(Vector2[] newPath, bool pathSuccessful)
    {
        
        if (pathSuccessful)
        {
            _path = newPath;
            endroomsDistance.Add(endrooms[Pathcount],_path.Length);
            Pathcount++;
            //Debug.Log(endroomsDistance.Count);
            if(endroomsDistance.Count == endrooms.Count)
            {
                int Max = 0;
                Vector2Int vi = Vector2Int.zero;
               foreach(Vector2Int a in endrooms)
                {
                    if (Max < endroomsDistance[a])
                    {
                        vi = a;
                        Max = endroomsDistance[a];
                    }
                }
                EndroomToBoss(vi,Max);
            }
        }
    }//출발지점에서 가장 멀리있는 엔드룸을 찾는 길찾기 함수

    void EndroomToBoss(Vector2Int Vi2,int Max)
    {
        _grid[Vi2.x, Vi2.y].RoomType = RoomType.Boss;
        GameObject Room = roomsdic[Vi2].gameObject;
        //Room.GetComponent<Image>().color = new Color(1, 0, 0, 0);
        Room.name = "BOSS";
        Room.transform.GetChild(0).gameObject.SetActive(true);
        Room.transform.GetChild(0).GetComponent<Image>().sprite = null;
        Room.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 0, 0, 0);
        Room.transform.GetChild(1).GetComponent<TMP_Text>().text = "BOSS"+"\n"+Max;
        Room.GetComponent<Room>().RoomCode = 1;
        Room.GetComponent<Room>().Endroom = true;
        endrooms.Remove(Vi2);
        specialrooms.Add(Vi2);
        Debug.Log("보스방");
        EndroomToGoldenRoom(Random.Range(0, endrooms.Count));
    }//보스룸으로 변경

    void EndroomToGoldenRoom(int Vi2)
    {
        _grid[endrooms[Vi2].x, endrooms[Vi2].y].RoomType = RoomType.Gold;
        GameObject Room = roomsdic[endrooms[Vi2]].gameObject;
        // Room.GetComponent<Image>().color = new Color(1, 0.92f, 0.16f, 0);
        Room.name = "Gold";
        Room.transform.GetChild(1).GetComponent<TMP_Text>().text = "Gold";
        Room.transform.GetChild(0).gameObject.SetActive(true);
        Room.transform.GetChild(0).GetComponent<Image>().sprite = null;
        Room.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 0.92f, 0.16f, 0);
        Room.GetComponent<Room>().RoomCode = 0;
        Room.GetComponent<Room>().Endroom = true;
        specialrooms.Add(endrooms[Vi2]);
        endrooms.Remove(endrooms[Vi2]);
        mapInstanceEvent();
        Debug.Log("황금방");
        PlayerVector2Set(_grid[mapWidth/2,mapWidth/2]);//중앙지점
        Minimap(true);
        Loding.SetActive(false);
    }//황금방으로 변경

    void Debug_error_Event()
    {
        Debug.Log("이벤트 시작");
    }

    void PlayerVector2Set(Node node)
    {
        PlayerV2 = new Vector2Int(node._gX, node._gY);
        roomsdic[PlayerV2].transform.GetComponent<Image>().sprite = images[0];
        roomsdic[PlayerV2].transform.GetChild(0).gameObject.SetActive(true);
        roomsdic[PlayerV2].transform.GetChild(0).GetComponent<Image>().sprite = images[2];
        List<Node> Neighboirs = GetNeighbours1234(node);
        for(int i = 0;i< Neighboirs.Count;i++)
        {
            if (takenPositions.Contains(Rtv2(Neighboirs[i])))
            {
                Image alfa = roomsdic[Rtv2(Neighboirs[i])].GetComponent<Image>();
                Image beta = roomsdic[Rtv2(Neighboirs[i])].transform.GetChild(0).GetComponent<Image>();
                alfa.color = Color255(alfa.color);
                beta.color = Color255(beta.color);
            }

            if (Neighboirs[i]._walkable == false || Neighboirs[i].Equals(node))//갈 수 없거나 자기 자신일 경우
                Doors[i].SetActive(false);
            else
                Doors[i].SetActive(true);

            if (Neighboirs[i].RoomType.Equals(RoomType.Boss))
                Doors[i].GetComponent<Renderer>().material.color = Color.red;
            else if (Neighboirs[i].RoomType.Equals(RoomType.Gold))
                Doors[i].GetComponent<Renderer>().material.color = Color.yellow;
            else
                Doors[i].GetComponent<Renderer>().material.color = Color.white;
        }
        IsClearRoom();//방 진입시 출구 체크
        roomsdic[PlayerV2].GetComponent<Room>().SetFiled(true);   
        roomsdic[PlayerV2].GetComponent<Room>().SetEnemy();
        MiniMapMove();
        D_calcuate.i.BattelStart();
    }//맵상에서의 플레이어 노드 상 좌표 변경 [방 입장 판정]
    
    // 5,5면 0,0?
    
    Vector2Int Rtv2(Node node)//노드를 Vector2Int로 바꿔주는 함수
    {
        Vector2Int alfa = new Vector2Int(node._gX, node._gY);
        return alfa;
    }

    Node Rtnode(Vector2Int v2)
    {
        Node node = _grid[v2.x,v2.y];
        return node;
    }//Vector2Int를 Node로 바꿔주는 함수

    Color Color255(Color tmpcolor)
    {
        Color curColor = new Color(tmpcolor.r, tmpcolor.g, tmpcolor.b, 255);
        tmpcolor = curColor;
        return tmpcolor;
    }//알파값이 0인 색을 255로 돌려주는 함수
    
    void Hidemap()
    {
        roomsdic[PlayerV2].transform.GetComponent<Image>().sprite = images[0];//이미지 초기화
        roomsdic[PlayerV2].transform.GetChild(0).gameObject.SetActive(false);
        roomsdic[PlayerV2].transform.GetChild(0).GetComponent<Image>().sprite = null;
        roomsdic[PlayerV2].GetComponent<Room>().SetFiled(false);
    }

    public void IsClearRoom()
    {
        if (roomsdic[PlayerV2].GetComponent<Room>().isClear == false)
            Doors[0].transform.parent.gameObject.SetActive(false);
        else
            Doors[0].transform.parent.gameObject.SetActive(true);
    }
    public void PlayerMoveToMap(int i)
    {
        Vector2Int v = PlayerV2;
        Hidemap();
        List<Node> Neighboirs = GetNeighbours1234(_grid[v.x,v.y]);//up,left,right,down
        if (Neighboirs[i]._walkable == true)
        {
            PlayerVector2Set(Neighboirs[i]);
        }
        else//갈곳이 없으면 현재자리로 다시 이동
            PlayerVector2Set(_grid[v.x, v.y]);
    }//플레이어 위치 이동[연결된곳]

    Vector2Int retrunV2(int i)
    {
        switch(i)
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

    void NomalroomToEndRoom()
    {
        foreach(Vector2Int v in takenPositions)
        {
            endroomcheck(v);
        }
        
        if (takenPositions.Count<MinRoom || endrooms.Count<2)//방수가 최소치 이하거나 엔드룸 생성이 2개 이하일 경우 방 리롤
        {
            ResetMap();
        }else // 생성조건이 모두 만족했을경우 Node생성
        {
            foreach(Vector2Int a in takenPositions)
            {
                _grid[a.x,a.y]._walkable= true;
                _grid[a.x, a.y].RoomType = RoomType.Normal;
            }
            foreach (Vector2Int v in endrooms)
            {
                if (takenPositions.Contains(v))
                {
                    PathRequestManager.RequestPath(v, takenPositions[0], OnPathFound);
                }
            }
        }
    }//방생성 마지막 단계
    public Node NodeFormCanvas(Vector2Int alfa)
    {
        return _grid[alfa.x,alfa.y ] ;
    }
    void CreatNode()
    {
        _grid = new Node[mapWidth, mapHeight];
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector2Int alfa = new Vector2Int(x, y);
                //bool walkable = takenPositions.Contains(alfa);
                bool walkable = false;
                int movePenealty = 0;

                _grid[x, y] = new Node(walkable, Vector3.zero, x, y, movePenealty);
            }
        }
    }
    IEnumerator _newRoom(int k,int t,Vector2Int startPosition)
    {
        Color prevColor = roomsdic[startPosition].GetComponent<Image>().color;
        roomsdic[startPosition].GetComponent<Image>().color = Color.blue;
        yield return new WaitForSeconds(0.001f);
        k++;//시도 횟수
        t++;//방 번호 찾기
        if (endrooms.Contains(startPosition))
        {
            roomsdic[startPosition].GetComponent<Image>().color = prevColor;
            if ( takenPositions.Count<k&& takenPositions.Count > t)//시도 
                StartCoroutine(_newRoom(k, t, takenPositions[t]));
            else
                NomalroomToEndRoom();
        }//시작점이 엔드룸이면 생성 시도 넘기기
        else
        {
            NewPosition(startPosition + Vector2Int.up); // 방생성 시도
            NewPosition(startPosition + Vector2Int.down);
            NewPosition(startPosition + Vector2Int.left);
            NewPosition(startPosition + Vector2Int.right);

            roomsdic[startPosition].GetComponent<Image>().color = prevColor;
            endroomcheck(startPosition);

            if (takenPositions.Count < MaxRoom && k < RoomCount && takenPositions.Count > t)//시도 
                StartCoroutine(_newRoom(k, t, takenPositions[t]));
            else if (takenPositions.Count < MinRoom)//방 생성 갯수가 최소치보다 작으면
            {
                t = 0;//0부터 시작하기
                StartCoroutine(_newRoom(k, t, takenPositions[t]));
            }
            else if (MaxRoom < takenPositions.Count)
            {
                NomalroomToEndRoom();
            }
            else
                NomalroomToEndRoom();
        }
    }

    void endroomcheck(Vector2Int startPosition)//엔드룸 체크
    {
        int end = 0;

        for (int i = 0; i < 4; i++)
        {
            if (PosContains(i, startPosition) == true)//주변 방 검사
                end++;
        }

        if (end == 1 && startPosition != takenPositions[0] && !endrooms.Contains(startPosition))
        {
            endrooms.Add(startPosition);//엔드룸 추가
            //roomsdic[startPosition].gameObject.GetComponent<Renderer>().material.color = Color.red;
            //roomsdic[startPosition].GetComponent<Image>().color = new Color(0, 1, 1, 0);
        }
    }

    public void ResetMap()//맵 리셋 함수
    {
       // GameObject[] findall = GameObject.FindGameObjectsWithTag("MapPicea");
        foreach (Vector2Int a in takenPositions)
        {
            Destroy(roomsdic[a]);
        }
        StopAllCoroutines();
        StartCoroutine(LodingText(0));
        takenPositions.Clear();
        endrooms.Clear();
        roomsdic.Clear();
        endroomsDistance.Clear();
        specialrooms.Clear();
        Pathcount=0;
        GenerateMap();
    }

    void NewPosition(Vector2Int curpos) // 방 생성 함수
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

            if (attempts == 1&&Random.Range(0,9)<5&& curpos.x<mapWidth &&curpos.x>0 && curpos.y <mapWidth &&curpos.y>0)
            {
                takenPositions.Add(newPos);//방생성 시도
                GameObject newRoom = RoomCreat(curpos);
                newRoom.transform.GetChild(1).GetComponent<TMP_Text>().text = takenPositions.Count.ToString()+"\n"+curpos.ToString();
                newRoom.GetComponent<Room>().roomVector2 = newPos;
                newRoom.GetComponent<Room>().RoomCode = Random.Range(1,Room.Length);
                roomsdic.Add(newPos, newRoom);
            }                
        }
    }

    GameObject RoomCreat(Vector2 curpos)//지도위에 방을 생성하는 함수
    {
        GameObject newRoom = Instantiate(roomPrefabs[0], Canvas.transform.GetChild(0));
        newRoom.GetComponent<RectTransform>().sizeDelta =new Vector2(tilex, tiley);
        newRoom.GetComponent<RectTransform>().anchoredPosition = new Vector2((curpos.x - mapWidth/2) * tilex, (curpos.y -mapHeight/2) * tiley);
        return newRoom;
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
    public List<Node> GetNeighbours1234(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                if ((x == -1 && y == -1) || (x == -1 && y == 1) || (x == 1 && y == -1) || (x == 1 && y == 1))
                    continue;

                int checkX = node._gX + x;
                int checkY = node._gY + y;

                if (checkX >= 0 && checkX < mapWidth && checkY >= 0 && checkY < mapHeight)
                {
                    neighbours.Add(_grid[checkX, checkY]);
                }else
                {
                    neighbours.Add(_grid[node._gX, node._gY]);//자기 자신을 리턴
                }
            }
        }

        return neighbours;
    }
    #endregion [맵 생성 함수]
    #region[맵 이벤트 함수]
    Vector2Int SerchRoom(int i)
    {
       return specialrooms[i];
    }//스페셜룸 찾기[0=보스룸,1=황금방]
    public void ViewRoom(int i)
    {
        Image alfa = roomsdic[SerchRoom(i)].GetComponent<Image>();
        Image beta = roomsdic[SerchRoom(i)].transform.GetChild(0).GetComponent<Image>();
        alfa.color = Color255(alfa.color);
        beta.color = Color255(beta.color);
    }//스페셜룸 드러내기
     public void ViewMap()
    {
        for(int i=0; i<roomsdic.Count;i++)
        {
            Image alfa = roomsdic[takenPositions[i]].GetComponent<Image>();
            alfa.color = Color255(alfa.color);
        }
    }//지도 

    public void GotoRoom(int i)
    {
        Hidemap();
        PlayerVector2Set(Rtnode(SerchRoom(i)));
    }//해당 스페셜룸으로 이동하기
    #endregion[맵 이벤트 함수]
    #region[미니맵]
    public void Minimap(bool bol)
    {
        if (bol)
        {
            Canvas.GetComponent<RectTransform>().transform.position = new Vector3(230, 150);
            Canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(900, 490);
            MapSacle = 1f;
            Canvas.GetComponent<RectTransform>().localScale = Vector3.one * 0.5f;
            Canvas.transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.one * MapSacle;
            Canvas.transform.GetChild(1).gameObject.SetActive(false);
            Canvas.transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            Canvas.GetComponent<RectTransform>().transform.position = new Vector3(960, 540);
            Canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(1820, 980);
            MapSacle = 1f;
            Canvas.GetComponent<RectTransform>().localScale = Vector3.one * MapSacle;
            Canvas.transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.one * MapSacle;
            Canvas.transform.GetChild(1).gameObject.SetActive(true);
            Canvas.transform.GetChild(2).gameObject.SetActive(true);
        }
        MiniMapMove();
    }
    public void Minimap(int a)
    {
        if(a==0)
        Canvas.GetComponent<RectTransform>().transform.position = new Vector3(9999, 9999);
    }//대충 맵 어딘가에다 치워둠 게임오브젝트를 비활성화 하면 뭔가 오류가 터질지도 모름

    void MiniMapMove()
    {
        Transform a = Canvas.transform.GetChild(0);
        a.GetComponent<RectTransform>().anchoredPosition = new Vector2((PlayerV2.x - mapWidth / 2) * -182*MapSacle, (PlayerV2.y - mapHeight / 2) * -100 * MapSacle);
    }//미니맵 좌표 수정

    public void minimapscaleminus()
    {
        if (MapSacle <= 0.1f)
            return;
        MapSacle -= 0.1f;
        Canvas.transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.one * MapSacle;
        MiniMapMove();
    }
    public void minimapscaleplus()
    {
        if (MapSacle >= 2f)
            return;
        MapSacle += 0.1f;
        Canvas.transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.one * MapSacle;
        MiniMapMove();
    }
    #endregion[미니맵]
}