using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    [SerializeField]
    List<GameObject> nearObjects;

    Inventory inventory;

    void Awake()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if(Input.GetKeyDown(KeyCode.E)) GetItems();
    }

    void Move()
    {
        float speed = 10.0f;
        if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.forward *  speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.forward * -speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.right   * -speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right   *  speed * Time.deltaTime);
    }

    void PrintNearObject()
    {
        foreach (GameObject i in nearObjects)
            Debug.Log(i);
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
        if(nearObjects.Count > 0)
        {
            //inventory.AddItem(nearObjects[0]);
            nearObjects.RemoveAt(0);

            //1. �ش� ������Ʈ ��Ȱ��ȭ
            //2. �κ��丮: ������ ȹ��
            //              - ī��Ʈ, 2D Sprite
            // ������: Name, Information, Assets
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
