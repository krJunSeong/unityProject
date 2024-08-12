using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance { get; private set; }
    [SerializeField] GameObject pan_Farm;// Farm pannel
    [SerializeField] Slot[] farmSlots;      // Farm farmSlots
    [SerializeField] List<Slot> playerSeedSlots; // 플레이어에서 seed만 선택된 슬롯들
    [SerializeField] FarmPlot[] farmPlots;    // 심을 땅들

    [SerializeField] Dictionary<string, Slot> connectSlot; // 인벤 차감을 위한 슬롯 잇기
    private int sum = 0;             // 씨앗 심을 것이 몇 개인지 세기 위함
    private int useablePlotsCnt = 0; // 사용가능한 남은 Plot들 개수

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
        // 인벤토리 -> Farm 슬롯에 옮기는 함수
        // 1. 인벤토리에서 아이템 OR 슬롯을 받는다
        Inventory inven = GameManager.Instance.GetPlayerInventory();

        // 2. Seed 슬롯들만 추려낸다
        if (playerSeedSlots != null) playerSeedSlots.Clear();
        
        playerSeedSlots = new List<Slot>();

        if(farmSlots is null) { Debug.Log($"{farmSlots} farmSlots nullptr"); return; }

        // 슬롯에서 아이템을 옮긴다.
        // 인벤 슬롯을 찾을 때 배치되는 슬롯과 해당 위치를 찾아야 한다.
        foreach(var slot in inven.slots)
        {
            if (slot.GetItem() == null) continue;
            if (slot.GetItem().itemData.type == ItemData.ItemType.SEED)
            {
                playerSeedSlots.Add(slot);
            }
        }
        // 3. FarmManager의 슬롯들에 슬롯들을 넣어준다
        foreach(var playerInventorySlot in playerSeedSlots)
        {
            Slot farmSlot = GetEmptySlot();

            farmSlot.gameObject.SetActive(true);
            farmSlot.SetItemData(playerInventorySlot.GetItem(), playerInventorySlot.GetCount());

            if (connectSlot.ContainsKey(farmSlot.name)) connectSlot[farmSlot.name] = null;

            // ConnectSlot[farmSlot 이름] : [플레이어 인벤토리 슬롯]
            connectSlot[farmSlot.name] = playerInventorySlot;
        }
        #region
        // 원래 코드. 문제 발견해서 수정
        /*for(int i = 0; i < selectedSlots.Count; i++)
        //{
        //    if (farmSlots[i].GetItem() == null) // 문제 발견) 해당 slot이 차 있으면 다음 슬롯에 연결해야 하는데, 빈 슬롯 찾는 함수 필요할 듯
        //    {
        //        farmSlots[i].gameObject.SetActive(true);
        //        farmSlots[i].SetItemData(selectedSlots[i].GetItem(), selectedSlots[i].GetCount());
        //
        //        // 나중에 차감하기 위해 인벤토리의 슬롯과 연결한다
        //        //      연결된 슬롯 초기화
        //        //      farmSlot은 0번째부터 시작해서 연결된다.
        //        //      ["Farm 0번째 슬롯의 이름"] = [List가 가리키는 슬롯, Inven의 슬롯]
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
        connectSlot.Clear();    // 예외처리, 작물 생성 후 다시 결정버튼 누르면 인벤토리 강제 징수 막기

        // 예외처리: Plot 개수보다 많으면 예외처리
        sum = 0;
        foreach (var s in farmSlots)
        {
            sum += int.Parse(s.itemCount.text);
        }
        if(sum > farmPlots.Length) { Debug.Log($"plots 개수보다 더 많이 선택했습니다."); return; }

        /* 농작물 심기
            1. 인벤토리 슬롯 조사
                 - 농작물 슬롯을 전부 조사해서 1 이상인 경우 심고, 인벤토리에서 차감한다.
        
            2. 농작물 심기
                - plot들에 Seed를 주면, 그 데이터를 이용해서 Plot에서 string을 조사해서 만든다.
        */
        foreach (var s in farmSlots) // FarmSlot
        {
            if (!s.gameObject.activeSelf || s.GetItem() == null) continue; // 비어 있으면 스킵

            int strTemp = int.Parse(s.itemCount.text);  // Text에서 가감 차감한 숫자 갖고온다
            if (strTemp > 0)    // 해당 숫자가 0 이상이라면 인벤토리에서 가감 진행한다.
            {
                connectSlot[s.name].DecreaceItemCnt(strTemp);
            }

            // Dig Plot에 심기 진행
            // 개수만큼 Plot에 진행하기
            for(int i = 0; i < strTemp; i++)
            {
                // 1. Plots에서 지금 작동중인지 return 해주는 함수 필요.
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
        // 비어있는 plot 반환하는 함수
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
        // **필요**: 카메라 플레이어에게 다시 주는 코드 필요!
    }
}
