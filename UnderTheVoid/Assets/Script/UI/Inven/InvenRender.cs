using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenRender : MonoBehaviour
{
    [SerializeField, Tooltip("�κ��丮 1ĭ�� ũ��")]
    private Vector2Int _cellSize = new Vector2Int(32, 32);

    [SerializeField, Tooltip("����ִ� ���� ��������Ʈ")]
    private Sprite _cellSpriteEmpty = null;

    [SerializeField, Tooltip("������ ���� ����� ��������Ʈ")]
    private Sprite _cellSpriteSelected = null;

    [SerializeField, Tooltip("���ܵ� ���� ����� ��������Ʈ")]
    private Sprite _cellSpriteBlocked = null;

   // [SerializeField, Tooltip("����ִ� ���� ��������Ʈ")]
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
     * ����
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
    /// �������� �� ����� �κ��丮 ����
    /// </summary>
    public void SetInventory(IinvenManager inventoryManager, ItemSoltType renderMode)
    {
        OnDisable();
        inventory = inventoryManager ?? throw new ArgumentNullException(nameof(inventoryManager));
        _renderMode = renderMode;
        OnEnable();
    }


    /// <summary>
    /// �� �������� ���� ��Ʈ Ʈ�������� ��ȯ�մϴ�.
    /// </summary>
    public RectTransform rectTransform { get; private set; }

    /// <summary>
    /// �� �κ��丮�� �� ũ�⸦ ��ȯ�մϴ�.
    /// </summary>
    public Vector2 cellSize => _cellSize;

    /* 
    �κ��丮 �κ��丮 �������� Ȱ��ȭ�Ǹ� ȣ��˴ϴ�.
    */
    void OnEnable()
    {
        if (inventory != null && !_haveListeners)
        {
            if (_cellSpriteEmpty == null) { throw new NullReferenceException("�� ���� ���� ��������Ʈ�� ����"); }
            if (_cellSpriteSelected == null) { throw new NullReferenceException("���õ� ���� ���� ��������Ʈ�� ����"); }
            if (_cellSpriteBlocked == null) { throw new NullReferenceException("������ ���� ���� ��������Ʈ�� ����"); }

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
    �κ��丮 �κ��丮 �������� ��Ȱ��ȭ�Ǿ��� �� ȣ��˴ϴ�.
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
    �׸��带 ����� �������մϴ�. �κ��丮�� ũ�Ⱑ ����� ������ �� �۾��� �����ؾ� �մϴ�.
    */
    private void ReRenderGrid()
    {
        // �׸��� �����
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

        // �� �׸��� ������
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
                // �׸��� �̹��� ����
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

        //  ���� ��Ʈ Ʈ�������� ũ�⸦ �����մϴ�.
        // �̴� ����� ���� �׷��� ��Ҹ� ����ϹǷ� �����մϴ�.
        // �κ��丮�� ũ�⸦ ����ϴ� �׵θ� ���Դϴ�.
        rectTransform.sizeDelta = containerSize;
        ClearSelection();
    }

    /*
   ��� �׸��� ����� �������մϴ�.
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
    inventory.OnItemAdded�� ȣ��� ���� ���� �ڵ鷯
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
            //������ �߰� �̺�Ʈ �ߵ� (�׽�Ʈ��) 
            
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
    inventory.OnItemRemoved�� ȣ��� ���� ���� �ڵ鷯
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
                //����� �� ĳ����
                //�ߵ��ؾ��� ȿ�� 
                Debug.Log("������ ����");
                D_calcuate.i.ItemSkill[item.itemData.itemEffect].SKillOff((D_calcuate.i.PlayerData[this]._pb));
            }
        }
    }

    /*
    inventory.OnResized�� ȣ��� ���� ���� �ڵ鷯
    */
    private void HandleResized()
    {
        ReRenderGrid();
        ReRenderAllItems();
    }

    /*
     * �־��� ��������Ʈ�� �������� �̹��� �����
    */
    private GameObject CreateImage(Sprite sprite, bool raycastTarget)
    {
        var gameObject = Instantiate(_imagePool, transform.GetChild(0));
        var img = gameObject.GetComponent<Image>();
        img.gameObject.SetActive(true);
        img.sprite = sprite;
        img.rectTransform.sizeDelta = new Vector2(img.sprite.rect.width, img.sprite.rect.height);
        img.transform.SetAsLastSibling(); // ���̶���Ű ���� �� �ڷ� �̵�
        img.type = Image.Type.Simple;
        img.raycastTarget = raycastTarget;
        return gameObject;
    }
    private GameObject CreateImageAPS(Sprite sprite, bool raycastTarget)
    {
        var gameObject = ObjPoolManager.i.InstantiateAPSTr("��������Ʈ", transform.GetChild(0));
        var img = gameObject.GetComponent<Image>();
        img.gameObject.SetActive(true);
        img.sprite = sprite;
        img.rectTransform.sizeDelta = new Vector2(img.sprite.rect.width, img.sprite.rect.height);
        img.transform.SetAsLastSibling(); // ���̶���Ű ���� �� �ڷ� �̵�
        img.type = Image.Type.Simple;
        img.raycastTarget = raycastTarget;
        return gameObject;
    }
    /*
     * �־��� �̹��� ��Ȱ�� 
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
    /// �κ��丮���� ������ �׸��� �����մϴ�.
    /// </summary>
    /// <param name="item">������ ����</param>
    /// <param name="blocked">�κ��丮���� ������ ����� ���</param>
    /// <param name="color">���� �׸��� ����</param>
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
                    for (var y = 0; y < item.height; y++)//������ ũ�⸸ŭ �ݺ�
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
    /// �� �κ��丮���� ������ ��� �׸��� ����ϴ�.
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
    �׸��忡 �� �µ��� �׸��� ������ �������� ��ȯ�մϴ�. 
    */
    internal Vector2 GetItemOffset(IInventoryItem item)
    {
        var x = (-(inventory.width * 0.5f) + item.position.x + item.width * 0.5f) * cellSize.x;
        var y = (-(inventory.height * 0.5f) + item.position.y + item.height * 0.5f) * cellSize.y;
        return new Vector2(x, y);
    }
}

