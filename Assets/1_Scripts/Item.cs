using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    // Item 기본 정보
    public int itemCode;
    public string itemName;
    public Sprite itemImage;
}

// ScriptableObject:
//      - 메모리 소모를 줄이는 용도
//      - 오브젝트가 이 데이터를 참조하는 형식으로 작동한다.