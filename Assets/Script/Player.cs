using System.Collections;                          // Unity�� �⺻ �÷��� ���
using System.Collections.Generic;                  // List, Dictionary ���� ����ϱ� ���� Ȯ�� �÷���
using UnityEngine;                                 // Unity ������ �ٽ� ���
using UnityEngine.InputSystem;                     // ���ο� �Է� �ý���(Input System)�� ����ϱ� ���� ���ӽ����̽�

public class Player : MonoBehaviour                // Player Ŭ����: �÷��̾� ĳ���͸� �����ϴ� ��ũ��Ʈ
{
    public Vector2 inputVec;                       // �÷��̾� �Է� ���� �� (X, Y)
    public float speed;                            // �̵� �ӵ�
    public Scanner scanner;                        // �ֺ� Ž�� ������Ʈ (�� Ž�� ��)
    public Hand[] hands;                           // ���⸦ ������ �� ������Ʈ �迭
    public RuntimeAnimatorController[] animCon;    // ĳ���� �� �ִϸ��̼� ��Ʈ�ѷ� ���

    Rigidbody2D rigid;                             // 2D ���� �̵��� ���� ������ٵ�
    SpriteRenderer spriter;                        // ĳ���� ��������Ʈ ������ �����
    Animator anim;                                 // �ִϸ��̼� ��Ʈ�ѷ�

    void Awake()                                   // ���� ���� �� �� �� ����Ǵ� �ʱ�ȭ �Լ�
    {
        rigid = GetComponent<Rigidbody2D>();       // Rigidbody2D ������Ʈ ã��
        spriter = GetComponent<SpriteRenderer>();  // ��������Ʈ ������ ������Ʈ ã��
        anim = GetComponent<Animator>();           // �ִϸ����� ������Ʈ ã��
        scanner = GetComponent<Scanner>();         // Ž����(Scanner) ������Ʈ ã��
        hands = GetComponentsInChildren<Hand>(true); // �ڽ� ������Ʈ���� Hand ������Ʈ�� ��� ã�� (��Ȱ�� ����)
    }

    void OnEnable()                                // ������Ʈ�� Ȱ��ȭ�� �� ����
    {
        speed *= Character.Speed;                  // ĳ���� �ɷ�ġ(�ӵ� ����ġ) �ݿ�
        anim.runtimeAnimatorController =           // ���õ� ĳ������ �ִϸ��̼� ��Ʈ�ѷ� ����
            animCon[GameManager.instance.playerId];
    }

    void FixedUpdate()                             // ���� ���� ���� ������Ʈ (������ ���� �����ϰ� �����ϰ� ȣ���)
    {
        if (!GameManager.instance.isLive)          // ������ �ߴܵǾ����� �ƹ��͵� ���� ����
            return;

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime; // �̵� ���⿡ �ӵ� �� �ð� ���� ���ϱ�
        rigid.MovePosition(rigid.position + nextVec);              // ���ο� ��ġ�� �̵�
    }

    void OnMove(InputValue value)                  // �Է� �ý������κ��� �̵� �Է��� ���� �� ȣ���
    {
        inputVec = value.Get<Vector2>();           // �Էµ� ���� ���� ���� (���̽�ƽ�̳� Ű���� ����)
    }

    void LateUpdate()                              // ��� Update�� ���� �� ���� (�ַ� �ð� ȿ�� ó��)
    {
        if (!GameManager.instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude); // �ִϸ��̼ǿ� �̵� �ӵ� ���� (0�̸� Idle ����)

        if (inputVec.x != 0)                        // �¿�� ������ ��� ������ �ݿ��Ͽ� ��������Ʈ ������
        {
            spriter.flipX = inputVec.x < 0;         // �������� �̵� ���̸� ��������Ʈ�� �¿� ����
        }
    }

    void OnCollisionStay2D(Collision2D collision)  // 2D �浹�� ��ӵǴ� ���� �� ������ ȣ���
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10; // �浹 ���̸� �ʴ� 10��ŭ ü�� ����

        if (GameManager.instance.health < 0)        // ü���� 0 ���ϰ� �Ǹ�
        {
            for (int index = 2; index < transform.childCount; index++) // ���̳� ���� �� �ڽ� ������Ʈ�� ����
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");               // �״� �ִϸ��̼� ����
            GameManager.instance.GameOver();       // ���� �Ŵ����� GameOver ó�� ��û
        }
    }
}