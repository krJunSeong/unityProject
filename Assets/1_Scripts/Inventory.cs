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
    public void GetItem(GameObject inputGameObject)
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
}
