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