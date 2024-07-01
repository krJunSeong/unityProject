using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    public string Name => itemData.itemName;
    public string itemName { get {return itemData.itemName; } }
    public Sprite icon => itemData.itemImage;

    //public Item(ItemData data) => itemData = data;
}
