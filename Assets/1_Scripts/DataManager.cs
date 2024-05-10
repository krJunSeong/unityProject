using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChatData
{
    public string name   {get;}
    public string image  {get;}
    public string script {get;}

    public ChatData(string n, string i, string sc)
    {
        name = n;
        image = i;
        script = sc;
    }
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [SerializeField] Sprite[] imageMap = new Sprite[30];

    List<ChatData> storyData;
    Dictionary<int, List<ChatData>> dChapterStoryData { get; set; }

    // ----------- Item --------------------
    Dictionary<string, Item> dItemData;
    [SerializeField] Sprite[] ItemImage;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        dChapterStoryData = csvReaderTest.Instance.LoadChatData("StoryChat");
    }

    public ChatData GetChapterScript(int chapter, int lineNum)
    {
        // 같거나 크면 해당 스크립트의 끝
        if (dChapterStoryData[chapter].Count <= lineNum) return null;

        return dChapterStoryData[chapter][lineNum];
    }

    public Sprite GetCharacterTalkImage(int num)
    {
        return imageMap[num];
    }

    void AddItemData()
    {
        //dItemData["Tree"] = new Item("Tree", );
    }

    public Item GetItemData(string str) { return dItemData[str]; }
}
