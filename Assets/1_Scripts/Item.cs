using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    // Item 기본 정보
    public string itemName;
    public Sprite itemImage;

    public Item(string name, Sprite itemImage)
    {
        itemName = name;
        this.itemImage = itemImage;
    }
}

// ScriptableObject:
//      - 메모리 소모를 줄이는 용도
//      - 오브젝트가 이 데이터를 참조하는 형식으로 작동한다.