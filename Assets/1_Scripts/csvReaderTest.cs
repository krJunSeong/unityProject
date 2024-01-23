using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class csvReaderTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoadItemTextData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Dictionary<int, List<string>> LoadData(string path, int startRow)
    {
        // path: 경로, col: 열 개수
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        string temp = textAsset.text.Replace("\r\n", "\n");
        string[] row = temp.Split('\n');

        // [Data, Data, Data], [Data, Data, Data]
        for (int i = 2; i < row.Length; i++)
        {
            List<string> data = row[i].Split(',').ToList();

            if (data.Count <= 1) continue;
        }

        return null;
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