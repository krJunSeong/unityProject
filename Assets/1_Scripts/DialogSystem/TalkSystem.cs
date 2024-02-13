using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkSystem : MonoBehaviour
{
    public static TalkSystem Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] GameObject uiTalkPanel;
    [SerializeField] Image playerImg;
    [SerializeField] Image cpuImg;

    [SerializeField] Text talkerName;
    [SerializeField] Text script;


    public bool isTalking = false; // ��ȭ���ΰ�?
    bool isNext = false;    // ����Ű �Է´��
    bool isTalkEnd = false; // ��ȭ�� ���������� ���°�?
    int scriptLineNum = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Talk(0);
    }

    void SetUI(bool b)
    {
        uiTalkPanel.SetActive(b);
    }
    
    void Talk(int chapter)
    {
        // 1. ��ũ��Ʈ �ٿ�
        ChatData chData = DataManager.Instance.GetChapterScript(chapter, scriptLineNum);
        
        if (chData == null)
        {
            // 1-1. ��ũ��Ʈ�� �����ų�, ��ũ��Ʈ�� ���� ��� UI �����ش�.
            // (���� �� ��) 1-2. ���� ���μ������� ��� Off �Ѵ�
            SetUI(false);
            scriptLineNum = 0;
            return;
        }

        // 2. ��ũ��Ʈ�� �ִٸ� ����Ѵ�.
        script.text = "";
        talkerName.text = chData.name;
        StartCoroutine(TypeWriter(chData.script.Replace("_", ",")));
        //script.text = chData.script.Replace("*", ",");
        scriptLineNum++;

        if(talkerName.text == "������")
        {
            // ���ΰ��̶�� ���ΰ� �̹��� ����
            playerImg.sprite = DataManager.Instance.GetCharacterTalkImage(int.Parse(chData.image));
            playerImg.enabled = true;
            cpuImg.enabled = false;
        }
        else
        {
            // ���ΰ��� �ƴ϶�� CPU �̹��� ����
            cpuImg.sprite = DataManager.Instance.GetCharacterTalkImage(int.Parse(chData.image));
            playerImg.enabled = false;
            cpuImg.enabled = true;
        }
    }

    IEnumerator TypeWriter(string s)
    {
        int cnt = 0;
        while (cnt < s.Length)
        {
            script.text += s[cnt++];

            yield return new WaitForSeconds(0.1f);
        }
    }
}
