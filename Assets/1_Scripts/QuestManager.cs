using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;

    // ID, Data
    Dictionary<int, QuestData> questList;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        questList.Add(1000, new QuestData("첫 마을 방문", new int[] { 1000, 2000 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        return questId;
    }
}
