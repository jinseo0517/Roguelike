using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 업적 해금 및 알림 처리를 담당하는 매니저 클래스
public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockChracter;   // 잠긴 상태일 때 표시되는 캐릭터 UI 오브젝트
    public GameObject[] unlockChracter; // 해금 상태일 때 표시되는 캐릭터 UI 오브젝트
    public GameObject uiNotice;         // 캐릭터 해금 시 출력되는 알림 UI

    enum Achive { UnlockJaeyong, UnlockSeongeun } // 캐릭터별 업적 항목 열거형
    Achive[] achives;                             // enum 배열로 변환해 내부 사용
    WaitForSecondsRealtime wait;                  // 알림을 몇 초간 표시할지 저장

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive)); // enum의 모든 항목을 배열로 추출
        wait = new WaitForSecondsRealtime(5f);              // 알림 대기 시간 설정 (5초)
    }

    void Start()
    {
        UnlockCharacter(); // 저장된 해금 정보를 바탕으로 UI 상태 반영
    }

    // JSON에 저장된 해금 정보를 기반으로 UI 캐릭터 상태 업데이트
    void UnlockCharacter()
    {
        GameSaveData data = SaveSystem.Load(); // 저장된 게임 데이터 로드

        for (int i = 0; i < lockChracter.Length; i++)
        {
            string key = achives[i].ToString(); // 현재 업적 이름 추출
            bool isUnlock = data.unlockedCharacters.Contains(key); // 해금 여부 확인

            lockChracter[i].SetActive(!isUnlock);   // 잠긴 상태 UI는 해금 안 된 경우만 표시
            unlockChracter[i].SetActive(isUnlock);  // 해금 상태 UI는 해금된 경우 표시
        }
    }

    // 매 프레임마다 특정 업적(제한시간 생존)을 체크
    void LateUpdate()
    {
        foreach (Achive achive in achives)
        {
            if (achive == Achive.UnlockSeongeun)
                CheckAchive(achive);
        }
    }

    // 업적 조건을 검사하고 조건 달성 시 알림 및 저장 처리
    void CheckAchive(Achive achive)
    {
        bool isAchive = false;
        GameSaveData data = SaveSystem.Load();      // 저장된 업적 데이터 불러오기
        string key = achive.ToString();             // 업적 이름 추출

        switch (achive)
        {
            case Achive.UnlockJaeyong:
                isAchive = GameManager.instance.kill >= 50;
                break;
            case Achive.UnlockSeongeun:
                isAchive = GameManager.instance.gameTime >= GameManager.instance.maxGameTime;
                break;
        }

        if (isAchive && !data.unlockedCharacters.Contains(key))
        {
            data.unlockedCharacters.Add(key);       // 해금 처리
            SaveSystem.Save(data);                  // 저장

            // 알림 UI 중 해당 캐릭터에 맞는 오브젝트만 표시
            for (int i = 0; i < uiNotice.transform.childCount; i++)
                uiNotice.transform.GetChild(i).gameObject.SetActive(i == (int)achive);

            StartCoroutine(NoticeRoutine());        // 알림 코루틴 실행
        }
    }

    // 적 처치 수 기반 해금 조건을 실시간으로 체크하는 함수
    public void CheckKillAchiveImmediate(int currentKill)
    {
        string key = "UnlockJaeyong"; // 해당 업적 이름
        GameSaveData data = SaveSystem.Load();

        if (currentKill >= 50 && !data.unlockedCharacters.Contains(key))
        {
            data.unlockedCharacters.Add(key);       // 업적 달성 처리
            SaveSystem.Save(data);                  // JSON 저장

            // Jaeyong은 enum의 0번째 항목이므로 인덱스 0 표시
            for (int i = 0; i < uiNotice.transform.childCount; i++)
                uiNotice.transform.GetChild(i).gameObject.SetActive(i == 0);

            uiNotice.SetActive(true);               // 알림 UI 켜기
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Levelup); // 효과음 재생
            StartCoroutine(NoticeRoutine());        // 알림 종료 코루틴 실행
        }
    }

    // 일정 시간동안 알림을 표시하고 종료하는 코루틴
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);     // 알림 UI 켜기
        yield return wait;           // 대기 (5초)
        uiNotice.SetActive(false);   // 알림 UI 끄기
    }
}
