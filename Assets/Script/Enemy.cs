using System.Collections;                          // Unity �⺻ �÷��� ���
using System.Collections.Generic;                  // ���׸� �÷��� ���
using UnityEngine;                                 // Unity�� ���� ��� ���

public class Enemy : MonoBehaviour                 // ���� ������, ü��, ��� ó�� ���� ����ϴ� Ŭ����
{
    public float speed;                            // �̵� �ӵ�
    public float health;                           // ���� ü��
    public float maxHealth;                        // �ִ� ü��
    public RuntimeAnimatorController[] animCon;    // �� �ִϸ��̼� ���� ���
    public Rigidbody2D target;                     // ������ ��� (�÷��̾��� Rigidbody2D)

    bool isLive;                                   // ���� ����ִ��� ����

    Rigidbody2D rigid;                             // ���� ���� �̵��� ����ϴ� ������Ʈ
    Collider2D coll;                               // �浹 ó���� ���� �ݶ��̴�
    Animator anim;                                 // �ִϸ��̼� ����� ������Ʈ
    SpriteRenderer spriter;                        // ��������Ʈ ���� �� �ð� �����
    WaitForFixedUpdate wait;                       // �ڷ�ƾ���� ���� ������ ����� �� ���

    void Awake()                                   // ���� ���� �� �� �� ����Ǵ� �ʱ�ȭ �Լ�
    {
        rigid = GetComponent<Rigidbody2D>();       // Rigidbody2D ������Ʈ ����
        coll = GetComponent<Collider2D>();         // Collider2D ����
        anim = GetComponent<Animator>();           // Animator ����
        spriter = GetComponent<SpriteRenderer>();  // SpriteRenderer ����
        wait = new WaitForFixedUpdate();           // �� ������ ���� �ð� ��� ��ü ����
    }

    void FixedUpdate()                             // ������ �������� ȣ�� (���� ����)
    {
        if (!GameManager.instance.isLive)          // ������ ���� �����̸� �������� ����
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;                                // �׾��ų� �´� ���̸� �̵� ����

        Vector2 dirVec = target.position - rigid.position;          // �÷��̾� ���� ����
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // ����� �ӵ��� �̵� ���� ���
        rigid.MovePosition(rigid.position + nextVec);               // ���� ��ġ�� �̵�
        rigid.velocity = Vector2.zero;                              // �ܿ� �̵��ӵ� ����
    }

    void LateUpdate()                              // ȭ�� �׸��� ���� ����Ǵ� �Լ�
    {
        if (!GameManager.instance.isLive || !isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;       // �÷��̾ ���ʿ� ������ ��������Ʈ �¿� ����
    }

    void OnEnable()                                // ������Ʈ�� Ȱ��ȭ�� �� �����
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); // ���� ����� �÷��̾�
        isLive = true;                             // �� Ȱ��ȭ
        coll.enabled = true;                       // �浹 Ȱ��ȭ
        rigid.simulated = true;                    // ���� ���� Ȱ��ȭ
        spriter.sortingOrder = 2;                  // ��������Ʈ ǥ�� ���� ����
        anim.SetBool("Dead", false);               // ���� �ִϸ��̼� ��Ȱ��ȭ
        health = maxHealth;                        // ü�� �ʱ�ȭ
    }

    public void Init(SpawnData data)               // �� ���� �� ����/�Ӽ� �ʱ� ����
    {
        anim.runtimeAnimatorController = animCon[data.spriteType]; // ���� ����
        speed = data.speed;                        // �̵� �ӵ� ����
        maxHealth = data.health;                   // �ִ� ü�� ����
        health = data.health;                      // ���� ü�� ����
    }

    void OnTriggerEnter2D(Collider2D collision)    // �浹 �� ����Ǵ� �Լ�
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage; // �Ѿ��� ��������ŭ ü�� ����
        StartCoroutine(KnockBack());                       // �˹� ȿ�� ����

        if (health > 0)                                    // ��� �ִٸ�
        {
            anim.SetTrigger("Hit");                        // �´� �ִϸ��̼� ���
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit); // Ÿ�� ���� ���
        }
        else                                               // ü���� 0 ������ ���
        {
            isLive = false;
            coll.enabled = false;                          // �浹 ��Ȱ��ȭ
            rigid.simulated = false;                       // ���� ���� ����
            spriter.sortingOrder = 1;                      // ��������Ʈ �켱���� ����
            anim.SetBool("Dead", true);                    // ���� �ִϸ��̼� ����
            GameManager.instance.AddKill();                   // ���ӸŴ����� óġ �� ����
            GameManager.instance.GetExp();                 // ����ġ ȹ��

            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead); // �� ��� ���� ���
        }
    }

    IEnumerator KnockBack()                        // �÷��̾�κ��� �з����� ����
    {
        yield return wait;                         // �� ���� ������ ���
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;  // �÷��̾� �ݴ���� ���
        rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse); // ���� �־� ƨ�ܳ���
    }

    void Dead()                                    // �ִϸ��̼� �̺�Ʈ���� ȣ�� (��� ó��)
    {
        gameObject.SetActive(false);               // ������Ʈ�� ��Ȱ��ȭ (Ǯ�� �ý��� ��Ȱ��)
    }
}