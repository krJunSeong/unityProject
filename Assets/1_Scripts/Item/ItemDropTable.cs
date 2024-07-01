using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItemTable", menuName = "Item/DropItemTable")]
public class ItemDropTable : ScriptableObject
{
    public Item[] dropItems;// 드랍아이템
    public float dropPer;   // 드랍확률
}
