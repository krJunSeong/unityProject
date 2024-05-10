using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    // Item �⺻ ����
    public string itemName;
    public Sprite itemImage;

    public Item(string name, Sprite itemImage)
    {
        itemName = name;
        this.itemImage = itemImage;
    }
}

// ScriptableObject:
//      - �޸� �Ҹ� ���̴� �뵵
//      - ������Ʈ�� �� �����͸� �����ϴ� �������� �۵��Ѵ�.