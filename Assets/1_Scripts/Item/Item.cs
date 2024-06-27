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
//      - �޸� �Ҹ� ���̴� �뵵
//      - ������Ʈ�� �� �����͸� �����ϴ� �������� �۵��Ѵ�.