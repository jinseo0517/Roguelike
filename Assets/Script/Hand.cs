using System.Collections;                          // Unity�� �⺻ �÷��� ���
using System.Collections.Generic;                  // List, Dictionary �� ���׸� �÷���
using UnityEngine;                                 // Unity ���� ��� ���

public class Hand : MonoBehaviour                  // ĳ���� ��(���� ���� ��ġ)�� �����ϴ� Ŭ����
{
    public bool isLeft;                            // �޼� ���� (��������� true)
    public SpriteRenderer spriter;                 // �տ� ������ ������ ��������Ʈ ������

    SpriteRenderer player;                         // �÷��̾� ��ü�� ��������Ʈ ������

    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);         // ������ �⺻ ��ġ
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0); // ������ ������ ��ġ
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);         // �޼� ȸ�� ����
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135); // ������ �޼� ȸ�� ����

    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];  // �θ� �� �� ��° SpriteRenderer �������� (�÷��̾�)
    }

    void LateUpdate()                                         // �ð� ��� �ݿ��� ���� LateUpdate���� ����
    {
        bool isReverse = player.flipX;                        // �÷��̾ �¿� ������ �������� Ȯ��

        if (isLeft)                                           // ���� ������ ��� (�޼�)
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot; // ���� ���ο� ���� ȸ�� ���� ����
            spriter.flipY = isReverse;                                   // ���Ʒ� ���� ����
            spriter.sortingOrder = isReverse ? 4 : 6;                    // ������ ���� ���� (��/�� ��ġ)
        }
        else                                                  // ���Ÿ� ������ ��� (������)
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos; // ��ġ ���� (���� ���ο� ����)
            spriter.flipX = isReverse;                                  // �¿� ����
            spriter.sortingOrder = isReverse ? 6 : 4;                   // ������ ���� �ݴ�� ����
        }
    }
}