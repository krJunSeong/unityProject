using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Slot[] slots;  // �κ��丮 ���� �迭
    public Seed item;     // �ν����Ϳ��� �Ҵ�� Seed ��ü

    Dictionary<string, Slot> itemSlotDictionary; // ������ �̸��� ������ �����ϴ� ��ųʸ�
    Dictionary<string, int> stoneTreeCnt;        // Stone, Tree ��� �����ϴ� ��ųʸ�

    void Start()
    {
        // �޸� �Ҵ�
        itemSlotDictionary = new Dictionary<string, Slot>();
        stoneTreeCnt = new Dictionary<string, int>();

        // ���� �ʱ� ����
        stoneTreeCnt["Stone"] = 0;
        stoneTreeCnt["Tree"] = 0;

        // ���� �迭 �ʱ�ȭ
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
            Debug.Log($"inventory: �߸��� �Ǽ����� �߰��Դϴ�.(�̸� �̽� {name})");
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
        // Stone, Tree ��ȯ�Լ�
        if (!stoneTreeCnt.ContainsKey(constructionMaterial))
        {
            Debug.Log("�߸��� Tree, Stone �̸��Դϴ�.");
            return 0;
        }

        return stoneTreeCnt[constructionMaterial];
    }

    public Item GetSeed()
    {
        foreach (var inven in slots)
        {
            // ���Կ��� �������� �˻��ϴ� ���� �߰� �ʿ�
        }
        return null;
    }
}
