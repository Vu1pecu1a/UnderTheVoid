using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDraggedItem 
{
    public enum DropMode
    {
        Added,
        Swapped,
        Returned,
        Dropped,
    }

    /// <summary>
    /// 이 아이템이 생성된 인벤토리 컨트롤러를 반환합니다.
    /// </summary>
    public InvenController originalController { get; private set; }

    /// <summary>
    /// 이 아이템이 시작된 인벤토리 내 지점을 반환합니다.
    /// </summary>
    public Vector2Int originPoint { get; private set; }

    /// <summary>
    /// 드래그 중인 아이템 인스턴스를 반환합니다.
    /// </summary>
    public IInventoryItem item { get; private set; }

    /// <summary>
    /// 현재 이 항목을 제어하고 있는 인벤토리 컨트롤러를 가져오거나 설정합니다.
    /// </summary>
    public InvenController currentController;

    private readonly Canvas _canvas;
    private readonly RectTransform _canvasRect;
    private readonly Image _image;
    private Vector2 _offset;
    itemRotae _ro;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="canvas">The canvas</param>
    /// <param name="originalController">이 항목의 출처가 된 인벤토리 컨트롤러는 다음과 같습니다.</param>
    /// <param name="originPoint">이 아이템의 출처가 된 인벤토리 내 지점입니다.</param>
    /// <param name="item">드래그 중인 아이템 인스턴스</param>
    /// <param name="offset">아이템의 시작 오프셋</param>
    [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
    public InventoryDraggedItem(Canvas canvas,InvenController originalController,Vector2Int originPoint,IInventoryItem item,Vector2 offset)
    {
        this.originalController = originalController;
        currentController = this.originalController;
        this.originPoint = originPoint;
        this.item = item;

        _canvas = canvas;
        _canvasRect = canvas.transform as RectTransform;

        _offset = offset;

        _ro = item.Rotate;

        // 드래그한 항목을 나타내는 아이템 만들기
        _image = new GameObject("DraggedItem").AddComponent<Image>();
        _image.raycastTarget = false;
        _image.transform.SetParent(_canvas.transform);
        _image.transform.SetAsLastSibling();
        _image.transform.localScale = Vector3.one;
        _image.sprite = item.sprite;
        _image.transform.rotation = Quaternion.Euler(0, 0, (float)item.Rotate);
        _image.SetNativeSize();
    }

    /// <summary>
    /// 드래그한 항목의 위치를 가져오거나 설정
    /// </summary>
    public Vector2 position
    {
        set
        {
            // 이미지 이동
            var camera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, value + _offset, camera, out var newValue);
            _image.rectTransform.localPosition = newValue;


            // 선택
            if (currentController != null)
            {
                item.position = currentController.ScreenToGrid(value + _offset + GetDraggedItemOffset(currentController.inventoryRenderer, item));
                var canAdd = currentController.inventory.CanAddAt(item, item.position) || CanSwap();
                currentController.inventoryRenderer.SelectItem(item, !canAdd, Color.white);
            }

            // 마우스 포인터의 중앙으로 천천히 이동
            _offset = Vector2.Lerp(_offset, Vector2.zero, Time.deltaTime * 10f);
        }
    }

    /// <summary>
    /// 오른쪽으로 회전
    /// </summary>
    public void RotateItem()
    {
        item.RotateRight();
        _image.transform.rotation = Quaternion.Euler(0, 0, (float)item.Rotate);
    }

    /// <summary>
    /// Drop this item at the given position
    /// </summary>
    public DropMode Drop(Vector2 pos)
    {
        DropMode mode;
        if (currentController != null)
        {
            var grid = currentController.ScreenToGrid(pos + _offset + GetDraggedItemOffset(currentController.inventoryRenderer, item));

            // Try to add new item
            if (currentController.inventory.CanAddAt(item, grid))
            {
                currentController.inventory.TryAddAt(item, grid); //새 위치에 항목 배치
                mode = DropMode.Added;
            }
            // Adding did not work, try to swap
            else if (CanSwap())
            {
                var otherItem = currentController.inventory.allItems[0];
                currentController.inventory.TryRemove(otherItem);
                originalController.inventory.TryAdd(otherItem);
                currentController.inventory.TryAdd(item);
                mode = DropMode.Swapped;
            }
            // Could not add or swap, return the item
            else
            {
                Debug.Log(originPoint);
                item.RotateOrigin(_ro);
                originalController.inventory.TryAddAt(item, originPoint); //항목을 이전 위치로 되돌리기
                mode = DropMode.Returned;

            }

            currentController.inventoryRenderer.ClearSelection();
        }
        else
        {
            mode = DropMode.Dropped;
            if (!originalController.inventory.TryForceDrop(item)) // 아이템을 바닥에 떨어뜨리기
            {
                originalController.inventory.TryAddAt(item, originPoint);
            }
        }

        // 항목을 나타내는 이미지를 삭제
        Object.Destroy(_image.gameObject);

        return mode;
    }

    /*
     * 드래그한 항목과 그리드 사이의 오프셋을 반환
     */
    private Vector2 GetDraggedItemOffset(InvenRender renderer, IInventoryItem item)
    {
        var scale = new Vector2(
            Screen.width / _canvasRect.sizeDelta.x,
            Screen.height / _canvasRect.sizeDelta.y
        );
        var gx = -(item.width * renderer.cellSize.x / 2f) + (renderer.cellSize.x / 2);
        var gy = -(item.height * renderer.cellSize.y / 2f) + (renderer.cellSize.y / 2);
        return new Vector2(gx, gy) * scale;
    }

    /* 
     * 스왑이 가능하면 true반환
     */
    private bool CanSwap()
    {
        if (!currentController.inventory.CanSwap(item)) return false;
        var otherItem = currentController.inventory.allItems[0];
        return originalController.inventory.CanAdd(otherItem) && currentController.inventory.CanRemove(otherItem);
    }
}
