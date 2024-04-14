using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IInventoryController
{
    /// <summary>
    /// ����ִ� ������
    /// </summary>
    Action<IInventoryItem> onItemHovered { get; set; }
    /// <summary>
    /// �������� ����� ��
    /// </summary>
    Action<IInventoryItem> onItemPickedUp { get; set; }
    /// <summary>
    /// �������� �߰� �Ϸ��� ���� ��
    /// </summary>
    Action<IInventoryItem> onItemAdded { get; set; }
    /// <summary>
    /// �������� ��ä�Ϸ��� �� ��
    /// </summary>
    Action<IInventoryItem> onItemSwapped { get; set; }
    /// <summary>
    /// �������� ���� ��ġ�� �̵��� ��
    /// </summary>
    Action<IInventoryItem> onItemReturned { get; set; }
    /// <summary>
    /// �������� ������ ��
    /// </summary>
    Action<IInventoryItem> onItemDropped { get; set; }
}
[RequireComponent(typeof(InvenRender))]
public class InvenController : MonoBehaviour,
IPointerDownHandler, IBeginDragHandler, IDragHandler,IPointerUpHandler,
IEndDragHandler, IPointerExitHandler, IPointerEnterHandler,
IInventoryController,IPointerClickHandler
{
    // �巡���� �׸��� �����̸� ��� ��Ʈ�ѷ��� �����մϴ�.
    // �̷��� �ϸ� ��Ʈ�ѷ� ���� �׸��� ���� �̵��� �� �ֽ��ϴ�.
    private static InventoryDraggedItem _draggedItem;

    /// <inheritdoc />
    public Action<IInventoryItem> onItemHovered { get; set; }

    /// <inheritdoc />
    public Action<IInventoryItem> onItemPickedUp { get; set; }

    /// <inheritdoc />
    public Action<IInventoryItem> onItemAdded { get; set; }

    /// <inheritdoc />
    public Action<IInventoryItem> onItemSwapped { get; set; }

    /// <inheritdoc />
    public Action<IInventoryItem> onItemReturned { get; set; }

    /// <inheritdoc />
    public Action<IInventoryItem> onItemDropped { get; set; }

    private Canvas _canvas;
    internal InvenRender inventoryRenderer;
    internal InventoryManager inventory => (InventoryManager)inventoryRenderer.inventory;

    private IInventoryItem _itemToDrag;
    private PointerEventData _currentEventData;
    private IInventoryItem _lastHoveredItem;

    /*
     * Setup
     */
    void Awake()
    {
        inventoryRenderer = GetComponent<InvenRender>();
        if (inventoryRenderer == null) { throw new NullReferenceException("�κ��丮 �������� ����"); }
       // FindCanvas();
    }

    public void FindCanvas()
    {
        var canvases = GetComponentsInParent<Canvas>();
        if (canvases.Length == 0)
        {
            throw new NullReferenceException("ĵ������ ã�� �� ����");
        }
        _canvas = canvases[canvases.Length - 1];
    }

    void Start()
    {
        Managers.instance.RkeyInput += R;
        if(_canvas==null)
        FindCanvas();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        var grid = ScreenToGrid(eventData.position);
        if (inventory.GetAtPoint(grid) == null)
        {Managers.instance._UI.ItemtoolTip().gameObject.SetActive(false);
            return;
        }
        if (eventData.clickCount != 2)
        {
            return;
        }
        Managers.instance._UI.ItemtoolTip().gameObject.SetActive(true);
        Managers.instance._UI.ItemtoolTip().GetComponent<ItemInfo>().SetInfo(inventory.GetAtPoint(grid));
        Managers.instance._UI.ItemtoolTip().GetComponent<RectTransform>().position =
            new Vector3(Mathf.Clamp(eventData.position.x, 0, Screen.width), Mathf.Clamp(eventData.position.y, 0, Screen.height));

    }
    /*
     * Grid was clicked (IPointerDownHandler)
     */
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_draggedItem != null) return;
        // �巡���� �׸��� �����ɴϴ�(�׸��� ������ null�� �˴ϴ�).
        var grid = ScreenToGrid(eventData.position);
        _itemToDrag = inventory.GetAtPoint(grid);
        inventoryRenderer.ClearSelection();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_draggedItem != null) return; 
        var grid = ScreenToGrid(eventData.position);
    }
    /*
     * �巡�� ����(IBeginDragHandler)
     */
    public void OnBeginDrag(PointerEventData eventData)
    {
        inventoryRenderer.ClearSelection();

        if (_itemToDrag == null || _draggedItem != null) return;

        var localPosition = ScreenToLocalPositionInRenderer(eventData.position);
        var itemOffest = inventoryRenderer.GetItemOffset(_itemToDrag);
        var offset = itemOffest - localPosition;

        // �巡���� �׸� ����� 
        _draggedItem = new InventoryDraggedItem(
            _canvas,
            this,
            _itemToDrag.position,
            _itemToDrag,
            offset
        );

        // �κ��丮���� ������ ����
        inventory.TryRemove(_itemToDrag);

        onItemPickedUp?.Invoke(_itemToDrag);
    }

    /*
     * �巡�װ� ��� ��(IDragHandler)
     */
    public void OnDrag(PointerEventData eventData)
    {
        _currentEventData = eventData;
        if (_draggedItem != null)
        {
            // Update the items position
            //_draggedItem.Position = eventData.position;
        }
        Managers.instance._UI.ItemtoolTip().gameObject.SetActive(false);
    }

    /*
     * �巡�� ����(IEndDragHandler)
     */
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_draggedItem == null) return;

        ClearHoveredItem();
        var mode = _draggedItem.Drop(eventData.position);

        switch (mode)
        {
            case InventoryDraggedItem.DropMode.Added:
                onItemAdded?.Invoke(_itemToDrag);
                break;
            case InventoryDraggedItem.DropMode.Swapped:
                onItemSwapped?.Invoke(_itemToDrag);
                break;
            case InventoryDraggedItem.DropMode.Returned:
                onItemReturned?.Invoke(_itemToDrag);
                break;
            case InventoryDraggedItem.DropMode.Dropped:
                onItemDropped?.Invoke(_itemToDrag);
                ClearHoveredItem();
                break;
        }
        _draggedItem = null;
        _currentEventData = null;
        _currentEventData = eventData;
    }

    /*
     * �����Ͱ� �κ��丮�� ���� ���(IPointerExitHandler)
     */
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_draggedItem != null)
        {
            // �׸��� ���� ��Ʈ�ѷ��� ���� �� �����
            _draggedItem.currentController = null;
            inventoryRenderer.ClearSelection();
        }
        else {
            inventoryRenderer.ClearSelection();
            ClearHoveredItem(); }
        _currentEventData = null;
    }

    /*
     * �����Ͱ� �κ��丮�� �������� ���(IPointerEnterHandler).
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_draggedItem != null)
        {
            // �巡���� �׸��� ������ ��Ʈ�ѷ� �����ϱ�
            _draggedItem.currentController = this;
        }
        _currentEventData = eventData;
    }

    /*
     * Update loop
     */
    void Update()
    {
        if (_currentEventData == null)
        {
            return;
        }
            
        if (_draggedItem == null)
        {
            // Detect hover
            var grid = ScreenToGrid(_currentEventData.position);
            var item = inventory.GetAtPoint(grid);
            if(item!=null)
            inventoryRenderer.OnmouseEnteritemcolor(item);
            else
            {
                inventoryRenderer.ClearSelection();
                ClearHoveredItem();
            }

            if (item == _lastHoveredItem) return;

            inventoryRenderer.ClearSelection();
            onItemHovered?.Invoke(item);
            _lastHoveredItem = item;
        }
        else
        {
            // Update position while dragging
            _draggedItem.position = _currentEventData.position;
        }
    }

    public void R()
    {
        if(_draggedItem==null)
        return;
        if(_draggedItem.currentController == this)
        _draggedItem.currentController.RotateGetKeyDown();
    }

    public void RotateGetKeyDown()
    {
        Debug.Log("������ ȸ��");
        _draggedItem.RotateItem();
    }

    /* 
     * 
     */
    private void ClearHoveredItem()
    {
        if (_lastHoveredItem != null)
        {
            onItemHovered?.Invoke(null);
        }
        _lastHoveredItem = null;
    }

    /*
     * ������ ȭ�� �������� �׸������ �� ��������
     */
    internal Vector2Int ScreenToGrid(Vector2 screenPoint)
    {
        var pos = ScreenToLocalPositionInRenderer(screenPoint);
        var sizeDelta = inventoryRenderer.rectTransform.sizeDelta;
        pos.x += sizeDelta.x / 2;
        pos.y += sizeDelta.y / 2;
        return new Vector2Int(Mathf.FloorToInt(pos.x / inventoryRenderer.cellSize.x), Mathf.FloorToInt(pos.y / inventoryRenderer.cellSize.y));
    }

    private Vector2 ScreenToLocalPositionInRenderer(Vector2 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            inventoryRenderer.rectTransform,
            screenPosition,
            _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
            out var localPosition
        );
        return localPosition;
    }

}
