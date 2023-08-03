using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    List<Slot> slots;
    
    void Start()
    {
        Slot[] s = GetComponentsInChildren<Slot>(true);
        slots.AddRange(s);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetItem()
    {
        bool bEmptySlot;
        foreach(Slot slot in slots)
        {
            if (slot.GetNowUsing())
            {
                // 1.해당 빈자리에 아이템 넣어주기.
                Debug.Log("빈 자리가 있습니다.");
                slot
                return;
            }
        }
        // 3. 빈 슬롯이 없다면 로그 출력.
        Debug.Log("인벤토리에 빈 자리가 없습니다.");
    }
}
