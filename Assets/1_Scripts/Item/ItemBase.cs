using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour, IItem
{
    public ItemData itemData;
    public string Name => itemData.itemName;

    public virtual void Use()
    {
    }
}
