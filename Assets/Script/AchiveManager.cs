using System;
using System.Collections;                          // Unity 기본 컬렉션 기능
using System.Collections.Generic;                  // List, Dictionary 같은 제네릭 컬렉션 기능
using UnityEngine;                                 // Unity 엔진 기능 사용

public class AchiveManager : MonoBehaviour         // 업적 시스템을 관리하는 클래스
{
    public GameObject[] lockChracter;              // 잠긴 캐릭터 UI 오브젝트들
    public GameObject[] unlockChracter;            // 해금된 캐릭터 UI 오브젝트들
    public GameObject uiNotice;                    // 업적 달성 시 표시할 알림 UI

    enum Achive { UnlockJaeyong, UnlockSeongeun }  // 업적 이름 정의
    Achive[] achives;                              // 업적들을 배열로 저장 (전체 반복용)
    WaitForSecondsRealtime wait;                   // 코루틴에서 사용할 대기 시간 (5초)

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive)); // 열거형 전체 값을 배열로 변환
        wait = new WaitForSecondsRealtime(5);               // 5초 대기 객체 생성

        if (!PlayerPrefs.HasKey("MyData"))                  // 처음 실행하는 경우
        {
            Init();                                         // 업적 데이터 초기화
        }
    }

    void Init()                                             // 업적 저장 초기화 함수
    {
        PlayerPrefs.SetInt("MyData", 1);                    // 업적 저장이 있음을 표시

        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);       // 모든 업적을 잠긴 상태(0)로 설정
        }
    }

    void Start()
    {
        UnlockCharacter();                                  // 시작 시 업적 달성 여부 반영하여 캐릭터 잠금 해제 상태 설정
    }

    void UnlockCharacter()
    {
        for (int index = 0; index < lockChracter.Length; index++)
        {
            string achiveName = achives[index].ToString();                  // 업적 이름 가져오기 (문자열)
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;           // 해금 여부 확인

            lockChracter[index].SetActive(!isUnlock);                      // 잠금 상태 캐릭터는 업적 미완료 시만 활성화
            unlockChracter[index].SetActive(isUnlock);                     // 해금 캐릭터는 업적 완료 시만 활성화
        }
    }

    void LateUpdate()                                                     // 매 프레임 후반에 실행
    {
        foreach (Achive achive in achives)                                // 모든 업적에 대해
        {
            CheckAchive(achive);                                          // 해당 업적의 조건을 체크
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;                                            // 업적 조건을 충족했는지 여부

        switch (achive)                                                   // 업적 종류별 조건 정의
        {
            case Achive.UnlockJaeyong:
                isAchive = GameManager.instance.kill >= 10;               // 적 10마리 이상 처치
                break;
            case Achive.UnlockSeongeun:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime; // 제한시간까지 생존
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)       // 처음으로 조건을 충족한 경우
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);                     // 업적 해금 상태 저장

            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;                     // 해당 업적에 해당하는 알림 UI만 켜기
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());                              // 알림 UI 보여주는 코루틴 실행
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);                                         // 알림 UI 보이기
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Levelup);          // 업적 달성 효과음 재생

        yield return wait;                                                // 5초 대기

        uiNotice.SetActive(false);                                        // 알림 UI 숨기기
    }
}