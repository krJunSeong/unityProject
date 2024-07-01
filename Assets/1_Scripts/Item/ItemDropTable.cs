using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItemTable", menuName = "Item/DropItemTable")]
public class ItemDropTable : ScriptableObject
{
    public Item[] dropItems;// ���������
    public float dropPer;   // ���Ȯ��
}
