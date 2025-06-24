using System.Collections;                          // Unity의 기본 컬렉션 기능 사용
using System.Collections.Generic;                  // 제네릭 컬렉션(List 등) 사용
using UnityEngine;                                 // Unity 엔진 기능 사용

public class Scanner : MonoBehaviour               // 근처의 적을 자동으로 탐지하는 기능을 하는 클래스
{
    public float scanRange;                        // 탐지 범위 (반지름)
    public LayerMask targetLayer;                  // 어떤 레이어의 오브젝트를 탐지할지 설정
    public RaycastHit2D[] targets;                 // 탐지된 대상들 저장
    public Transform nearestTarget;                // 가장 가까운 대상의 Transform

    void FixedUpdate()                             // 물리 연산 주기마다 실행
    {
        targets = Physics2D.CircleCastAll(
            transform.position,                    // 현재 위치를 기준으로
            scanRange,                             // 반지름 내에서
            Vector2.zero,                          // 방향 없음 (점 중심 탐지)
            0,
            targetLayer                            // 지정된 레이어만 탐지
        );

        nearestTarget = GetNearest();              // 가장 가까운 대상 찾기
    }

    Transform GetNearest()                         // 가장 가까운 대상 계산
    {
        Transform result = null;                   // 결과 저장용 변수
        float diff = 100;                          // 현재까지 가장 짧은 거리 (초기값 충분히 크게 설정)

        foreach (RaycastHit2D target in targets)   // 탐지된 모든 대상 중에서
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos); // 현재 거리 계산

            if (curDiff < diff)                    // 더 가까운 대상이면
            {
                diff = curDiff;                    // 가장 짧은 거리 갱신
                result = target.transform;         // 결과로 저장
            }
        }

        return result;                             // 가장 가까운 대상 반환
    }
}