using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class TalkSystem : MonoBehaviour
{
    public static TalkSystem Instance { get; private set; }

    [SerializeField] GameObject uiTalkPanel;
    [SerializeField] Image playerImg;
    [SerializeField] Image cpuImg;

    [SerializeField] Text talkerName;
    [SerializeField] Text script;
    [SerializeField] DepthOfField depthOfField;

    public bool isTalking = false; // ��ȭ���ΰ�?
    int scriptLineNum = 0;

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

    private void Start()
    {
        depthOfField = GameObject.Find("Main Camera").GetComponent<PostProcessVolume>().profile.GetSetting<DepthOfField>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Talk(0);
    }
    
    void Talk(int chapter)
    {
        // 1. ��ũ��Ʈ �ٿ�
        ChatData chData = DataManager.Instance.GetChapterScript(chapter, scriptLineNum);
        
        if (chData == null)
        {
            // 1-1. ��ũ��Ʈ�� �����ų�, ��ũ��Ʈ�� ���� ��� UI �����ش�.
            // (���� �� ��) 1-2. ���� ���μ������� ��� Off �Ѵ�
            ActivePannel(false);
            scriptLineNum = 0;
            return;
        }

        // 2. ��ũ��Ʈ�� �ִٸ� ����Ѵ�.
        ActivePannel(true);
        ScriptControll(chData);
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

    void ScriptControll(ChatData _chData)
    {
        StopAllCoroutines();
        script.text = "";
        talkerName.text = _chData.name;
        StartCoroutine(TypeWriter(_chData.script.Replace("_", ",")));
        scriptLineNum++;

        if (talkerName.text == "������")
        {
            // ���ΰ��̶�� ���ΰ� �̹��� ����
            playerImg.sprite = DataManager.Instance.GetCharacterTalkImage(int.Parse(_chData.image));
            playerImg.enabled = true;
            cpuImg.enabled = false;
        }
        else
        {
            // ���ΰ��� �ƴ϶�� CPU �̹��� ����
            cpuImg.sprite = DataManager.Instance.GetCharacterTalkImage(int.Parse(_chData.image));
            playerImg.enabled = false;
            cpuImg.enabled = true;
        }
    }
    void ActivePannel(bool b)
    {
        uiTalkPanel.SetActive(b);
        depthOfField.active = b;
    }
}
