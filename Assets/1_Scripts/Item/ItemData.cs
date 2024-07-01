using UnityEngine;

public class ItemData : ScriptableObject
{
    public string itemName => _itemName;
    public Sprite itemImage => _itemImage;
    public string itemDescription => _itemDescription;
    public ItemType type => _type;

    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemImage;
    [SerializeField] private string _itemDescription;
    [SerializeField] private ItemType _type;
    [SerializeField] private GameObject _dropPrefab;

    public enum ItemType { SEED, USED, EQUIPMENT }
}
