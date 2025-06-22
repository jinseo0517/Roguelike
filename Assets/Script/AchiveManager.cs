using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockChracter;   //��� ĳ���͸� ǥ���ϴ� ������Ʈ �迭
    public GameObject[] unlockChracter; //��� ������ ĳ���͸� ǥ���ϴ� ������Ʈ �迭
    public GameObject uiNotice;         //�˸�ui ������Ʈ

    enum Achive {  UnlockJaeyong, UnlockSeongeun }  //���� ���� ����
    Achive[] achives;               //������ �������� �迭�� ����
    WaitForSecondsRealtime wait;    //���� �ð� ��ٸ��� ���� ����(�˸���)

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive)); //������ ���� �迭�� ��ȯ
        wait = new WaitForSecondsRealtime(5);               //5�ʵ��� ��ٸ��� ��ü ����

        if (!PlayerPrefs.HasKey("MyData"))  //����� �����Ͱ� ������
        {
            Init();                         //�ʱⰪ ����
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);                //����� ������ ���� ǥ��

        foreach (Achive achive in achives)              //��� ������ ����
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);   //���� �ʱⰪ�� 0���� ����(��� ����)
        }
    }

    void Start()
    {
        UnlockCharacter();  //���۽� ��� ���� Ȯ���ؼ� ĳ���� ���� ����
    }

    void UnlockCharacter()
    {
        for (int index = 0; index < lockChracter.Length; index++)   //�� ĳ���͸���
        {
            string achiveName = achives[index].ToString();          //���� �̸� ��������
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;    //�ش� ������ �رݵǾ����� Ȯ��
            lockChracter[index].SetActive(!isUnlock);               //��� ĳ���ʹ� ���� �̿Ϸ��϶� Ȱ��ȭ
            unlockChracter[index].SetActive(isUnlock);              //�رݵ� ĳ���ʹ� ���� �Ϸ��ϋ� Ȱ��ȭ
        }
    }

    void LateUpdate()
    {
        foreach (Achive achive in achives)  //�� �����Ӹ��� ���� ���� üũ
        {
            CheckAchive(achive);            //�ش� ���� ���� ���� Ȯ��
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;      //���� �޼� ����

        switch (achive)
        {
            case Achive.UnlockJaeyong:
                isAchive = GameManager.instance.kill >= 10;     //�� 10���� óġ�� �����޼�
                break;
            case Achive.UnlockSeongeun:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;   //���ѽð����� ������ �����޼�
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0) //������ ó�� �޼����� ���
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);               //������ �Ϸ�� ����

            for (int index=0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;               //���� ������ �ش��ϴ� UI�� Ȱ��ȭ
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());    //�˸� UIǥ�� �ڷ�ƾ ����
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);   //�˸� UI���̱�
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Levelup);    //���� ������ ȿ����

        yield return wait;

        uiNotice.SetActive(false);  //�˸� UI�����
    }
}
