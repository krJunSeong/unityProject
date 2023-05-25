using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;

    // ID, Data
    Dictionary<int, QuestData> questList;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        // Questname, NPCID[]
        questList.Add(10, new QuestData("마을 사람들과 대화하기.", new int[] { 1000, 1000 }));
        questList.Add(20, new QuestData("벌목하기.", new int[] { 1000, 1000 }));

    }

    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public void CheckQuest(int id) // npc id
    {
        if(id == questList[questId].npcId[questActionIndex]) 
            questActionIndex++;

        if (questActionIndex == questList[questId].npcId.Length)
            nextQuest();
    }

    void nextQuest()
    { 
        questId += 10;
        questActionIndex = 0;
    }
}
