using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
    }

    void SetVolume(float f)
    {
        // �μ��� 1 �̻����� ���� ��� 100�� �������� �ٿ��ش�.
        // ex) 94.3 = 0.94

        /*
        if (f > 1.0f) f = ((f / 100.0f) + (f % 100.0f));

        audioSource.volume = f;
        */
    }

    void SetAudioClip(AudioClip clip)
    {

    }
}
