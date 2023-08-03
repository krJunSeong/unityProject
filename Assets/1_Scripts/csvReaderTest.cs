using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void LoadItemTextData() //������ȭ ���Ѽ� �ҷ���
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