using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemData
{
    public string itemName;
    public Sprite itmeImage;
    public string itemDescription;
}
public interface IItem
{
    string Name { get; }
    void Use();
}
// ScriptableObject:
//      - 메모리 소모를 줄이는 용도
//      - 오브젝트가 이 데이터를 참조하는 형식으로 작동한다.