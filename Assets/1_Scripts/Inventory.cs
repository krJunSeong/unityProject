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
                // 1.�ش� ���ڸ��� ������ �־��ֱ�.
                Debug.Log("�� �ڸ��� �ֽ��ϴ�.");
                slot
                return;
            }
        }
        // 3. �� ������ ���ٸ� �α� ���.
        Debug.Log("�κ��丮�� �� �ڸ��� �����ϴ�.");
    }
}
