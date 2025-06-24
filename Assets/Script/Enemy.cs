using System.Collections;                          // Unity 기본 컬렉션 기능
using System.Collections.Generic;                  // 제네릭 컬렉션 사용
using UnityEngine;                                 // Unity의 엔진 기능 사용

public class Enemy : MonoBehaviour                 // 적의 움직임, 체력, 사망 처리 등을 담당하는 클래스
{
    public float speed;                            // 이동 속도
    public float health;                           // 현재 체력
    public float maxHealth;                        // 최대 체력
    public RuntimeAnimatorController[] animCon;    // 적 애니메이션 종류 목록
    public Rigidbody2D target;                     // 추적할 대상 (플레이어의 Rigidbody2D)

    bool isLive;                                   // 현재 살아있는지 여부

    Rigidbody2D rigid;                             // 적의 물리 이동을 담당하는 컴포넌트
    Collider2D coll;                               // 충돌 처리를 위한 콜라이더
    Animator anim;                                 // 애니메이션 제어용 컴포넌트
    SpriteRenderer spriter;                        // 스프라이트 반전 등 시각 제어용
    WaitForFixedUpdate wait;                       // 코루틴에서 물리 프레임 대기할 때 사용

    void Awake()                                   // 게임 시작 시 한 번 실행되는 초기화 함수
    {
        rigid = GetComponent<Rigidbody2D>();       // Rigidbody2D 컴포넌트 연결
        coll = GetComponent<Collider2D>();         // Collider2D 연결
        anim = GetComponent<Animator>();           // Animator 연결
        spriter = GetComponent<SpriteRenderer>();  // SpriteRenderer 연결
        wait = new WaitForFixedUpdate();           // 한 프레임 물리 시간 대기 객체 생성
    }

    void FixedUpdate()                             // 일정한 간격으로 호출 (물리 연산)
    {
        if (!GameManager.instance.isLive)          // 게임이 정지 상태이면 실행하지 않음
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;                                // 죽었거나 맞는 중이면 이동 중지

        Vector2 dirVec = target.position - rigid.position;          // 플레이어 방향 벡터
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // 방향과 속도로 이동 벡터 계산
        rigid.MovePosition(rigid.position + nextVec);               // 다음 위치로 이동
        rigid.velocity = Vector2.zero;                              // 잔여 이동속도 제거
    }

    void LateUpdate()                              // 화면 그리기 직전 실행되는 함수
    {
        if (!GameManager.instance.isLive || !isLive)
            return;

        spriter.flipX = target.position.x < rigid.position.x;       // 플레이어가 왼쪽에 있으면 스프라이트 좌우 반전
    }

    void OnEnable()                                // 오브젝트가 활성화될 때 실행됨
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); // 추적 대상은 플레이어
        isLive = true;                             // 적 활성화
        coll.enabled = true;                       // 충돌 활성화
        rigid.simulated = true;                    // 물리 연산 활성화
        spriter.sortingOrder = 2;                  // 스프라이트 표시 순서 설정
        anim.SetBool("Dead", false);               // 죽음 애니메이션 비활성화
        health = maxHealth;                        // 체력 초기화
    }

    public void Init(SpawnData data)               // 적 생성 시 외형/속성 초기 설정
    {
        anim.runtimeAnimatorController = animCon[data.spriteType]; // 외형 설정
        speed = data.speed;                        // 이동 속도 설정
        maxHealth = data.health;                   // 최대 체력 설정
        health = data.health;                      // 현재 체력 설정
    }

    void OnTriggerEnter2D(Collider2D collision)    // 충돌 시 실행되는 함수
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage; // 총알의 데미지만큼 체력 감소
        StartCoroutine(KnockBack());                       // 넉백 효과 시작

        if (health > 0)                                    // 살아 있다면
        {
            anim.SetTrigger("Hit");                        // 맞는 애니메이션 재생
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit); // 타격 사운드 재생
        }
        else                                               // 체력이 0 이하일 경우
        {
            isLive = false;
            coll.enabled = false;                          // 충돌 비활성화
            rigid.simulated = false;                       // 물리 연산 정지
            spriter.sortingOrder = 1;                      // 스프라이트 우선순위 낮춤
            anim.SetBool("Dead", true);                    // 죽음 애니메이션 실행
            GameManager.instance.AddKill();                   // 게임매니저에 처치 수 증가
            GameManager.instance.GetExp();                 // 경험치 획득

            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead); // 적 사망 사운드 재생
        }
    }

    IEnumerator KnockBack()                        // 플레이어로부터 밀려나는 연출
    {
        yield return wait;                         // 한 물리 프레임 대기
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;  // 플레이어 반대방향 계산
        rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse); // 힘을 주어 튕겨나감
    }

    void Dead()                                    // 애니메이션 이벤트에서 호출 (사망 처리)
    {
        gameObject.SetActive(false);               // 오브젝트를 비활성화 (풀링 시스템 재활용)
    }
}