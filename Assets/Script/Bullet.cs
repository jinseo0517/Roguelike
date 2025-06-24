using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;                 // 이 총알이 가하는 데미지
    public int per;                      // 관통 가능한 횟수

    Rigidbody2D rigid;                   // 자기 자신의 Rigidbody2D

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); // 초기화
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per >= 0)
        {
            rigid.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100)
            return;

        // 안전하게 Bullet.cs → Enemy.cs 로직에만 damage 전달
        var enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            per--;

            if (per < 0)
            {
                rigid.velocity = Vector2.zero;
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);
    }
}