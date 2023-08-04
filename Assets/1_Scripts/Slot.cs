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

    List<ItemDatas> itemDatas; // 딕셔너리<네임>으로 바꿀 것

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
    void LoadItemTextData() //데이터화 시켜서 불러옴
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
      - 자기 자신 포함해서 자식까지 찾아서 컴포넌트를 갖고 올 때 사용
      - 만약 자식을 찾고 싶은 거라면 trnasform.Getchild(int index)
                                   transform.FindChild(string str)

      - 자기 자신을 제외한 자식들을 찾고 싶다면 foreach (Transform t in transform) listName.add(t);

  transform.Find(string)
        - 자기 자식에서 찾아서 반환.

  Array to List
        - List.addRange(Array);
        - List<> listName = new List(Array);

 리소스 호출 && ScriptableObject 사용할 때
        - itemData = new Item();
        - itemData.itemImage = Resources.Load<Sprite>("9_Assetes/RPG_inventory_icons/book");
*/