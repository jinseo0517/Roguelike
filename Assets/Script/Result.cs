using System.Collections;                          // Unity�� �⺻ �÷��� ��� ���
using System.Collections.Generic;                  // List, Dictionary �� ���׸� �÷��� ���
using UnityEngine;                                 // Unity ������ �ٽ� ��� ���

public class Result : MonoBehaviour                // ���� ���(�¸� �Ǵ� �й�) ȭ���� �����ϴ� Ŭ����
{
    public GameObject[] titles;                    // 0��: �й� ȭ��, 1��: �¸� ȭ�� ���� ���� ���� ������Ʈ �迭

    public void Lose()                             // ���� ���� �� ȣ��Ǵ� �Լ�
    {
        titles[0].SetActive(true);                 // �й� Ÿ��Ʋ(ù ��° ������Ʈ)�� ȭ�鿡 ǥ��
    }

    public void Win()                              // ���� �¸� �� ȣ��Ǵ� �Լ�
    {
        titles[1].SetActive(true);                 // �¸� Ÿ��Ʋ(�� ��° ������Ʈ)�� ȭ�鿡 ǥ��
    }
}