using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤 인스턴스 (다른 스크립트에서 접근용)

    [Header("# Game Control")]
    public bool isLive;                // 게임 진행 여부
    public float gameTime;            // 누적 게임 시간
    public float maxGameTime = 20f;   // 제한 시간 (20초)

    [Header("# Player Info")]
    public int playerId;              // 선택된 캐릭터 ID
    public float health;              // 현재 체력
    public float maxHealth = 100;     // 최대 체력
    public int level;                 // 현재 레벨
    public int kill;                  // 적 처치 수
    public int exp;                   // 현재 경험치
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 }; // 각 레벨별 요구 경험치

    [Header("# Game Object")]
    public PoolManager pool;          // 오브젝트 풀링 매니저
    public Player player;             // 플레이어 객체
    public LevelUp uiLevelUp;         // 레벨업 UI
    public Result uiResult;           // 결과 UI
    public GameObject enemyCleaner;  // 남은 적 제거용 오브젝트 (승리 시 사용)

    private void Awake()
    {
        instance = this; // 싱글톤 할당
    }

    void Start()
    {
        Debug.Log("Save Path: " + Application.persistentDataPath); // 저장 경로 출력

        
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;
        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2); // 캐릭터 종류에 따른 UI 선택
        Resume();

        AudioManager.instance.PlayBgm(true);           // 배경음 재생
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select); // 선택 효과음
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine()); // 게임 오버 처리 시작
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;                    // 게임 정지
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();                   // 패배 UI 출력
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());

        // 누적 처치 수 저장 (JSON 저장)
        GameSaveData data = SaveSystem.Load();
        data.totalKills += kill;
        SaveSystem.Save(data);
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);     // 적 제거기 발동

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();                   // 승리 UI 출력
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);        // 씬 다시 로드 (게임 재시작)
    }

    public void GameQuit()
    {
        Application.Quit();               // 게임 종료
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();               // 제한 시간 도달 시 자동 승리 처리
        }
    }

    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;

        int currentLevel = Mathf.Min(level, nextExp.Length - 1);

        if (exp == nextExp[currentLevel])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();            // 레벨업 UI 출력
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;              // 게임 시간 정지
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;              // 게임 시간 재개
    }

    // 적 처치 시 호출되는 함수
    public void AddKill()
    {
        kill++;
        FindObjectOfType<AchiveManager>().CheckKillAchiveImmediate(kill); // 실시간 업적 체크
    }
}