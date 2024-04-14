using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IInventoryController
{
    /// <summary>
    /// 들고있는 아이템
    /// </summary>
    Action<IInventoryItem> onItemHovered { get; set; }
    /// <summary>
    /// 아이템을 들었을 때
    /// </summary>
    Action<IInventoryItem> onItemPickedUp { get; set; }
    /// <summary>
    /// 아이템을 추가 하려고 했을 때
    /// </summary>
    Action<IInventoryItem> onItemAdded { get; set; }
    /// <summary>
    /// 아이템을 교채하려고 할 때
    /// </summary>
    Action<IInventoryItem> onItemSwapped { get; set; }
    /// <summary>
    /// 아이템이 원래 위치로 이동할 때
    /// </summary>
    Action<IInventoryItem> onItemReturned { get; set; }
    /// <summary>
    /// 아이템을 버렸을 때
    /// </summary>
    Action<IInventoryItem> onItemDropped { get; set; }
}
[RequireComponent(typeof(InvenRender))]
public class InvenController : MonoBehaviour,
IPointerDownHandler, IBeginDragHandler, IDragHandler,IPointerUpHandler,
IEndDragHandler, IPointerExitHandler, IPointerEnterHandler,
IInventoryController,IPointerClickHandler
{
    // 드래그한 항목은 정적이며 모든 컨트롤러가 공유합니다.
    // 이렇게 하면 컨트롤러 간에 항목을 쉽게 이동할 수 있습니다.
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
        if (inventoryRenderer == null) { throw new NullReferenceException("인벤토리 랜더러가 없음"); }
       // FindCanvas();
    }

    public void FindCanvas()
    {
        var canvases = GetComponentsInParent<Canvas>();
        if (canvases.Length == 0)
        {
            throw new NullReferenceException("캔버스를 찾을 수 없음");
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
        // 드래그할 항목을 가져옵니다(항목이 없으면 null이 됩니다).
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
     * 드래그 시작(IBeginDragHandler)
     */
    public void OnBeginDrag(PointerEventData eventData)
    {
        inventoryRenderer.ClearSelection();

        if (_itemToDrag == null || _draggedItem != null) return;

        var localPosition = ScreenToLocalPositionInRenderer(eventData.position);
        var itemOffest = inventoryRenderer.GetItemOffset(_itemToDrag);
        var offset = itemOffest - localPosition;

        // 드래그한 항목 만들기 
        _draggedItem = new InventoryDraggedItem(
            _canvas,
            this,
            _itemToDrag.position,
            _itemToDrag,
            offset
        );

        // 인벤토리에서 아이템 제거
        inventory.TryRemove(_itemToDrag);

        onItemPickedUp?.Invoke(_itemToDrag);
    }

    /*
     * 드래그가 계속 중(IDragHandler)
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
     * 드래그 중지(IEndDragHandler)
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
     * 포인터가 인벤토리를 떠난 경우(IPointerExitHandler)
     */
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_draggedItem != null)
        {
            // 항목이 현재 컨트롤러를 떠날 때 지우기
            _draggedItem.currentController = null;
            inventoryRenderer.ClearSelection();
        }
        else {
            inventoryRenderer.ClearSelection();
            ClearHoveredItem(); }
        _currentEventData = null;
    }

    /*
     * 포인터가 인벤토리에 진입했을 경우(IPointerEnterHandler).
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_draggedItem != null)
        {
            // 드래그한 항목을 제어할 컨트롤러 변경하기
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
        Debug.Log("아이템 회전");
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
     * 지정된 화면 지점에서 그리드상의 점 가져오기
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
