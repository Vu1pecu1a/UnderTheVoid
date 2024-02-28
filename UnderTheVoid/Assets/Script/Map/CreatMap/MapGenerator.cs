using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class MapGenerator : MonoBehaviour // ����� �Լ��� ���� ���⼭ ó��
{
    [SerializeField]
    Sprite[] images;
    public GameObject[] roomPrefabs; // �� ������ �迭
    public GameObject[] Room;//�Ϲ� ��
    public GameObject[] SpecialRoom;//����� ��
    [SerializeField]
    public static Vector2Int PlayerV2;//�÷��̾� ��ġ
    public int mapWidth = 10; // ���� ���� ����
    public int mapHeight = 10; // ���� ���� ����
    public int MaxRoom,MinRoom; // �ִ밪 �ּҰ�
    Node[,] _grid;

    [SerializeField]
    int tilex = 100, tiley=100;
    [SerializeField]
    private List<Vector2Int> takenPositions = new List<Vector2Int>(); // �̹� ��ġ�� ��ġ ���
    [SerializeField]
    private List<Vector2Int> endrooms = new List<Vector2Int>(); //�����
    [SerializeField]
    private Dictionary<Vector2Int, GameObject> roomsdic = new Dictionary<Vector2Int, GameObject>();//�̴ϸ� ��ųʸ� ��� 
    [SerializeField]
    Vector2[] _path;
    [SerializeField]
    Vector2Int curendrooms;
    public Dictionary<Vector2Int, int> endroomsDistance = new Dictionary<Vector2Int, int>();//���� �Ÿ� ����Ʈ
    public static MapGenerator i;

    public delegate void MapInstance();
    public event MapInstance mapInstanceEvent;

    int Pathcount = 0;
    public int RoomCount;

    [SerializeField]
    GameObject Canvas,Loding;

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
    }
    public int _maxSize
    {
        get { return mapHeight*mapWidth; }
    }

    IEnumerator LodingText(int i)
    {
        i++;
        if (i > 9) i = 0;
        yield return new WaitForSeconds(0.1f);
        Loding.transform.GetChild(0).GetComponent<TMP_Text>().text = "�ε���" + new string('.',i);
        StartCoroutine(LodingText(i));
    }

    public void Minimap(bool bol)
    {
        if (bol)
        {
            Canvas.GetComponent<RectTransform>().transform.position = new Vector3(230, 150);
            Canvas.GetComponent<RectTransform>().localScale = Vector3.one * 0.2f;
        }
        else
        {
            Canvas.GetComponent<RectTransform>().transform.position = new Vector3(960, 540);
            Canvas.GetComponent<RectTransform>().localScale = Vector3.one * 1f;
        }  
    }


    void GenerateMap()
    {
        endrooms.Clear();
        Vector2Int startPosition = new Vector2Int(mapWidth / 2, mapHeight / 2); // ���� ��ġ�� ���� �߽�
        GameObject newRoom = RoomCreat(startPosition);
        newRoom.GetComponent<Image>().color = Color.black;
        newRoom.GetComponent<Room>().RoomCode = 0;
        roomsdic.Add(startPosition, newRoom);
        takenPositions.Add(startPosition); // ���� ��ġ�� �̹� ��ġ�� ��ġ�� �߰�
        int i = 0;
        int t = 0;
        StartCoroutine(_newRoom(i,t,startPosition));//����� �õ�
        // �� ��ġ
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
    }

    void EndroomToBoss(Vector2Int Vi2,int Max)
    {
        GameObject Room = roomsdic[Vi2].gameObject;
        Room.GetComponent<Image>().color = Color.red;
        Room.transform.GetChild(0).GetComponent<TMP_Text>().text = "BOSS"+"\n"+Max;
        Room.GetComponent<Room>().RoomCode = 1;
        Room.GetComponent<Room>().Endroom = true;
        endrooms.Remove(Vi2);
        EndroomToGoldenRoom(Random.Range(0, endrooms.Count));
    }//���������� ����

    void EndroomToGoldenRoom(int Vi2)
    {
        GameObject Room = roomsdic[endrooms[Vi2]].gameObject;
        Room.GetComponent<Image>().color = Color.yellow;
        Room.transform.GetChild(0).GetComponent<TMP_Text>().text = "Gold";
        Room.GetComponent<Room>().RoomCode = 0;
        Room.GetComponent<Room>().Endroom = true;
        endrooms.Remove(endrooms[Vi2]);
        mapInstanceEvent();
        PlayerVector2Set(_grid[5,5]);
        Minimap(true);
        Loding.SetActive(false);
    }//Ȳ�ݹ����� ����

    void Debug_error_Event()
    {
        Debug.Log("�̺�Ʈ ����");
    }

    void PlayerVector2Set(Node node)
    {
        PlayerV2 = new Vector2Int(node._gX, node._gY);
        roomsdic[PlayerV2].transform.GetComponent<Image>().sprite = images[0];
        roomsdic[PlayerV2].GetComponent<Room>().SetFiled(true);   
    }//�ʻ󿡼��� �÷��̾� ��ġ ����
    
    public void PlayerMoveToMap(int i)
    {
        Vector2Int v = PlayerV2;
        roomsdic[PlayerV2].transform.GetComponent<Image>().sprite = null;//�̹��� �ʱ�ȭ
        roomsdic[PlayerV2].GetComponent<Room>().SetFiled(false);
        List<Node> Neighboirs = GetNeighbours1234(_grid[v.x,v.y]);//up,left,right,down
        if (Neighboirs[i]._walkable == true)
        {
            PlayerVector2Set(Neighboirs[i]);
        }
        else//������ ������ �����ڸ��� �ٽ� �̵�
            PlayerVector2Set(_grid[v.x, v.y]);

    }//�÷��̾� ��ġ �̵�

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
        
        if (takenPositions.Count<MinRoom || endrooms.Count<2)//����� �ּ�ġ ���ϰų� ����� ������ 2�� ������ ��� �� ����
        {
            ResetMap();
        }else // ���������� ��� ����������� Node����
        {
            foreach(Vector2Int a in takenPositions)
            {
                _grid[a.x,a.y]._walkable= true;
            }
            foreach (Vector2Int v in endrooms)
            {
                if (takenPositions.Contains(v))
                {
                    PathRequestManager.RequestPath(v, takenPositions[0], OnPathFound);
                }
            }
        }
    }//����� ������ �ܰ�
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
        yield return new WaitForSeconds(0.01f);
        k++;//�õ� Ƚ��
        t++;//�� ��ȣ ã��
        if (endrooms.Contains(startPosition))
        {
            roomsdic[startPosition].GetComponent<Image>().color = prevColor;
            if ( takenPositions.Count<k&& takenPositions.Count > t)//�õ� 
                StartCoroutine(_newRoom(k, t, takenPositions[t]));
            else
                NomalroomToEndRoom();
        }//�������� ������̸� ���� �õ� �ѱ��
        else
        {
            NewPosition(startPosition + Vector2Int.up); // ����� �õ�
            NewPosition(startPosition + Vector2Int.down);
            NewPosition(startPosition + Vector2Int.left);
            NewPosition(startPosition + Vector2Int.right);

            roomsdic[startPosition].GetComponent<Image>().color = prevColor;
            endroomcheck(startPosition);

            if (takenPositions.Count < MaxRoom && k < RoomCount && takenPositions.Count > t)//�õ� 
                StartCoroutine(_newRoom(k, t, takenPositions[t]));
            else if (takenPositions.Count < MinRoom)//�� ���� ������ �ּ�ġ���� ������
            {
                t = 0;//0���� �����ϱ�
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

    void endroomcheck(Vector2Int startPosition)//����� üũ
    {
        int end = 0;

        for (int i = 0; i < 4; i++)
        {
            if (PosContains(i, startPosition) == true)//�ֺ� �� �˻�
                end++;
        }

        if (end == 1 && startPosition != takenPositions[0] && !endrooms.Contains(startPosition))
        {
            endrooms.Add(startPosition);//����� �߰�
            //roomsdic[startPosition].gameObject.GetComponent<Renderer>().material.color = Color.red;
            roomsdic[startPosition].GetComponent<Image>().color = Color.cyan;
        }
    }

    public void ResetMap()//�� ���� �Լ�
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
        Pathcount=0;
        GenerateMap();
    }

    void NewPosition(Vector2Int curpos) // �� ���� �Լ�
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

            if (attempts == 1&&Random.Range(0,9)<5&& curpos.x<10 &&curpos.x>0 && curpos.y <10 &&curpos.y>0)
            {
                takenPositions.Add(newPos);//����� �õ�
                GameObject newRoom = RoomCreat(curpos);
                newRoom.transform.GetChild(0).GetComponent<TMP_Text>().text = takenPositions.Count.ToString()+"\n"+curpos.ToString();
                newRoom.GetComponent<Room>().roomVector2 = newPos;
                newRoom.GetComponent<Room>().RoomCode = Random.Range(1,Room.Length);
                roomsdic.Add(newPos, newRoom);
            }                
        }
    }

    GameObject RoomCreat(Vector2 curpos)//���� ���� �����ϴ� �Լ�
    {
        GameObject newRoom = Instantiate(roomPrefabs[0], Canvas.transform);
        newRoom.GetComponent<RectTransform>().sizeDelta =new Vector2(tilex, tiley);
        newRoom.GetComponent<RectTransform>().anchoredPosition = new Vector2((curpos.x - 5) * tilex, (curpos.y - 5) * tiley);
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
                    neighbours.Add(_grid[node._gX, node._gY]);//�ڱ� �ڽ��� ����
                }
            }
        }

        return neighbours;
    }
}