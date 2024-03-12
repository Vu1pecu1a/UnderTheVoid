using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Item", menuName = "인벤토리/아이템 아키타입 생성", order = 1)]
public class ItemDefinition : ScriptableObject, IInventoryItem
{
    [SerializeField] private Sprite _sprite = null;
    [SerializeField] private InvenShape _shape = null;
    [SerializeField] private ItemType _type = ItemType.relic;
    [SerializeField] private bool _canDrop = true;
    [SerializeField, HideInInspector] private Vector2Int _position = Vector2Int.zero;
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

    public void RotateRight()
    {
       _shape.RotateRight();
    }

    public void RotateOrigin(itemRotae _ro)
    {
        while(true)
        {
            _shape.RotateRight();
            
            if(_ro == _shape.rotate)
            break;
        }
    }

    /// <inheritdoc />
    public bool canDrop => _canDrop;

    itemRotae IInventoryItem.Rotate { get => _shape.rotate; set => _shape.rotate = value; }

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