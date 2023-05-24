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
        talkData.Add(1000, new string[] { "�ȳ�", "�̰��� ó�� �Ա���?" });
        talkData.Add(100, new string[] { "����� �������ڴ�." });
        talkData.Add(200, new string[] { "200�� å���̴�" });

        //portraitData.Add(1000 + 0, );
        //portraitData.Add(1000 + 1, );
        //portraitData.Add(1000 + 2, );
        //portraitData.Add(1000 + 3, );
    }

    public string GetTalk(int id, int talkIndex) // id, string ��ȣ
    {
        if(talkIndex == talkData[id].Length) 
            return null;
        else return talkData[id][talkIndex];
    }
}
