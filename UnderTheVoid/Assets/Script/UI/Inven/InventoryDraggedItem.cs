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
    /// �� �������� ������ �κ��丮 ��Ʈ�ѷ��� ��ȯ�մϴ�.
    /// </summary>
    public InvenController originalController { get; private set; }

    /// <summary>
    /// �� �������� ���۵� �κ��丮 �� ������ ��ȯ�մϴ�.
    /// </summary>
    public Vector2Int originPoint { get; private set; }

    /// <summary>
    /// �巡�� ���� ������ �ν��Ͻ��� ��ȯ�մϴ�.
    /// </summary>
    public IInventoryItem item { get; private set; }

    /// <summary>
    /// ���� �� �׸��� �����ϰ� �ִ� �κ��丮 ��Ʈ�ѷ��� �������ų� �����մϴ�.
    /// </summary>
    public InvenController currentController;

    private readonly Canvas _canvas;
    private readonly RectTransform _canvasRect;
    private readonly Image _image;
    private Vector2 _offset;
    itemRotae _ro;

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="canvas">The canvas</param>
    /// <param name="originalController">�� �׸��� ��ó�� �� �κ��丮 ��Ʈ�ѷ��� ������ �����ϴ�.</param>
    /// <param name="originPoint">�� �������� ��ó�� �� �κ��丮 �� �����Դϴ�.</param>
    /// <param name="item">�巡�� ���� ������ �ν��Ͻ�</param>
    /// <param name="offset">�������� ���� ������</param>
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

        // �巡���� �׸��� ��Ÿ���� ������ �����
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
    /// �巡���� �׸��� ��ġ�� �������ų� ����
    /// </summary>
    public Vector2 position
    {
        set
        {
            // �̹��� �̵�
            var camera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, value + _offset, camera, out var newValue);
            _image.rectTransform.localPosition = newValue;


            // ����
            if (currentController != null)
            {
                item.position = currentController.ScreenToGrid(value + _offset + GetDraggedItemOffset(currentController.inventoryRenderer, item));
                var canAdd = currentController.inventory.CanAddAt(item, item.position) || CanSwap();
                currentController.inventoryRenderer.SelectItem(item, !canAdd, Color.white);
            }

            // ���콺 �������� �߾����� õõ�� �̵�
            _offset = Vector2.Lerp(_offset, Vector2.zero, Time.deltaTime * 10f);
        }
    }

    /// <summary>
    /// ���������� ȸ��
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
                currentController.inventory.TryAddAt(item, grid); //�� ��ġ�� �׸� ��ġ
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
                originalController.inventory.TryAddAt(item, originPoint); //�׸��� ���� ��ġ�� �ǵ�����
                mode = DropMode.Returned;

            }

            currentController.inventoryRenderer.ClearSelection();
        }
        else
        {
            mode = DropMode.Dropped;
            if (!originalController.inventory.TryForceDrop(item)) // �������� �ٴڿ� ����߸���
            {
                originalController.inventory.TryAddAt(item, originPoint);
            }
        }

        // �׸��� ��Ÿ���� �̹����� ����
        Object.Destroy(_image.gameObject);

        return mode;
    }

    /*
     * �巡���� �׸�� �׸��� ������ �������� ��ȯ
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
     * ������ �����ϸ� true��ȯ
     */
    private bool CanSwap()
    {
        if (!currentController.inventory.CanSwap(item)) return false;
        var otherItem = currentController.inventory.allItems[0];
        return originalController.inventory.CanAdd(otherItem) && currentController.inventory.CanRemove(otherItem);
    }
}
