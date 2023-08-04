using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public struct ItemDatas
{
    string key;
    string name;
    string path;

    public void Set(string a, string b, string c) { key = a; name = b; path = c; }
}
public class Slot : MonoBehaviour
{
    public Image slotImg = null;
    Item itemData;

    List<ItemDatas> itemDatas; // ��ųʸ�<����>���� �ٲ� ��

    public Sprite testImg = null;
    private bool nowUsing;
    
    void Start()
    {
        //slotItem = transform.GetChild(0).gameObject;
        //slotItem.GetComponent<GameObject>();
        slotImg = transform.Find("slotItem")?.GetComponent<Image>();
        LoadItemTextData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            ChangeItemImage(itemData.itemImage);

        if (Input.GetKeyDown(KeyCode.F2))
            ChangeItemImage(testImg);
    }

    void ChangeItemImage(Sprite img)
    {
        slotImg.sprite = img;
    }
    void GetItem(Item i)
    {
        itemData = i;
    }
    void LoadItemTextData() //������ȭ ���Ѽ� �ҷ���
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/ItemData");

        string temp = textAsset.text.Replace("\r\n", "\n");
        string[] row = temp.Split('\n');

        for (int i = 1; i < row.Length; i++)
        {
            string[] data = row[i].Split(',');

            if (data.Length <= 1) continue;

            ItemDatas _itemData = new ItemDatas();

            _itemData.Set(data[0], data[1], data[2]);

            itemDatas.Add(_itemData);
        }
    }

    public bool GetNowUsing() { return nowUsing; }
}

/*
  GetComponentInChildren
      - �ڱ� �ڽ� �����ؼ� �ڽı��� ã�Ƽ� ������Ʈ�� ���� �� �� ���
      - ���� �ڽ��� ã�� ���� �Ŷ�� trnasform.Getchild(int index)
                                   transform.FindChild(string str)

      - �ڱ� �ڽ��� ������ �ڽĵ��� ã�� �ʹٸ� foreach (Transform t in transform) listName.add(t);

  transform.Find(string)
        - �ڱ� �ڽĿ��� ã�Ƽ� ��ȯ.

  Array to List
        - List.addRange(Array);
        - List<> listName = new List(Array);

 ���ҽ� ȣ�� && ScriptableObject ����� ��
        - itemData = new Item();
        - itemData.itemImage = Resources.Load<Sprite>("9_Assetes/RPG_inventory_icons/book");
*/