using System.Collections;                          // Unity 기본 컬렉션 기능 사용
using System.Collections.Generic;                  // 제네릭 컬렉션(List 등) 사용
using UnityEngine;                                 // Unity 엔진의 핵심 기능 사용

public class AudioManager : MonoBehaviour          // 오디오 재생 전반을 관리하는 싱글톤 클래스
{
    public static AudioManager instance;           // 다른 스크립트에서 접근하기 위한 싱글톤 인스턴스

    [Header("#BGM")]                               // 인스펙터에서 BGM 항목 구분
    public AudioClip bgmClip;                      // 배경음악 오디오 클립
    public float bgmVolume;                        // 배경음 볼륨 크기
    AudioSource bgmPlayer;                         // BGM 재생용 AudioSource 컴포넌트
    AudioHighPassFilter bgmEffect;                 // 고역필터 효과 (레벨업 시 음향 효과용)

    [Header("#SFX")]                               // 인스펙터에서 효과음 항목 구분
    public AudioClip[] sfxClips;                   // 효과음 클립 배열 (여러 효과음 저장)
    public float sfxVolume;                        // 효과음 볼륨
    public int channels;                           // 효과음을 동시에 재생할 수 있는 채널 수
    AudioSource[] sfxPlayesr;                      // 효과음용 AudioSource 배열
    int channelIndex;                              // 다음 사용할 채널 인덱스 저장용

    public enum Sfx { Dead, Hit, Levelup = 3, Lose, Melee, Range = 7, Select, Win }
    // 효과음을 식별하기 위한 열거형. 배열 인덱스로도 사용됨

    void Awake()
    {
        instance = this;                           // 싱글톤 패턴 적용
        Init();                                    // 오디오 초기화 함수 호출
    }

    void Init()
    {
        // BGM용 AudioSource 생성 및 설정
        GameObject bgmObject = new GameObject("BgmPlayer"); // 새로운 오브젝트 생성
        bgmObject.transform.parent = transform;             // 이 AudioManager 오브젝트의 자식으로 설정
        bgmPlayer = bgmObject.AddComponent<AudioSource>();  // AudioSource 추가
        bgmPlayer.playOnAwake = false;                      // 시작 시 자동 재생 X
        bgmPlayer.loop = true;                              // 반복 재생 O
        bgmPlayer.volume = bgmVolume;                       // 지정한 볼륨 설정
        bgmPlayer.clip = bgmClip;                           // 미리 지정한 BGM 클립 설정
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>(); // 메인 카메라의 오디오 필터 연결

        // SFX용 AudioSource 여러 개 생성 (멀티채널 용도)
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayesr = new AudioSource[channels];             // 채널 수만큼 배열 생성

        for (int index = 0; index < sfxPlayesr.Length; index++)
        {
            sfxPlayesr[index] = sfxObject.AddComponent<AudioSource>(); // 채널마다 AudioSource 추가
            sfxPlayesr[index].playOnAwake = false;
            sfxPlayesr[index].bypassListenerEffects = true; // 리스너 효과 무시 (효과음은 선명하게)
            sfxPlayesr[index].volume = sfxVolume;            // 효과음 볼륨 설정
        }
    }

    public void PlayBgm(bool isPlay)               // BGM을 재생 또는 정지하는 함수
    {
        if (isPlay)
        {
            bgmPlayer.Play();                      // BGM 시작
        }
        else
        {
            bgmPlayer.Stop();                      // BGM 정지
        }
    }

    public void EffectBgm(bool isPlay)             // 고역필터 효과 켜기/끄기 (예: 레벨업 시 몰입감)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)                   // 효과음 재생 함수
    {
        for (int index = 0; index < sfxPlayesr.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayesr.Length; // 다음 채널 선택

            if (sfxPlayesr[loopIndex].isPlaying)
                continue;                           // 이미 재생 중이면 다음 채널로

            int ranIndex = 0;                       // 효과음이 여러 클립 중 랜덤일 때 인덱스 저장

            if (sfx == Sfx.Hit || sfx == Sfx.Melee) // 타격류 사운드인 경우
            {
                ranIndex = Random.Range(0, 2);      // 인덱스 랜덤 지정 (다양한 히트음 제공)
            }

            channelIndex = loopIndex;               // 현재 채널 기록
            sfxPlayesr[loopIndex].clip = sfxClips[(int)sfx]; // 효과음 클립 설정
            sfxPlayesr[loopIndex].Play();           // 재생
            break;                                  // 한 번만 재생하고 반복 종료
        }
    }
}