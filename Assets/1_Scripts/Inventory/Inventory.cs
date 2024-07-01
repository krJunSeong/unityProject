using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Slot[] slots;  // 인벤토리 슬롯 배열
    public Seed item;     // 인스펙터에서 할당된 Seed 객체

    Dictionary<string, Slot> itemSlotDictionary; // 아이템 이름과 슬롯을 매핑하는 딕셔너리
    Dictionary<string, int> stoneTreeCnt;        // Stone, Tree 재고를 추적하는 딕셔너리

    void Start()
    {
        // 메모리 할당
        itemSlotDictionary = new Dictionary<string, Slot>();
        stoneTreeCnt = new Dictionary<string, int>();

        // 개수 초기 세팅
        stoneTreeCnt["Stone"] = 0;
        stoneTreeCnt["Tree"] = 0;

        // 슬롯 배열 초기화
        slots = GetComponentsInChildren<Slot>();
        foreach (var slot in slots)
        {
            slot.ClearSlot();
        }

        Debug.Log($"{item.gameObject.name}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (item != null)
            {
                AddItem(item, 1);
            }
            else
            {
                Debug.LogError("No Seed item assigned.");
            }
        }
    }

    public void AddItem(Item item, int amount)
    {
        if (itemSlotDictionary.ContainsKey(item.itemName))
        {
            Slot slot = itemSlotDictionary[item.itemName];
            slot.AddItemCount(amount);
        }
        else
        {
            Slot emptySlot = GetEmptySlot();
            if (emptySlot != null)
            {
                emptySlot.SetItem(item, amount);
                itemSlotDictionary[item.itemName] = emptySlot;
            }
            else
            {
                Debug.Log("No empty slot available!");
            }
        }
    }

    public void AddMaterialItem(string name, int n)
    {
        if (stoneTreeCnt.ContainsKey(name))
        {
            stoneTreeCnt[name] += n;
        }
        else
        {
            Debug.Log($"inventory: 잘못된 건설자재 추가입니다.(이름 미싱 {name})");
        }
    }

    private Slot GetEmptySlot()
    {
        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                return slot;
            }
        }
        return null;
    }

    public int GetItemCount(string itemName)
    {
        if (itemSlotDictionary.ContainsKey(itemName))
        {
            return itemSlotDictionary[itemName].GetCount();
        }
        return 0;
    }

    public int GetItemInInventory(string constructionMaterial)
    {
        // Stone, Tree 반환함수
        if (!stoneTreeCnt.ContainsKey(constructionMaterial))
        {
            Debug.Log("잘못된 Tree, Stone 이름입니다.");
            return 0;
        }

        return stoneTreeCnt[constructionMaterial];
    }

    public Item GetSeed()
    {
        foreach (var inven in slots)
        {
            // 슬롯에서 아이템을 검색하는 로직 추가 필요
        }
        return null;
    }
}
