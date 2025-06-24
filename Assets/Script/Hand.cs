using System.Collections;                          // Unity의 기본 컬렉션 기능
using System.Collections.Generic;                  // List, Dictionary 등 제네릭 컬렉션
using UnityEngine;                                 // Unity 엔진 기능 사용

public class Hand : MonoBehaviour                  // 캐릭터 손(무기 장착 위치)을 조정하는 클래스
{
    public bool isLeft;                            // 왼손 여부 (근접무기면 true)
    public SpriteRenderer spriter;                 // 손에 장착된 무기의 스프라이트 렌더러

    SpriteRenderer player;                         // 플레이어 본체의 스프라이트 렌더러

    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);         // 오른손 기본 위치
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0); // 반전된 오른손 위치
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);         // 왼손 회전 각도
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135); // 반전된 왼손 회전 각도

    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];  // 부모 중 두 번째 SpriteRenderer 가져오기 (플레이어)
    }

    void LateUpdate()                                         // 시각 요소 반영을 위해 LateUpdate에서 실행
    {
        bool isReverse = player.flipX;                        // 플레이어가 좌우 반전된 상태인지 확인

        if (isLeft)                                           // 근접 무기일 경우 (왼손)
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot; // 반전 여부에 따른 회전 각도 설정
            spriter.flipY = isReverse;                                   // 위아래 반전 여부
            spriter.sortingOrder = isReverse ? 4 : 6;                    // 렌더링 순서 조절 (앞/뒤 위치)
        }
        else                                                  // 원거리 무기일 경우 (오른손)
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos; // 위치 조정 (반전 여부에 따라)
            spriter.flipX = isReverse;                                  // 좌우 반전
            spriter.sortingOrder = isReverse ? 6 : 4;                   // 렌더링 순서 반대로 설정
        }
    }
}