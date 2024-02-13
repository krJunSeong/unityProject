using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class csvReaderTest : MonoBehaviour
{
    private static csvReaderTest instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public static csvReaderTest Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("csv Reader는 nullptr이다.");
                return null;
            }

            return instance;

        }
    }

    // 잘못된 완성
    // 필요한 것: 데이터만 쫙쫙 뽑아서 넘겨줄 필요가 있음.
    public Dictionary<int, List<ChatData>> LoadChatData(string path = "TextData/ExcelFile/storyChat")
    {
        // path: 경로, Resources에 있어야 함
        var textAsset = Resources.Load<TextAsset>(path);
        string temp = textAsset.text.Replace("\r\n", "\n");
        string[] row = temp.Split('\n');
        Dictionary<int, List<ChatData>> dAnswer = new Dictionary<int, List<ChatData>>();

        // 1 row: 자료형
        // 2 row: 데이터 이름
        // 3 row: [Data, Data, Data], [Data, Data, Data]
        for (int i = 2; i < row.Length; i++)
        {
            //data: [chapter], [name], [image], [script]
            List<string> data = row[i].Split(',').ToList();
           
            if (data.Count <= 1) continue;

            ChatData lTemp = new ChatData(data[1], data[2], data[3]);

            // 1. ChatData 구조체에 데이터 삽입
            // 2. 아래와 같이 Dictionary 구성
            //    Chapter : ChatData1, ChatData2, ChatData3
            int iChapter = int.Parse(data[0]);
            if (dAnswer.ContainsKey(iChapter))
            {
                dAnswer[iChapter].Add(lTemp);
            }
            else dAnswer.Add(iChapter, new List<ChatData>() { lTemp });
        }

        return dAnswer;
    }

    void LoadItemTextData() //데이터화 시켜서 불러옴
    {
        TextAsset textAsset = Resources.Load<TextAsset>("TextData/ItemData");

        string temp = textAsset.text.Replace("\r\n", "\n");
        string[] row = temp.Split('\n');

        for (int i = 1; i < row.Length; i++)
        {
            string[] data = row[i].Split(',');

            if (data.Length <= 1) continue;

            Debug.Log("0:" + data[0]);
            Debug.Log("1:" + data[1]);
            Debug.Log("2:" + data[2]);
        }
    }
}