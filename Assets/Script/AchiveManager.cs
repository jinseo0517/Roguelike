using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// ���� �ر� �� �˸� ó���� ����ϴ� �Ŵ��� Ŭ����
public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockChracter;   // ��� ������ �� ǥ�õǴ� ĳ���� UI ������Ʈ
    public GameObject[] unlockChracter; // �ر� ������ �� ǥ�õǴ� ĳ���� UI ������Ʈ
    public GameObject uiNotice;         // ĳ���� �ر� �� ��µǴ� �˸� UI

    enum Achive { UnlockJaeyong, UnlockSeongeun } // ĳ���ͺ� ���� �׸� ������
    Achive[] achives;                             // enum �迭�� ��ȯ�� ���� ���
    WaitForSecondsRealtime wait;                  // �˸��� �� �ʰ� ǥ������ ����

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive)); // enum�� ��� �׸��� �迭�� ����
        wait = new WaitForSecondsRealtime(5f);              // �˸� ��� �ð� ���� (5��)
    }

    void Start()
    {
        UnlockCharacter(); // ����� �ر� ������ �������� UI ���� �ݿ�
    }

    // JSON�� ����� �ر� ������ ������� UI ĳ���� ���� ������Ʈ
    void UnlockCharacter()
    {
        GameSaveData data = SaveSystem.Load(); // ����� ���� ������ �ε�

        for (int i = 0; i < lockChracter.Length; i++)
        {
            string key = achives[i].ToString(); // ���� ���� �̸� ����
            bool isUnlock = data.unlockedCharacters.Contains(key); // �ر� ���� Ȯ��

            lockChracter[i].SetActive(!isUnlock);   // ��� ���� UI�� �ر� �� �� ��츸 ǥ��
            unlockChracter[i].SetActive(isUnlock);  // �ر� ���� UI�� �رݵ� ��� ǥ��
        }
    }

    // �� �����Ӹ��� Ư�� ����(���ѽð� ����)�� üũ
    void LateUpdate()
    {
        foreach (Achive achive in achives)
        {
            if (achive == Achive.UnlockSeongeun)
                CheckAchive(achive);
        }
    }

    // ���� ������ �˻��ϰ� ���� �޼� �� �˸� �� ���� ó��
    void CheckAchive(Achive achive)
    {
        bool isAchive = false;
        GameSaveData data = SaveSystem.Load();      // ����� ���� ������ �ҷ�����
        string key = achive.ToString();             // ���� �̸� ����

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
            data.unlockedCharacters.Add(key);       // �ر� ó��
            SaveSystem.Save(data);                  // ����

            // �˸� UI �� �ش� ĳ���Ϳ� �´� ������Ʈ�� ǥ��
            for (int i = 0; i < uiNotice.transform.childCount; i++)
                uiNotice.transform.GetChild(i).gameObject.SetActive(i == (int)achive);

            StartCoroutine(NoticeRoutine());        // �˸� �ڷ�ƾ ����
        }
    }

    // �� óġ �� ��� �ر� ������ �ǽð����� üũ�ϴ� �Լ�
    public void CheckKillAchiveImmediate(int currentKill)
    {
        string key = "UnlockJaeyong"; // �ش� ���� �̸�
        GameSaveData data = SaveSystem.Load();

        if (currentKill >= 50 && !data.unlockedCharacters.Contains(key))
        {
            data.unlockedCharacters.Add(key);       // ���� �޼� ó��
            SaveSystem.Save(data);                  // JSON ����

            // Jaeyong�� enum�� 0��° �׸��̹Ƿ� �ε��� 0 ǥ��
            for (int i = 0; i < uiNotice.transform.childCount; i++)
                uiNotice.transform.GetChild(i).gameObject.SetActive(i == 0);

            uiNotice.SetActive(true);               // �˸� UI �ѱ�
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Levelup); // ȿ���� ���
            StartCoroutine(NoticeRoutine());        // �˸� ���� �ڷ�ƾ ����
        }
    }

    // ���� �ð����� �˸��� ǥ���ϰ� �����ϴ� �ڷ�ƾ
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);     // �˸� UI �ѱ�
        yield return wait;           // ��� (5��)
        uiNotice.SetActive(false);   // �˸� UI ����
    }
}
