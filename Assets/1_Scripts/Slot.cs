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
      - �ڱ� �ڽ� �����ؼ� �ڽı��� ã�Ƽ� ������Ʈ�� ���� �� �� ���
      - ���� �ڽ��� ã�� ���� �Ŷ�� trnasform.Getchild(int index)
                                   transform.FindChild(string str)
  transform.Find(string)
        - �ڱ� �ڽĿ��� ã�Ƽ� ��ȯ.

  Array to List
        - List.addRange(Array);
        - List<> listName = new List(Array);
*/