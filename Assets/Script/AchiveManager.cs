using System;
using System.Collections;                          // Unity �⺻ �÷��� ���
using System.Collections.Generic;                  // List, Dictionary ���� ���׸� �÷��� ���
using UnityEngine;                                 // Unity ���� ��� ���

public class AchiveManager : MonoBehaviour         // ���� �ý����� �����ϴ� Ŭ����
{
    public GameObject[] lockChracter;              // ��� ĳ���� UI ������Ʈ��
    public GameObject[] unlockChracter;            // �رݵ� ĳ���� UI ������Ʈ��
    public GameObject uiNotice;                    // ���� �޼� �� ǥ���� �˸� UI

    enum Achive { UnlockJaeyong, UnlockSeongeun }  // ���� �̸� ����
    Achive[] achives;                              // �������� �迭�� ���� (��ü �ݺ���)
    WaitForSecondsRealtime wait;                   // �ڷ�ƾ���� ����� ��� �ð� (5��)

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive)); // ������ ��ü ���� �迭�� ��ȯ
        wait = new WaitForSecondsRealtime(5);               // 5�� ��� ��ü ����

        if (!PlayerPrefs.HasKey("MyData"))                  // ó�� �����ϴ� ���
        {
            Init();                                         // ���� ������ �ʱ�ȭ
        }
    }

    void Init()                                             // ���� ���� �ʱ�ȭ �Լ�
    {
        PlayerPrefs.SetInt("MyData", 1);                    // ���� ������ ������ ǥ��

        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);       // ��� ������ ��� ����(0)�� ����
        }
    }

    void Start()
    {
        UnlockCharacter();                                  // ���� �� ���� �޼� ���� �ݿ��Ͽ� ĳ���� ��� ���� ���� ����
    }

    void UnlockCharacter()
    {
        for (int index = 0; index < lockChracter.Length; index++)
        {
            string achiveName = achives[index].ToString();                  // ���� �̸� �������� (���ڿ�)
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;           // �ر� ���� Ȯ��

            lockChracter[index].SetActive(!isUnlock);                      // ��� ���� ĳ���ʹ� ���� �̿Ϸ� �ø� Ȱ��ȭ
            unlockChracter[index].SetActive(isUnlock);                     // �ر� ĳ���ʹ� ���� �Ϸ� �ø� Ȱ��ȭ
        }
    }

    void LateUpdate()                                                     // �� ������ �Ĺݿ� ����
    {
        foreach (Achive achive in achives)                                // ��� ������ ����
        {
            CheckAchive(achive);                                          // �ش� ������ ������ üũ
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;                                            // ���� ������ �����ߴ��� ����

        switch (achive)                                                   // ���� ������ ���� ����
        {
            case Achive.UnlockJaeyong:
                isAchive = GameManager.instance.kill >= 10;               // �� 10���� �̻� óġ
                break;
            case Achive.UnlockSeongeun:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime; // ���ѽð����� ����
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)       // ó������ ������ ������ ���
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);                     // ���� �ر� ���� ����

            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;                     // �ش� ������ �ش��ϴ� �˸� UI�� �ѱ�
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());                              // �˸� UI �����ִ� �ڷ�ƾ ����
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);                                         // �˸� UI ���̱�
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Levelup);          // ���� �޼� ȿ���� ���

        yield return wait;                                                // 5�� ���

        uiNotice.SetActive(false);                                        // �˸� UI �����
    }
}