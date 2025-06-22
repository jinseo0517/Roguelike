using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("#BGM")]    //�������
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]    //ȿ����
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayesr;
    int channelIndex;

    public enum Sfx { Dead, Hit, Levelup=3, Lose, Melee, Range=7, Select, Win }

    void Awake()
    {
        instance = this;    //�ڱ� �ڽ��� instance�� �����ؼ� �̱���ó�� ���
        Init();     //�ʱ�ȭ �Լ� ȣ��
    }

    void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayesr = new AudioSource[channels];

        for (int index=0; index < sfxPlayesr.Length; index++)
        {
            sfxPlayesr[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayesr[index].playOnAwake = false;
            sfxPlayesr[index].bypassListenerEffects = true;
            sfxPlayesr[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }
    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayesr.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayesr.Length;

            if (sfxPlayesr[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            sfxPlayesr[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayesr[loopIndex].Play();
            break;
        }

    }
}
