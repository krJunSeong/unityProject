using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    List<Slot> slots;
    Dictionary<string, int> inventory = new Dictionary<string, int>();
    
    void Start()
    {
        Slot[] s = GetComponentsInChildren<Slot>(true);
        slots.AddRange(s);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddItem(string item, int num)
    {
        // 아이템 추가 함수

        if(inventory.ContainsKey(item))
        {
            inventory[item] += num;
        }
        else inventory.Add(item, num);
    }

    public void AddItem(GameObject inputGameObject)
    {
        foreach(Slot slot in slots)
        {
            if (!slot.GetNowUsing())
            {
                // 1.해당 빈자리에 아이템 넣어주기.
                Debug.Log("빈 자리가 있습니다.");
                    // 0. 아이템 데이터에 텍스처 정보도 넣는다 or
                    //    아이템 매니저를 만들어서 딕셔너리로 관리한다. <- 이게 좋은 듯. 모든 아이템에 데이터 넣으면 방대해짐.
                    // 1. 아이템 매니저에서 "silverCoins"의 텍스처 정보를 갖고 온다.
                    // 2. 슬롯에 넣어준다.
                inputGameObject.SetActive(false);
                return;
            }
        }
        // 3. 빈 슬롯이 없다면 로그 출력.
        Debug.Log("인벤토리에 빈 자리가 없습니다.");
    }

    public int GetItemInInventory(string name)
    {
        // 인벤토리 내의 아이템 개수 출력하는 함수
        // 해당 아이템 없으면 0개 리턴
        if (inventory.ContainsKey(name)) return inventory[name];

        return 0;
    }
}
