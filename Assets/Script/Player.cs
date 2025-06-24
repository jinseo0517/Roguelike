using System.Collections;                          // Unity의 기본 컬렉션 기능
using System.Collections.Generic;                  // List, Dictionary 등을 사용하기 위한 확장 컬렉션
using UnityEngine;                                 // Unity 엔진의 핵심 기능
using UnityEngine.InputSystem;                     // 새로운 입력 시스템(Input System)을 사용하기 위한 네임스페이스

public class Player : MonoBehaviour                // Player 클래스: 플레이어 캐릭터를 제어하는 스크립트
{
    public Vector2 inputVec;                       // 플레이어 입력 방향 값 (X, Y)
    public float speed;                            // 이동 속도
    public Scanner scanner;                        // 주변 탐지 컴포넌트 (적 탐지 등)
    public Hand[] hands;                           // 무기를 장착할 손 오브젝트 배열
    public RuntimeAnimatorController[] animCon;    // 캐릭터 별 애니메이션 컨트롤러 목록

    Rigidbody2D rigid;                             // 2D 물리 이동을 위한 리지드바디
    SpriteRenderer spriter;                        // 캐릭터 스프라이트 렌더링 제어용
    Animator anim;                                 // 애니메이션 컨트롤러

    void Awake()                                   // 게임 시작 시 한 번 실행되는 초기화 함수
    {
        rigid = GetComponent<Rigidbody2D>();       // Rigidbody2D 컴포넌트 찾기
        spriter = GetComponent<SpriteRenderer>();  // 스프라이트 렌더러 컴포넌트 찾기
        anim = GetComponent<Animator>();           // 애니메이터 컴포넌트 찾기
        scanner = GetComponent<Scanner>();         // 탐지기(Scanner) 컴포넌트 찾기
        hands = GetComponentsInChildren<Hand>(true); // 자식 오브젝트에서 Hand 컴포넌트들 모두 찾기 (비활성 포함)
    }

    void OnEnable()                                // 오브젝트가 활성화될 때 실행
    {
        speed *= Character.Speed;                  // 캐릭터 능력치(속도 보정치) 반영
        anim.runtimeAnimatorController =           // 선택된 캐릭터의 애니메이션 컨트롤러 적용
            animCon[GameManager.instance.playerId];
    }

    void FixedUpdate()                             // 물리 연산 전용 업데이트 (프레임 수와 무관하게 일정하게 호출됨)
    {
        if (!GameManager.instance.isLive)          // 게임이 중단되었으면 아무것도 하지 않음
            return;

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime; // 이동 방향에 속도 및 시간 보정 곱하기
        rigid.MovePosition(rigid.position + nextVec);              // 새로운 위치로 이동
    }

    void OnMove(InputValue value)                  // 입력 시스템으로부터 이동 입력이 들어올 때 호출됨
    {
        inputVec = value.Get<Vector2>();           // 입력된 방향 값을 저장 (조이스틱이나 키보드 방향)
    }

    void LateUpdate()                              // 모든 Update가 끝난 뒤 실행 (주로 시각 효과 처리)
    {
        if (!GameManager.instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude); // 애니메이션에 이동 속도 전달 (0이면 Idle 상태)

        if (inputVec.x != 0)                        // 좌우로 움직일 경우 방향을 반영하여 스프라이트 뒤집기
        {
            spriter.flipX = inputVec.x < 0;         // 왼쪽으로 이동 중이면 스프라이트를 좌우 반전
        }
    }

    void OnCollisionStay2D(Collision2D collision)  // 2D 충돌이 계속되는 동안 매 프레임 호출됨
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10; // 충돌 중이면 초당 10만큼 체력 감소

        if (GameManager.instance.health < 0)        // 체력이 0 이하가 되면
        {
            for (int index = 2; index < transform.childCount; index++) // 손이나 무기 등 자식 오브젝트들 끄기
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");               // 죽는 애니메이션 실행
            GameManager.instance.GameOver();       // 게임 매니저에 GameOver 처리 요청
        }
    }
}