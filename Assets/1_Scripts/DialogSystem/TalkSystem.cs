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

    public bool isTalking = false; // 대화중인가?
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
        // 1. 스크립트 다운
        ChatData chData = DataManager.Instance.GetChapterScript(chapter, scriptLineNum);
        
        if (chData == null)
        {
            // 1-1. 스크립트가 끝났거나, 스크립트가 없을 경우 UI 없애준다.
            // (아직 안 함) 1-2. 볼륨 프로세서에서 블룸 Off 한다
            ActivePannel(false);
            scriptLineNum = 0;
            return;
        }

        // 2. 스크립트가 있다면 출력한다.
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

        if (talkerName.text == "코하쿠")
        {
            // 주인공이라면 주인공 이미지 변경
            playerImg.sprite = DataManager.Instance.GetCharacterTalkImage(int.Parse(_chData.image));
            playerImg.enabled = true;
            cpuImg.enabled = false;
        }
        else
        {
            // 주인공이 아니라면 CPU 이미지 변경
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
