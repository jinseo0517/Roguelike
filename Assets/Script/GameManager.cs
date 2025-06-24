using System;                                      // 기본 .NET 시스템 기능 사용
using System.Collections;                          // Unity에서 코루틴 등 컬렉션 기능 사용
using System.Collections.Generic;                  // List, Dictionary 등 제네릭 컬렉션 사용
using UnityEngine;                                 // Unity의 핵심 엔진 기능 사용
using UnityEngine.SceneManagement;                 // 씬을 불러오고 관리하는 기능 사용

public class GameManager : MonoBehaviour           // 게임 전체 상태를 관리하는 매니저 클래스
{
    public static GameManager instance;            // 싱글톤: 다른 스크립트에서 쉽게 접근 가능하도록 함

    [Header("# Game Control")]
    public bool isLive;                            // 게임이 진행 중인지 여부
    public float gameTime;                         // 현재 게임 진행 시간
    public float maxGameTime = 2 * 10f;            // 최대 게임 시간 (여기선 20초)

    [Header("# Player Info")]
    public int playerId;                           // 선택된 캐릭터 ID
    public float health;                           // 현재 체력
    public float maxHealth = 100;                  // 최대 체력
    public int level;                              // 현재 플레이어 레벨
    public int kill;                               // 적 처치 수
    public int exp;                                // 현재 경험치
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 }; // 각 레벨마다 요구되는 경험치

    [Header("# Game Object")]
    public PoolManager pool;                       // 오브젝트 풀 매니저
    public Player player;                          // 플레이어 오브젝트
    public LevelUp uiLevelUp;                      // 레벨업 UI
    public Result uiResult;                        // 결과화면 UI
    public GameObject enemyCleaner;                // 남은 적을 제거하는 오브젝트 (승리 시 사용)

    private void Awake()                           // 게임 시작 전 초기화. 싱글톤 instance 설정
    {
        instance = this;
    }

    public void GameStart(int id)                  // 게임 시작 시 호출되는 함수
    {
        playerId = id;                             // 선택한 캐릭터 ID 저장
        health = maxHealth;                        // 체력 초기화

        player.gameObject.SetActive(true);         // 플레이어 활성화
        uiLevelUp.Select(playerId % 2);            // 캐릭터 종류에 따라 레벨업 UI 설정
        Resume();                                  // 게임 시작

        AudioManager.instance.PlayBgm(true);       // 배경음악 재생
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select); // 캐릭터 선택 효과음
    }

    public void GameOver()                         // 게임 오버 처리 시작
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()                  // 게임 오버 연출 코루틴
    {
        isLive = false;                            // 게임 정지

        yield return new WaitForSeconds(0.5f);     // 잠깐 대기

        uiResult.gameObject.SetActive(true);       // 결과 UI 활성화
        uiResult.Lose();                           // 패배 UI 출력
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose); // 패배 효과음
    }

    public void GameVictory()                      // 승리 처리 시작
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()               // 게임 승리 연출 코루틴
    {
        isLive = false;

        enemyCleaner.SetActive(true);              // 화면상 모든 적 제거 오브젝트 활성화

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);       // 결과 UI 활성화
        uiResult.Win();                            // 승리 UI 출력
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win); // 승리 효과음
    }

    public void GameRetry()                        // 게임 재시작
    {
        SceneManager.LoadScene(0);                 // 씬을 다시 불러옴 (초기화)
    }

    public void GameQuit()                         // 게임 종료
    {
        Application.Quit();                        // 앱 종료
    }

    void Update()                                  // 매 프레임 실행되는 함수
    {
        if (!isLive)                               // 게임 중이 아니면 진행 안 함
            return;

        gameTime += Time.deltaTime;                // 경과 시간 증가

        if (gameTime > maxGameTime)                // 최대 시간 초과 시
        {
            gameTime = maxGameTime;
            GameVictory();                         // 자동 승리 처리
        }
    }

    public void GetExp()                           // 경험치 획득 함수
    {
        if (!isLive)                               // 게임 중이 아니면 무시
            return;

        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)]) // 경험치 최대치 도달 시
        {
            level++;                               // 레벨업
            exp = 0;                               // 경험치 초기화
            uiLevelUp.Show();                      // 레벨업 UI 표시
        }
    }

    public void Stop()                             // 게임 정지 처리
    {
        isLive = false;
        Time.timeScale = 0;                        // 시간 흐름 멈춤
    }

    public void Resume()                           // 게임 다시 시작
    {
        isLive = true;
        Time.timeScale = 1;                        // 시간 흐름 정상화
    }
}