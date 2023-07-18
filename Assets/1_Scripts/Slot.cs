using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image slotImg = null;
    Item itemData;

    public Sprite testImg = null;
    
    void Start()
    {
        //slotItem = transform.GetChild(0).gameObject;
        //slotItem.GetComponent<GameObject>();
        slotImg = transform.Find("slotItem")?.GetComponent<Image>();

        
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