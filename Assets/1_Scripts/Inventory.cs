using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    Dictionary<string, List<ItemBase>> inventory;
    Dictionary<string, ItemBase[]> seedPocket = new Dictionary<string, ItemBase[]>();
    Dictionary<string, int> itemCnt = new Dictionary<string, int>();
    
    void Start()
    {
        //Slot[] s = GetComponentsInChildren<Slot>(true);
        //slots.AddRange(s);
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Init()
    {
        inventory = new Dictionary<string, List<ItemBase>>();
        seedPocket = new Dictionary<string, ItemBase[]>();
        itemCnt = new Dictionary<string, int>();
    }
    public void AddItem(string item, int num)
    {
        // ������ �߰� �Լ�
        if(inventory.ContainsKey(item))
        {
            //inventory[item] += num;
        }
        //else inventory.Add(item, num);
    }

    public void AddItem(GameObject inputGameObject)
    {
        //foreach(Slot slot in slots)
        //{
        //    if (!slot.GetNowUsing())
        //    {
        //        // 1.�ش� ���ڸ��� ������ �־��ֱ�.
        //        Debug.Log("�� �ڸ��� �ֽ��ϴ�.");
        //            // 0. ������ �����Ϳ� �ؽ�ó ������ �ִ´� or
        //            //    ������ �Ŵ����� ���� ��ųʸ��� �����Ѵ�. <- �̰� ���� ��. ��� �����ۿ� ������ ������ �������.
        //            // 1. ������ �Ŵ������� "silverCoins"�� �ؽ�ó ������ ���� �´�.
        //            // 2. ���Կ� �־��ش�.
        //        inputGameObject.SetActive(false);
        //        return;
        //    }
        //}
        // 3. �� ������ ���ٸ� �α� ���.
        Debug.Log("�κ��丮�� �� �ڸ��� �����ϴ�.");
    }

    public int GetItemInInventory(ItemBase item)
    {
        // �κ��丮 ���� ������ ���� ����ϴ� �Լ�
        // �ش� ������ ������ 0�� ����
        //if (inventory.ContainsKey(item.name)) return inventory[item.name];

        return 0;
    }

    public ItemBase GetSeed()
    {
        foreach(var inven in inventory)
        {
        //    if(inven.Value.)
        }
        return null;
    }
}
