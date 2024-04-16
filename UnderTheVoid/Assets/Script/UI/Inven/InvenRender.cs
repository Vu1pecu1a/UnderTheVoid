using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenRender : MonoBehaviour
{
    [SerializeField, Tooltip("인벤토리 1칸의 크기")]
    private Vector2Int _cellSize = new Vector2Int(32, 32);

    [SerializeField, Tooltip("비어있는 셀의 스프라이트")]
    private Sprite _cellSpriteEmpty = null;

    [SerializeField, Tooltip("선택한 셀에 사용할 스프라이트")]
    private Sprite _cellSpriteSelected = null;

    [SerializeField, Tooltip("차단된 셀에 사용할 스프라이트")]
    private Sprite _cellSpriteBlocked = null;

   // [SerializeField, Tooltip("비어있는 셀의 스프라이트")]
  //  private Sprite _cellSpriteOccupied = null;

    internal IinvenManager inventory;
    ItemSoltType _renderMode;
    private bool _haveListeners;
    // private Pool<Image> _imagePool;
    [SerializeField]
    GameObject _imagePool;
    private Image[] _grids;
    private Dictionary<IInventoryItem, GameObject> _items = new Dictionary<IInventoryItem, GameObject>();

    /*
     * 시작
     */
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // Create the image container
        var imageContainer = new GameObject("Image Pool").AddComponent<RectTransform>();
        imageContainer.transform.SetParent(transform);
        imageContainer.transform.localPosition = Vector3.zero;
        imageContainer.transform.localScale = Vector3.one;
    }


    /// <summary>
    /// 렌더링할 때 사용할 인벤토리 설정
    /// </summary>
    public void SetInventory(IinvenManager inventoryManager, ItemSoltType renderMode)
    {
        OnDisable();
        inventory = inventoryManager ?? throw new ArgumentNullException(nameof(inventoryManager));
        _renderMode = renderMode;
        OnEnable();
    }


    /// <summary>
    /// 이 렌더러에 대한 렉트 트랜스폼을 반환합니다.
    /// </summary>
    public RectTransform rectTransform { get; private set; }

    /// <summary>
    /// 이 인벤토리의 셀 크기를 반환합니다.
    /// </summary>
    public Vector2 cellSize => _cellSize;

    /* 
    인벤토리 인벤토리 렌더러가 활성화되면 호출됩니다.
    */
    void OnEnable()
    {
        if (inventory != null && !_haveListeners)
        {
            if (_cellSpriteEmpty == null) { throw new NullReferenceException("빈 셀에 대한 스프라이트가 없음"); }
            if (_cellSpriteSelected == null) { throw new NullReferenceException("선택된 셀에 대한 스프라이트가 없음"); }
            if (_cellSpriteBlocked == null) { throw new NullReferenceException("점유된 셀에 대한 스프라이트가 없음"); }

            inventory.onRebuilt += ReRenderAllItems;
            inventory.onItemAdded += HandleItemAdded;
            inventory.onItemRemoved += HandleItemRemoved;
            inventory.onItemDropped += HandleItemRemoved;
            inventory.onResized += HandleResized;
            _haveListeners = true;

            // Render inventory
            ReRenderGrid();
            ReRenderAllItems();
        }
    }

    /* 
    인벤토리 인벤토리 렌더러가 비활성화되었을 때 호출됩니다.
    */
    void OnDisable()
    {
        if (inventory != null && _haveListeners)
        {
            inventory.onRebuilt -= ReRenderAllItems;
            inventory.onItemAdded -= HandleItemAdded;
            inventory.onItemRemoved -= HandleItemRemoved;
            inventory.onItemDropped -= HandleItemRemoved;
            inventory.onResized -= HandleResized;
            _haveListeners = false;
        }
    }

    /*
    그리드를 지우고 렌더링합니다. 인벤토리의 크기가 변경될 때마다 이 작업을 수행해야 합니다.
    */
    private void ReRenderGrid()
    {
        // 그리드 지우기
        if (_grids != null)
        {
            for (var i = 0; i < _grids.Length; i++)
            {
                _grids[i].gameObject.SetActive(false);
                RecycleImage(_grids[i].gameObject);
                _grids[i].transform.SetSiblingIndex(i);
            }
        }
        _grids = null;

        // 새 그리드 렌더링
        var containerSize = new Vector2(cellSize.x * inventory.width, cellSize.y * inventory.height);
        
        Image grid;
        switch (_renderMode)
        {
            case ItemSoltType.Single:    
                grid = CreateImageAPS(_cellSpriteEmpty, true).GetComponent<Image>();
                grid.rectTransform.SetAsFirstSibling();
                grid.type = Image.Type.Sliced;
                grid.rectTransform.localPosition = Vector3.zero;
                grid.rectTransform.sizeDelta = containerSize;
                _grids = new[] { grid };
                break;
            default:
                // 그리드 이미지 스폰
                var topLeft = new Vector3(-containerSize.x / 2, -containerSize.y / 2, 0); // Calculate topleft corner
                var halfCellSize = new Vector3(cellSize.x / 2, cellSize.y / 2, 0); // Calulcate cells half-size
                _grids = new Image[inventory.width * inventory.height];
                var c = 0;
                for (int y = 0; y < inventory.height; y++)
                {
                    for (int x = 0; x < inventory.width; x++)
                    {
                        grid = CreateImageAPS(_cellSpriteEmpty, true).GetComponent<Image>();
                        grid.gameObject.name = "Grid " + c;
                        grid.rectTransform.SetAsFirstSibling();
                        grid.type = Image.Type.Sliced;
                        grid.rectTransform.localPosition = topLeft + new Vector3(cellSize.x * ((inventory.width - 1) - x), cellSize.y * y, 0) + halfCellSize;
                        grid.rectTransform.sizeDelta = cellSize;
                        _grids[c] = grid;
                        c++;
                    }
                }
                break;
        }

        //  메인 렉트 트랜스폼의 크기를 설정합니다.
        // 이는 사용자 지정 그래픽 요소를 허용하므로 유용합니다.
        // 인벤토리의 크기를 모방하는 테두리 등입니다.
        rectTransform.sizeDelta = containerSize;
        ClearSelection();
    }

    /*
   모든 항목을 지우고 렌더링합니다.
    */
    private void ReRenderAllItems()
    {
        // Clear all items
        foreach (var image in _items.Values)
        {
            image.gameObject.SetActive(false);
            RecycleImage(image);
        }
        _items.Clear();

        // Add all items
        foreach (var item in inventory.allItems)
        {
            HandleItemAdded(item);
        }
    }

    public void SaveItemS()
    {
        foreach (var item in inventory.allItems)
        {
            SaveData.Save(new PlayerData(item), Application.streamingAssetsPath + "/1.bin");
        }
    }

    /*
    inventory.OnItemAdded가 호출될 때를 위한 핸들러
    */
    private void HandleItemAdded(IInventoryItem item)
    {
        if(ObjPoolManager.i == null)
        {
            var img = CreateImage(item.sprite, false);
            if (_renderMode == ItemSoltType.Single)
            {
                img.GetComponent<Image>().rectTransform.localPosition = rectTransform.rect.center;
            }
            else
            {
                img.GetComponent<Image>().rectTransform.localPosition = GetItemOffset(item);
                img.transform.rotation = Quaternion.Euler(0, 0, (float)item.Rotate);
            }
            _items.Add(item, img);
            //아이템 추가 이벤트 발동 (테스트용) 
            
        }
        else
        {
            var img = CreateImage(item.sprite, false);
            if (_renderMode == ItemSoltType.Single)
            {
                img.GetComponent<Image>().rectTransform.localPosition = rectTransform.rect.center;
            }
            else
            {
                img.GetComponent<Image>().rectTransform.localPosition = GetItemOffset(item);
                img.transform.rotation = Quaternion.Euler(0, 0, (float)item.Rotate);
            }
            _items.Add(item, img);
            ClearSelection();
        }
    }

    /*
    inventory.OnItemRemoved가 호출될 때를 위한 핸들러
    */
    private void HandleItemRemoved(IInventoryItem item)
    {
        if (_items.ContainsKey(item))
        {
            var image = _items[item];
            image.gameObject.SetActive(false);
            RecycleImage(image);
            _items.Remove(item);
            if (D_calcuate.i.PlayerData.ContainsKey(this))
            {
                //대상이 될 캐릭터
                //발동해야할 효과 
                Debug.Log("아이템 제거");
                D_calcuate.i.ItemSkill[item.itemData.itemEffect].SKillOff((D_calcuate.i.PlayerData[this]._pb));
            }
        }
    }

    /*
    inventory.OnResized가 호출될 때를 위한 핸들러
    */
    private void HandleResized()
    {
        ReRenderGrid();
        ReRenderAllItems();
    }

    /*
     * 주어진 스프라이트와 설정으로 이미지 만들기
    */
    private GameObject CreateImage(Sprite sprite, bool raycastTarget)
    {
        var gameObject = Instantiate(_imagePool, transform.GetChild(0));
        var img = gameObject.GetComponent<Image>();
        img.gameObject.SetActive(true);
        img.sprite = sprite;
        img.rectTransform.sizeDelta = new Vector2(img.sprite.rect.width, img.sprite.rect.height);
        img.transform.SetAsLastSibling(); // 하이라이키 순위 맨 뒤로 이동
        img.type = Image.Type.Simple;
        img.raycastTarget = raycastTarget;
        return gameObject;
    }
    private GameObject CreateImageAPS(Sprite sprite, bool raycastTarget)
    {
        var gameObject = ObjPoolManager.i.InstantiateAPSTr("스프라이트", transform.GetChild(0));
        var img = gameObject.GetComponent<Image>();
        img.gameObject.SetActive(true);
        img.sprite = sprite;
        img.rectTransform.sizeDelta = new Vector2(img.sprite.rect.width, img.sprite.rect.height);
        img.transform.SetAsLastSibling(); // 하이라이키 순위 맨 뒤로 이동
        img.type = Image.Type.Simple;
        img.raycastTarget = raycastTarget;
        return gameObject;
    }
    /*
     * 주어진 이미지 재활용 
     */
    private void RecycleImage(GameObject image)
    {
        if(ObjPoolManager.i== null)
        {
            image.gameObject.SetActive(false);
            //_imagePool.Recycle(image);
        }else
        {
            image.DestroyAPS();
            image.transform.SetParent(ObjPoolManager.i.gameObject.transform);
        }
    }

    /// <summary>
    /// 인벤토리에서 지정된 항목을 선택합니다.
    /// </summary>
    /// <param name="item">아이템 선택</param>
    /// <param name="blocked">인벤토리에서 점유된 블록일 경우</param>
    /// <param name="color">선택 항목의 색상</param>
    public void SelectItem(IInventoryItem item, bool blocked, Color color)
    {
        if (item == null) { return; }
        ClearSelection();

        switch (_renderMode)
        {
            case ItemSoltType.Single:
                _grids[0].GetComponent<Image>().sprite = blocked ? _cellSpriteBlocked : _cellSpriteSelected;
                _grids[0].GetComponent<Image>().color = color;
                break;
            default:
                for (var x = 0; x < item.width; x++)
                {
                    for (var y = 0; y < item.height; y++)//아이템 크기만큼 반복
                    {
                       // if (item.Rotate == itemRotae.right || item.Rotate == itemRotae.left)
                           // IsShapeRotate90(x, y, item, blocked, color);
                       // else
                            IsShapeRotate(x, y, item, blocked, color);

                    }
                }
                break;
        }
    }

    void IsShapeRotate(int x,int y, IInventoryItem item, bool blocked, Color color)
    {
        if (item.IsPartOfShape(new Vector2Int(x, y)))
        {
            var p = item.position + new Vector2Int(x, y);
            if (p.x >= 0 && p.x < inventory.width && p.y >= 0 && p.y < inventory.height)
            {
                var index = p.y * inventory.width + ((inventory.width - 1) - p.x);
                _grids[index].sprite = blocked ? _cellSpriteBlocked : _cellSpriteSelected;
                _grids[index].color = color;
            }
        }
    }
    void IsShapeRotate90(int x, int y, IInventoryItem item, bool blocked, Color color)
    {
        if (item.IsPartOfShape(new Vector2Int(x, y)))
        {
            var p = item.position + new Vector2Int(x, y);
            if (p.x >= 0 && p.x < inventory.width && p.y >= 0 && p.y < inventory.height)
            {
                var index = p.y * inventory.width + ((inventory.width - 1) - p.x);
                _grids[index].sprite = blocked ? _cellSpriteBlocked : _cellSpriteSelected;
                _grids[index].color = color;
            }
        }
    }
    /// <summary>
    /// 이 인벤토리에서 선택한 모든 항목을 지웁니다.
    /// </summary>
    public void ClearSelection()
    {
        
        for (var i = 0; i < _grids.Length; i++)
        {
            _grids[i].sprite = _cellSpriteEmpty;
            _grids[i].color = Color.white;
        }
        itemshape();
    }

    void itemshape()
    {
        IInventoryItem[] items = inventory.allItems;

        foreach (var item in items)
        {
            itemcolor(item);
        }
    }
    void itemcolor(IInventoryItem item)
    {
        for (int i = 0; i < item.width; i++)
        {
            for (int j = 0; j < item.height; j++)
            {
                var p = item.GetMinPoint() + new Vector2Int(i, j);

                if (p.x >= 0 && p.x < inventory.width && p.y >= 0 && p.y < inventory.height)
                {
                    var index = p.y * inventory.width + ((inventory.width - 1) - p.x);
                    _grids[index].color = item.IsPartOfShape(new Vector2Int(i, j)) || _grids[index].color == Color.gray ? Color.gray : Color.white;
                }
            }
        }
    }

    public void OnmouseEnteritemcolor(IInventoryItem item)
    {
        for (int i = 0; i < item.width; i++)
        {
            for (int j = 0; j < item.height; j++)
            {
                var p = item.GetMinPoint() + new Vector2Int(i, j);

                if (p.x >= 0 && p.x < inventory.width && p.y >= 0 && p.y < inventory.height)
                {
                    var index = p.y * inventory.width + ((inventory.width - 1) - p.x);
                    _grids[index].color = item.IsPartOfShape(new Vector2Int(i, j)) ? Color.black : _grids[index].color == Color.gray ? Color.gray : Color.white;
                }
            }
        }
    }


    public void SetSelection( Color color)
    {
        ClearSelection();
        for (var i = 0; i < inventory.allItems.Length; i++)
        {
            IInventoryItem item = inventory.allItems[i];
            for (var x = 0; x < item.width; x++)
            {
                for (var y = 0; y < item.height; y++)
                {
                    Debug.Log(item.GetMinPoint());
                    if (item.IsPartOfShape(new Vector2Int(item.GetMinPoint().x, item.GetMinPoint().y)))
                    {
                        var p = item.GetMinPoint();
                        var index = p.y * inventory.width + ((inventory.width - 1) - p.x);
                        //_grids[index].color = color;
                        _grids[index].sprite = _cellSpriteSelected;
                        Debug.Log(_grids[index]);
                    }
                }
            }
        }
    }
    /*
    그리드에 잘 맞도록 항목의 적절한 오프셋을 반환합니다. 
    */
    internal Vector2 GetItemOffset(IInventoryItem item)
    {
        var x = (-(inventory.width * 0.5f) + item.position.x + item.width * 0.5f) * cellSize.x;
        var y = (-(inventory.height * 0.5f) + item.position.y + item.height * 0.5f) * cellSize.y;
        return new Vector2(x, y);
    }
}

