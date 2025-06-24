using System.Collections;                          // Unity �⺻ �÷��� ��� ���
using System.Collections.Generic;                  // ���׸� �÷���(List ��) ���
using UnityEngine;                                 // Unity ������ �ٽ� ��� ���

public class AudioManager : MonoBehaviour          // ����� ��� ������ �����ϴ� �̱��� Ŭ����
{
    public static AudioManager instance;           // �ٸ� ��ũ��Ʈ���� �����ϱ� ���� �̱��� �ν��Ͻ�

    [Header("#BGM")]                               // �ν����Ϳ��� BGM �׸� ����
    public AudioClip bgmClip;                      // ������� ����� Ŭ��
    public float bgmVolume;                        // ����� ���� ũ��
    AudioSource bgmPlayer;                         // BGM ����� AudioSource ������Ʈ
    AudioHighPassFilter bgmEffect;                 // ������ ȿ�� (������ �� ���� ȿ����)

    [Header("#SFX")]                               // �ν����Ϳ��� ȿ���� �׸� ����
    public AudioClip[] sfxClips;                   // ȿ���� Ŭ�� �迭 (���� ȿ���� ����)
    public float sfxVolume;                        // ȿ���� ����
    public int channels;                           // ȿ������ ���ÿ� ����� �� �ִ� ä�� ��
    AudioSource[] sfxPlayesr;                      // ȿ������ AudioSource �迭
    int channelIndex;                              // ���� ����� ä�� �ε��� �����

    public enum Sfx { Dead, Hit, Levelup = 3, Lose, Melee, Range = 7, Select, Win }
    // ȿ������ �ĺ��ϱ� ���� ������. �迭 �ε����ε� ����

    void Awake()
    {
        instance = this;                           // �̱��� ���� ����
        Init();                                    // ����� �ʱ�ȭ �Լ� ȣ��
    }

    void Init()
    {
        // BGM�� AudioSource ���� �� ����
        GameObject bgmObject = new GameObject("BgmPlayer"); // ���ο� ������Ʈ ����
        bgmObject.transform.parent = transform;             // �� AudioManager ������Ʈ�� �ڽ����� ����
        bgmPlayer = bgmObject.AddComponent<AudioSource>();  // AudioSource �߰�
        bgmPlayer.playOnAwake = false;                      // ���� �� �ڵ� ��� X
        bgmPlayer.loop = true;                              // �ݺ� ��� O
        bgmPlayer.volume = bgmVolume;                       // ������ ���� ����
        bgmPlayer.clip = bgmClip;                           // �̸� ������ BGM Ŭ�� ����
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>(); // ���� ī�޶��� ����� ���� ����

        // SFX�� AudioSource ���� �� ���� (��Ƽä�� �뵵)
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayesr = new AudioSource[channels];             // ä�� ����ŭ �迭 ����

        for (int index = 0; index < sfxPlayesr.Length; index++)
        {
            sfxPlayesr[index] = sfxObject.AddComponent<AudioSource>(); // ä�θ��� AudioSource �߰�
            sfxPlayesr[index].playOnAwake = false;
            sfxPlayesr[index].bypassListenerEffects = true; // ������ ȿ�� ���� (ȿ������ �����ϰ�)
            sfxPlayesr[index].volume = sfxVolume;            // ȿ���� ���� ����
        }
    }

    public void PlayBgm(bool isPlay)               // BGM�� ��� �Ǵ� �����ϴ� �Լ�
    {
        if (isPlay)
        {
            bgmPlayer.Play();                      // BGM ����
        }
        else
        {
            bgmPlayer.Stop();                      // BGM ����
        }
    }

    public void EffectBgm(bool isPlay)             // ������ ȿ�� �ѱ�/���� (��: ������ �� ���԰�)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)                   // ȿ���� ��� �Լ�
    {
        for (int index = 0; index < sfxPlayesr.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayesr.Length; // ���� ä�� ����

            if (sfxPlayesr[loopIndex].isPlaying)
                continue;                           // �̹� ��� ���̸� ���� ä�η�

            int ranIndex = 0;                       // ȿ������ ���� Ŭ�� �� ������ �� �ε��� ����

            if (sfx == Sfx.Hit || sfx == Sfx.Melee) // Ÿ�ݷ� ������ ���
            {
                ranIndex = Random.Range(0, 2);      // �ε��� ���� ���� (�پ��� ��Ʈ�� ����)
            }

            channelIndex = loopIndex;               // ���� ä�� ���
            sfxPlayesr[loopIndex].clip = sfxClips[(int)sfx]; // ȿ���� Ŭ�� ����
            sfxPlayesr[loopIndex].Play();           // ���
            break;                                  // �� ���� ����ϰ� �ݺ� ����
        }
    }
}