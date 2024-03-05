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
        //  1) BGM ���� ������Ʈ ����, ���̱�
        //      1-1) BGM ������Ʈ�� 1���� Sound Component ����
        //      1-2) loop True, volume, clip �ۼ� �� play ���ش�.
        GameObject bgmObject = new GameObject("BGM");
        
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClips[(int)Bgm.COSTALTOWN];
        bgmPlayer.Play();

        //  2) SFX ���� ������Ʈ ����
        //      2-1) soundManager �Ʒ��� ������Ʈ���� �ٿ��ش�
        //      2-2) ������ҽ� channel ��ŭ �ٿ��ش�.
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
