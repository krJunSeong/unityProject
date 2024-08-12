using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance { get; private set; }
    [SerializeField] GameObject pan_Farm;// Farm pannel
    [SerializeField] Slot[] farmSlots;      // Farm farmSlots
    [SerializeField] List<Slot> playerSeedSlots; // �÷��̾�� seed�� ���õ� ���Ե�
    [SerializeField] FarmPlot[] farmPlots;    // ���� ����

    [SerializeField] Dictionary<string, Slot> connectSlot; // �κ� ������ ���� ���� �ձ�
    private int sum = 0;             // ���� ���� ���� �� ������ ���� ����
    private int useablePlotsCnt = 0; // ��밡���� ���� Plot�� ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    void Init()
    {
        pan_Farm = GameObject.Find("pan_ Farm");
        pan_Farm.SetActive(false);
        connectSlot = new Dictionary<string, Slot>();
        farmPlots = GameObject.Find("Farm_Piles").GetComponentsInChildren<FarmPlot>();
        useablePlotsCnt = GetPlotisEmptyCnt();
        //farmSlots = pan_Farm?.GetComponentsInChildren<FarmSlot>();
    }

    void InsertSeedToSlot()
    {
        // �κ��丮 -> Farm ���Կ� �ű�� �Լ�
        // 1. �κ��丮���� ������ OR ������ �޴´�
        Inventory inven = GameManager.Instance.GetPlayerInventory();

        // 2. Seed ���Ե鸸 �߷�����
        if (playerSeedSlots != null) playerSeedSlots.Clear();
        
        playerSeedSlots = new List<Slot>();

        if(farmSlots is null) { Debug.Log($"{farmSlots} farmSlots nullptr"); return; }

        // ���Կ��� �������� �ű��.
        // �κ� ������ ã�� �� ��ġ�Ǵ� ���԰� �ش� ��ġ�� ã�ƾ� �Ѵ�.
        foreach(var slot in inven.slots)
        {
            if (slot.GetItem() == null) continue;
            if (slot.GetItem().itemData.type == ItemData.ItemType.SEED)
            {
                playerSeedSlots.Add(slot);
            }
        }
        // 3. FarmManager�� ���Ե鿡 ���Ե��� �־��ش�
        foreach(var playerInventorySlot in playerSeedSlots)
        {
            Slot farmSlot = GetEmptySlot();

            farmSlot.gameObject.SetActive(true);
            farmSlot.SetItemData(playerInventorySlot.GetItem(), playerInventorySlot.GetCount());

            if (connectSlot.ContainsKey(farmSlot.name)) connectSlot[farmSlot.name] = null;

            // ConnectSlot[farmSlot �̸�] : [�÷��̾� �κ��丮 ����]
            connectSlot[farmSlot.name] = playerInventorySlot;
        }
        #region
        // ���� �ڵ�. ���� �߰��ؼ� ����
        /*for(int i = 0; i < selectedSlots.Count; i++)
        //{
        //    if (farmSlots[i].GetItem() == null) // ���� �߰�) �ش� slot�� �� ������ ���� ���Կ� �����ؾ� �ϴµ�, �� ���� ã�� �Լ� �ʿ��� ��
        //    {
        //        farmSlots[i].gameObject.SetActive(true);
        //        farmSlots[i].SetItemData(selectedSlots[i].GetItem(), selectedSlots[i].GetCount());
        //
        //        // ���߿� �����ϱ� ���� �κ��丮�� ���԰� �����Ѵ�
        //        //      ����� ���� �ʱ�ȭ
        //        //      farmSlot�� 0��°���� �����ؼ� ����ȴ�.
        //        //      ["Farm 0��° ������ �̸�"] = [List�� ����Ű�� ����, Inven�� ����]
        //        if (connectSlot.ContainsKey(farmSlots[i].name)) connectSlot[farmSlots[i].name] = null;
        //
        //        connectSlot[farmSlots[i].name] = selectedSlots[i];
        //    }
        }
        */
        #endregion
    }

    public void DigFarm()
    {
        connectSlot.Clear();    // ����ó��, �۹� ���� �� �ٽ� ������ư ������ �κ��丮 ���� ¡�� ����

        // ����ó��: Plot �������� ������ ����ó��
        sum = 0;
        foreach (var s in farmSlots)
        {
            sum += int.Parse(s.itemCount.text);
        }
        if(sum > farmPlots.Length) { Debug.Log($"plots �������� �� ���� �����߽��ϴ�."); return; }

        /* ���۹� �ɱ�
            1. �κ��丮 ���� ����
                 - ���۹� ������ ���� �����ؼ� 1 �̻��� ��� �ɰ�, �κ��丮���� �����Ѵ�.
        
            2. ���۹� �ɱ�
                - plot�鿡 Seed�� �ָ�, �� �����͸� �̿��ؼ� Plot���� string�� �����ؼ� �����.
        */
        foreach (var s in farmSlots) // FarmSlot
        {
            if (!s.gameObject.activeSelf || s.GetItem() == null) continue; // ��� ������ ��ŵ

            int strTemp = int.Parse(s.itemCount.text);  // Text���� ���� ������ ���� ����´�
            if (strTemp > 0)    // �ش� ���ڰ� 0 �̻��̶�� �κ��丮���� ���� �����Ѵ�.
            {
                connectSlot[s.name].DecreaceItemCnt(strTemp);
            }

            // Dig Plot�� �ɱ� ����
            // ������ŭ Plot�� �����ϱ�
            for(int i = 0; i < strTemp; i++)
            {
                // 1. Plots���� ���� �۵������� return ���ִ� �Լ� �ʿ�.
                GetPlotIsEmpty().PlantSeed((Seed)s.GetItem());
            }
        }

        
    }
    public void OpenSeedInventory()
    {
        InsertSeedToSlot();
        pan_Farm.SetActive(true);
        for (int i = 0; i < farmSlots.Length; i++)
        {
            farmSlots[i].gameObject.SetActive(true);
        }
        UIManager.Instance.CloesUies();
    }

    public void CloseSeedInventory()
    {
        pan_Farm.SetActive(false);
    }

    private Slot GetEmptySlot()
    {
        foreach (var slot in farmSlots)
        {
            if (slot.IsEmpty())
            {
                return slot;
            }
        }
        return null;
    }
    private FarmPlot GetPlotIsEmpty()
    {
        // ����ִ� plot ��ȯ�ϴ� �Լ�
        foreach (var plot in farmPlots)
        {
            if(plot.IsEmpty())
            {
                return plot;
            }
        }
        return null;
    }

    private int GetPlotisEmptyCnt()
    {
        int tmp = 0;
        foreach (var plot in farmPlots)
        {
            if (plot.IsEmpty())
            {
                tmp++;
            }
        }

        return tmp;
    }

    public void CancelBtn()
    {
        pan_Farm.SetActive(false);
        // **�ʿ�**: ī�޶� �÷��̾�� �ٽ� �ִ� �ڵ� �ʿ�!
    }
}
