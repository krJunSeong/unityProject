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
        // 1. �������̸� �ֺ� nearObject�� �߰�.
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
                // 1. ����Ʈ���� ���� �� ����
                if (nearObjects[i].name == other.name)
                {
                    // 2. ���� �� ã�Ҵٸ� �����ֱ�
                    // �̸��� ������? ���� �ߴ� �� �ƴѰ� �ϴ� ������ ���� ����^^;
                    nearObjects.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
