using System.Collections;                          // Unity�� �⺻ �÷��� ���
using System.Collections.Generic;                  // List, Dictionary ���� �÷��� Ŭ���� ���
using UnityEngine;                                 // Unity�� �ٽ� ���� ���

public class Bullet : MonoBehaviour                // �Ѿ��� ����ϴ� ��ũ��Ʈ (2D ���� ���)
{
    public float damage;                           // �� �Ѿ��� ���ϴ� ������ ��
    public int per;                                // ���� Ƚ�� (���� ���� �󸶳� �� ���� �� �ִ°�)

    Rigidbody2D rigid;                             // 2D ���� �̵� ó���� ���� Rigidbody2D ������Ʈ

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();       // Rigidbody2D ������Ʈ�� ã�� ������ ����
    }

    public void Init(float damage, int per, Vector3 dir) // �Ѿ� �ʱ� ���� �Լ�
    {
        this.damage = damage;                      // ������ ����
        this.per = per;                            // ���� Ƚ�� ����

        if (per >= 0)                               // ���� ���� Ƚ���� 0 �̻��� ��츸 �̵���Ŵ
        {
            rigid.velocity = dir * 15f;            // ����(dir)���� �ӵ��� ���� �Ѿ��� ����
        }
    }

    void OnTriggerEnter2D(Collider2D collision)     // �ٸ� ������Ʈ�� �浹���� �� �����
    {
        if (!collision.CompareTag("Enemy") || per == -100)
            return;                                // ���� �ƴϰų� Ư����(-100)�̸� ����

        per--;                                     // ���� �ϳ� �������� ���� Ƚ�� ���̱�

        if (per < 0)                                // �� �̻� ������ �� ������
        {
            rigid.velocity = Vector2.zero;         // �Ѿ� ���߱�
            gameObject.SetActive(false);           // �Ѿ� ��Ȱ��ȭ (��Ȱ�� �����ϰԲ�)
        }
    }

    void OnTriggerExit2D(Collider2D collision)      // Ư�� ���� ������ ���� �� �����
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;                                // "Area" �±װ� �ƴϰų� Ư�����̸� ����

        gameObject.SetActive(false);               // ���� ������ �������� ��Ȱ��ȭ (�ڵ� ���ſ�)
    }
}