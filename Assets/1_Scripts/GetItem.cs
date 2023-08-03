using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    [SerializeField]
    List<GameObject> nearObjects;

    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. 아이템이면 주변 nearObject에 추가.
        if (other.gameObject.tag == "Item")
        {
            nearObjects.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        EraseNearObject(other);
    }

    private void GetItems()
    {
        if(nearObjects.Count >= 1)
        {
            inventory.GetItem(nearObjects[0]);
            nearObjects.RemoveAt(0);
        }    
    }

    private void EraseNearObject(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            for (int i = 0; i < nearObjects.Count; i++)
            {
                // 1. 리스트에서 나간 것 색출
                if (nearObjects[i].name == other.name)
                {
                    // 2. 나간 거 찾았다면 없애주기
                    // 이름이 같으면? 에러 뜨는 거 아닌가 하는 생각은 조금 있음^^;
                    nearObjects.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
