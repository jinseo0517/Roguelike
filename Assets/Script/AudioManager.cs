using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("#BGM")]    //배경음악
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]    //효과음
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayesr;
    int channelIndex;

    public enum Sfx { Dead, Hit, Levelup=3, Lose, Melee, Range=7, Select, Win }

    void Awake()
    {
        instance = this;    //자기 자신을 instance에 저장해서 싱글톤처럼 사용
        Init();     //초기화 함수 호출
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
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
