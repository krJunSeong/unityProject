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
        if(nearObjects.Count > 0)
        {
            //inventory.AddItem(nearObjects[0]);
            nearObjects.RemoveAt(0);

            //1. 해당 오브젝트 비활성화
            //2. 인벤토리: 아이템 획득
            //              - 카운트, 2D Sprite
            // 아이템: Name, Information, Assets
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
