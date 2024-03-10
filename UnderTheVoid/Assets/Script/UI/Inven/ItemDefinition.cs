using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "인벤토리/아이템 아키타입 생성", order = 1)]
public class ItemDefinition : ScriptableObject, IInventoryItem
{
    [SerializeField] private Sprite _sprite = null;
    [SerializeField] private InvenShape _shape = null;
    [SerializeField] private ItemType _type = ItemType.relic;
    [SerializeField] private bool _canDrop = true;
    [SerializeField, HideInInspector] private Vector2Int _position = Vector2Int.zero;
    [SerializeField] private itemRotae _rotate = itemRotae.up;
    /// <summary>
    /// 아이템 이름
    /// </summary>
    public string Name => this.name;

    /// <summary>
    /// 아이템 타입
    /// </summary>
    public ItemType Type => _type;

    /// <inheritdoc />
    public Sprite sprite => _sprite;

    /// <inheritdoc />
    public int width => _shape.width;

    /// <inheritdoc />
    public int height => _shape.height;

    /// <inheritdoc />
    public Vector2Int position
    {
        get => _position;
        set => _position = value;
    }

    /// <inheritdoc />
    public bool IsPartOfShape(Vector2Int localPosition)
    {
        return _shape.IsPartOfShape(localPosition);
    }

    public bool IspartOfShape90(Vector2Int localPosition)
    {
        return _shape.IsPartOfShape90(localPosition);
    }
    /// <inheritdoc />
    public bool canDrop => _canDrop;

    public void RotateRight()
    {
        switch(_rotate)
        {
            case itemRotae.up:
                _rotate = itemRotae.right;
                break;
            case (itemRotae)90:
                _rotate = itemRotae.down;
                break;
            case (itemRotae)180:
                _rotate = itemRotae.left;
                break;
            case (itemRotae)270:
                _rotate = itemRotae.up;
                break;
        }
    }

    public void RotateLeft()
    {
        switch (_rotate)
        {
            case itemRotae.up:
                _rotate = itemRotae.left;
                break;
            case (itemRotae)270:
                _rotate = itemRotae.down;
                break;
            case (itemRotae)180:
                _rotate = itemRotae.right;
                break;
            case (itemRotae)90:
                _rotate = itemRotae.up;
                break;
        }
    }

    itemRotae IInventoryItem.Rotate => _rotate;

    /// <summary>
    /// Creates a copy if this scriptable object
    /// </summary>
    public IInventoryItem CreateInstance()
    {
        var clone = ScriptableObject.Instantiate(this);
        clone.name = clone.name.Substring(0, clone.name.Length - 7); // Remove (Clone) from name
        return clone;
    }
}