using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱��� �ν��Ͻ� (�ٸ� ��ũ��Ʈ���� ���ٿ�)

    [Header("# Game Control")]
    public bool isLive;                // ���� ���� ����
    public float gameTime;            // ���� ���� �ð�
    public float maxGameTime = 20f;   // ���� �ð� (20��)

    [Header("# Player Info")]
    public int playerId;              // ���õ� ĳ���� ID
    public float health;              // ���� ü��
    public float maxHealth = 100;     // �ִ� ü��
    public int level;                 // ���� ����
    public int kill;                  // �� óġ ��
    public int exp;                   // ���� ����ġ
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 }; // �� ������ �䱸 ����ġ

    [Header("# Game Object")]
    public PoolManager pool;          // ������Ʈ Ǯ�� �Ŵ���
    public Player player;             // �÷��̾� ��ü
    public LevelUp uiLevelUp;         // ������ UI
    public Result uiResult;           // ��� UI
    public GameObject enemyCleaner;  // ���� �� ���ſ� ������Ʈ (�¸� �� ���)

    private void Awake()
    {
        instance = this; // �̱��� �Ҵ�
    }

    void Start()
    {
        Debug.Log("Save Path: " + Application.persistentDataPath); // ���� ��� ���

        
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;
        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2); // ĳ���� ������ ���� UI ����
        Resume();

        AudioManager.instance.PlayBgm(true);           // ����� ���
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select); // ���� ȿ����
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine()); // ���� ���� ó�� ����
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;                    // ���� ����
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();                   // �й� UI ���
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());

        // ���� óġ �� ���� (JSON ����)
        GameSaveData data = SaveSystem.Load();
        data.totalKills += kill;
        SaveSystem.Save(data);
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);     // �� ���ű� �ߵ�

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();                   // �¸� UI ���
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);        // �� �ٽ� �ε� (���� �����)
    }

    public void GameQuit()
    {
        Application.Quit();               // ���� ����
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();               // ���� �ð� ���� �� �ڵ� �¸� ó��
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
            uiLevelUp.Show();            // ������ UI ���
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;              // ���� �ð� ����
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;              // ���� �ð� �簳
    }

    // �� óġ �� ȣ��Ǵ� �Լ�
    public void AddKill()
    {
        kill++;
        FindObjectOfType<AchiveManager>().CheckKillAchiveImmediate(kill); // �ǽð� ���� üũ
    }
}