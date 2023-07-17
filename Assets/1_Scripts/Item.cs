using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    // Item �⺻ ����
    public int itemCode;
    public string itemName;
    public Sprite itemImage;
}

// ScriptableObject:
//      - �޸� �Ҹ� ���̴� �뵵
//      - ������Ʈ�� �� �����͸� �����ϴ� �������� �۵��Ѵ�.