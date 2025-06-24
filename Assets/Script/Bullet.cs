using System.Collections;                          // Unity의 기본 컬렉션 기능
using System.Collections.Generic;                  // List, Dictionary 같은 컬렉션 클래스 사용
using UnityEngine;                                 // Unity의 핵심 엔진 기능

public class Bullet : MonoBehaviour                // 총알을 담당하는 스크립트 (2D 물리 기반)
{
    public float damage;                           // 이 총알이 가하는 데미지 값
    public int per;                                // 관통 횟수 (남은 적을 얼마나 더 뚫을 수 있는가)

    Rigidbody2D rigid;                             // 2D 물리 이동 처리를 위한 Rigidbody2D 컴포넌트

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();       // Rigidbody2D 컴포넌트를 찾고 변수에 저장
    }

    public void Init(float damage, int per, Vector3 dir) // 총알 초기 세팅 함수
    {
        this.damage = damage;                      // 데미지 저장
        this.per = per;                            // 관통 횟수 저장

        if (per >= 0)                               // 관통 가능 횟수가 0 이상일 경우만 이동시킴
        {
            rigid.velocity = dir * 15f;            // 방향(dir)으로 속도를 곱해 총알을 날림
        }
    }

    void OnTriggerEnter2D(Collider2D collision)     // 다른 오브젝트와 충돌했을 때 실행됨
    {
        if (!collision.CompareTag("Enemy") || per == -100)
            return;                                // 적이 아니거나 특수값(-100)이면 무시

        per--;                                     // 적을 하나 맞췄으니 관통 횟수 줄이기

        if (per < 0)                                // 더 이상 관통할 수 없으면
        {
            rigid.velocity = Vector2.zero;         // 총알 멈추기
            gameObject.SetActive(false);           // 총알 비활성화 (재활용 가능하게끔)
        }
    }

    void OnTriggerExit2D(Collider2D collision)      // 특정 영역 밖으로 나갈 때 실행됨
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;                                // "Area" 태그가 아니거나 특수값이면 무시

        gameObject.SetActive(false);               // 영역 밖으로 나갔으니 비활성화 (자동 제거용)
    }
}