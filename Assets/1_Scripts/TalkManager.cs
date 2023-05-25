using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;

    public Sprite[] portraitArr;
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();

        GenerateData();
    }

    void GenerateData()
    {
        //Talk Data
        // NPC A: 1000
        talkData.Add(1000, new string[] { "안녕?:0", "이곳에 처음 왔구나?:1" });
        talkData.Add(100, new string[] { "평범한 나무상자다." });
        talkData.Add(200, new string[] { "200번 책상이다" });
        talkData.Add(2000, new string[] { "2 000번 책상이다:0" });

        //Quest Talk, QuestID + NPCID
        talkData.Add(10 + 1000, new string[] {"어서 와.:0",
                                              "이 마을에 놀라운 전설이 있다는데:1",
                                              "오른쪽 호수 쪽에 루도가 알려줄거야.:0"});

        talkData.Add(11 + 2000, new string[] {"어서 와.:0",
                                              "이 마을에 놀라운 전설이 있다는데:1",
                                              "오른쪽 호수 쪽에 루도가 알려줄거야.:0"});
        // Portrait Data
        //
        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);
        portraitData.Add(1000 + 2, portraitArr[2]);
        portraitData.Add(1000 + 3, portraitArr[3]);
    }

    public string GetTalk(int id, int talkIndex) // id, string 번호
    {
        if(talkIndex == talkData[id].Length) 
            return null;
        else return talkData[id][talkIndex];
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }
}
