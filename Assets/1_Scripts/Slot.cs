using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField]
    GameObject slotItem = null;
    Transform t;
    Item itemData;

    void Start()
    {
        //slotItem = transform.GetChild(0).gameObject;
        slotItem = transform.Find("slotItem").gameObject; 
        //slotItem.GetComponent<GameObject>();
    }

    void Update()
    {
        
    }
}

/*
  GetComponentInChildren
      - 자기 자신 포함해서 자식까지 찾아서 컴포넌트를 갖고 올 때 사용
      - 만약 자식을 찾고 싶은 거라면 trnasform.Getchild(int index)
                                   transform.FindChild(string str)
  transform.Find(string)
        - 자기 자식에서 찾아서 반환.

  Array to List
        - List.addRange(Array);
        - List<> listName = new List(Array);
*/