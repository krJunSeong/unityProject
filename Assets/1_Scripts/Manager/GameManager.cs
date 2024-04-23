using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    public QuestManager questManager;
    public TalkManager talkManager;

    // ------------ Talk System ------------
    public Text talkText;
    public GameObject talkPanel;
    public Image portraitImg;
    public int talkIndex = 0;
    public bool isAction;

    // ------------ Player ------------
    [SerializeField] GameObject player;
    public GameObject scanObject;

    private void Start()
    {
        InitPlayerMoveStartPos();
    }

    public void TalkAction(GameObject scanObj)
    {
        ObjectData objData = scanObj.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNpc);
    }

    void Talk(int id, bool isNpc)
    {
        int questTalkIndex = questManager.GetQuestTalkIndex(id);
        string talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        if(talkData == null)
        {
            talkPanel.SetActive(false);
            talkIndex = 0;
            isAction = false;
            questManager.CheckQuest(id);
            return;
        }

        if(isNpc)
        {
            talkText.text = talkData.Split(':')[0];

            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);
        }
        else
        {
            talkText.text = talkData;

            portraitImg.color = new Color(1, 1, 1, 0);
        }
        talkPanel.SetActive(true);
        isAction = true;
        talkIndex++;

        //int questTalkIndex = questManager.GetQuestTalkIndex(id);
    }

    void InitPlayerMoveStartPos()
    {
        player = GameObject.Find("CreateUnitychan");
        player.transform.position = GameObject.Find("GameStartPosition").transform.position;
        player.GetComponent<Rigidbody>().useGravity = true;
    }
}
