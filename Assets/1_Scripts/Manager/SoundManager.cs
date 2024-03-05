using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{ 
    public static SoundManager Instance;
    public enum Sfx {BUTTON, SWING1, SWING2, SWING3,
        KOHAKU_ATTACK1, KOHAKU_ATTACK2, KOHAKU_ATTACK3, KOHAKU_DEAD,
        KOHAKU_HIT1, KOHAKU_HIT2, KOHAKU_WINSOUND1, KOHAKU_WINSOUND2,
        ENEMY_ATTACK, ENEMY_DEAD, ENEMY_HIT1, ENEMY_HIT2
    };
    public enum Bgm {COSTALTOWN };

    [SerializeField] AudioClip[] bgmClips;
    [SerializeField] AudioClip[] sfxClips;

    AudioSource bgmPlayer;
    AudioSource[] sfxPlayers;

    [SerializeField] int channels;
    [SerializeField] float bgmVolume = 0.7f;
    [SerializeField] float sfxVolume = 0.3f;
    int usedLastChannel;

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

    void Start()
    {
        Init();
    }

    void Init()
    {
        //  1) BGM 사운드 오브젝트 생성, 붙이기
        //      1-1) BGM 오브젝트에 1개의 Sound Component 장착
        //      1-2) loop True, volume, clip 작성 후 play 해준다.
        GameObject bgmObject = new GameObject("BGM");
        
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClips[(int)Bgm.COSTALTOWN];
        bgmPlayer.Play();

        //  2) SFX 사운드 오브젝트 생성
        //      2-1) soundManager 아래로 오브젝트들을 붙여준다
        //      2-2) 오디오소스 channel 만큼 붙여준다.
        GameObject sfxObject = new GameObject("SFX");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int i = 0; i < channels; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + usedLastChannel) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying) continue;

            usedLastChannel = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
