using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(1000, new string[] { "�ȳ�", "�̰��� ó�� �Ա���?" });
        talkData.Add(100, new string[] { "����� �������ڴ�." });
        talkData.Add(200, new string[] { "200�� å���̴�" });
    }

    public string GetTalk(int id, int talkIndex) // id, string ��ȣ
    {
        if(talkIndex == talkData[id].Length) 
            return null;
        else return talkData[id][talkIndex];
    }
}
