using System;                                      // �⺻ .NET �ý��� ��� ���
using System.Collections;                          // Unity���� �ڷ�ƾ �� �÷��� ��� ���
using System.Collections.Generic;                  // List, Dictionary �� ���׸� �÷��� ���
using UnityEngine;                                 // Unity�� �ٽ� ���� ��� ���
using UnityEngine.SceneManagement;                 // ���� �ҷ����� �����ϴ� ��� ���

public class GameManager : MonoBehaviour           // ���� ��ü ���¸� �����ϴ� �Ŵ��� Ŭ����
{
    public static GameManager instance;            // �̱���: �ٸ� ��ũ��Ʈ���� ���� ���� �����ϵ��� ��

    [Header("# Game Control")]
    public bool isLive;                            // ������ ���� ������ ����
    public float gameTime;                         // ���� ���� ���� �ð�
    public float maxGameTime = 2 * 10f;            // �ִ� ���� �ð� (���⼱ 20��)

    [Header("# Player Info")]
    public int playerId;                           // ���õ� ĳ���� ID
    public float health;                           // ���� ü��
    public float maxHealth = 100;                  // �ִ� ü��
    public int level;                              // ���� �÷��̾� ����
    public int kill;                               // �� óġ ��
    public int exp;                                // ���� ����ġ
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 }; // �� �������� �䱸�Ǵ� ����ġ

    [Header("# Game Object")]
    public PoolManager pool;                       // ������Ʈ Ǯ �Ŵ���
    public Player player;                          // �÷��̾� ������Ʈ
    public LevelUp uiLevelUp;                      // ������ UI
    public Result uiResult;                        // ���ȭ�� UI
    public GameObject enemyCleaner;                // ���� ���� �����ϴ� ������Ʈ (�¸� �� ���)

    private void Awake()                           // ���� ���� �� �ʱ�ȭ. �̱��� instance ����
    {
        instance = this;
    }

    public void GameStart(int id)                  // ���� ���� �� ȣ��Ǵ� �Լ�
    {
        playerId = id;                             // ������ ĳ���� ID ����
        health = maxHealth;                        // ü�� �ʱ�ȭ

        player.gameObject.SetActive(true);         // �÷��̾� Ȱ��ȭ
        uiLevelUp.Select(playerId % 2);            // ĳ���� ������ ���� ������ UI ����
        Resume();                                  // ���� ����

        AudioManager.instance.PlayBgm(true);       // ������� ���
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select); // ĳ���� ���� ȿ����
    }

    public void GameOver()                         // ���� ���� ó�� ����
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()                  // ���� ���� ���� �ڷ�ƾ
    {
        isLive = false;                            // ���� ����

        yield return new WaitForSeconds(0.5f);     // ��� ���

        uiResult.gameObject.SetActive(true);       // ��� UI Ȱ��ȭ
        uiResult.Lose();                           // �й� UI ���
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose); // �й� ȿ����
    }

    public void GameVictory()                      // �¸� ó�� ����
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()               // ���� �¸� ���� �ڷ�ƾ
    {
        isLive = false;

        enemyCleaner.SetActive(true);              // ȭ��� ��� �� ���� ������Ʈ Ȱ��ȭ

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);       // ��� UI Ȱ��ȭ
        uiResult.Win();                            // �¸� UI ���
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win); // �¸� ȿ����
    }

    public void GameRetry()                        // ���� �����
    {
        SceneManager.LoadScene(0);                 // ���� �ٽ� �ҷ��� (�ʱ�ȭ)
    }

    public void GameQuit()                         // ���� ����
    {
        Application.Quit();                        // �� ����
    }

    void Update()                                  // �� ������ ����Ǵ� �Լ�
    {
        if (!isLive)                               // ���� ���� �ƴϸ� ���� �� ��
            return;

        gameTime += Time.deltaTime;                // ��� �ð� ����

        if (gameTime > maxGameTime)                // �ִ� �ð� �ʰ� ��
        {
            gameTime = maxGameTime;
            GameVictory();                         // �ڵ� �¸� ó��
        }
    }

    public void GetExp()                           // ����ġ ȹ�� �Լ�
    {
        if (!isLive)                               // ���� ���� �ƴϸ� ����
            return;

        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)]) // ����ġ �ִ�ġ ���� ��
        {
            level++;                               // ������
            exp = 0;                               // ����ġ �ʱ�ȭ
            uiLevelUp.Show();                      // ������ UI ǥ��
        }
    }

    public void Stop()                             // ���� ���� ó��
    {
        isLive = false;
        Time.timeScale = 0;                        // �ð� �帧 ����
    }

    public void Resume()                           // ���� �ٽ� ����
    {
        isLive = true;
        Time.timeScale = 1;                        // �ð� �帧 ����ȭ
    }
}