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
        // ������ �߰� �Լ�

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
                // 1.�ش� ���ڸ��� ������ �־��ֱ�.
                Debug.Log("�� �ڸ��� �ֽ��ϴ�.");
                    // 0. ������ �����Ϳ� �ؽ�ó ������ �ִ´� or
                    //    ������ �Ŵ����� ���� ��ųʸ��� �����Ѵ�. <- �̰� ���� ��. ��� �����ۿ� ������ ������ �������.
                    // 1. ������ �Ŵ������� "silverCoins"�� �ؽ�ó ������ ���� �´�.
                    // 2. ���Կ� �־��ش�.
                inputGameObject.SetActive(false);
                return;
            }
        }
        // 3. �� ������ ���ٸ� �α� ���.
        Debug.Log("�κ��丮�� �� �ڸ��� �����ϴ�.");
    }

    public int GetItemInInventory(string name)
    {
        // �κ��丮 ���� ������ ���� ����ϴ� �Լ�
        // �ش� ������ ������ 0�� ����
        if (inventory.ContainsKey(name)) return inventory[name];

        return 0;
    }
}
