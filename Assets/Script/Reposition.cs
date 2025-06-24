using System.Collections;                          // Unity 기본 컬렉션 사용
using System.Collections.Generic;                  // 제네릭 컬렉션 사용
using UnityEngine;                                 // Unity의 핵심 기능 사용

public class Reposition : MonoBehaviour            // 맵 또는 적을 재배치하는 스크립트
{
    Collider2D coll;                               // 자신의 Collider2D를 저장할 변수

    void Awake()
    {
        coll = GetComponent<Collider2D>();         // 자신의 Collider2D 컴포넌트를 가져옴
    }

    private void OnTriggerExit2D(Collider2D collision) // 충돌한 오브젝트가 자신의 범위를 벗어날 때 실행
    {
        if (!collision.CompareTag("Area"))         // "Area"가 아닌 경우 무시 (경계 밖으로 나가는지 확인용)
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position; // 플레이어 위치
        Vector3 myPos = transform.position;        // 현재 오브젝트 위치

        switch (transform.tag)                     // Ground(땅) 또는 Enemy(적) 태그에 따라 다르게 처리
        {
            case "Ground":                         // 땅 타일일 경우 위치 재배치
                float diffX = playerPos.x - myPos.x; // X축 거리 차이
                float diffY = playerPos.y - myPos.y; // Y축 거리 차이
                float dirX = diffX < 0 ? -1 : 1;   // X축 방향 (-1이면 왼쪽, 1이면 오른쪽)
                float dirY = diffY < 0 ? -1 : 1;   // Y축 방향

                diffX = Mathf.Abs(diffX);         // 거리 차이 절대값 계산
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY)                // X축 차이가 더 클 경우 → 좌우로 이동
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)           // Y축 차이가 더 클 경우 → 위아래로 이동
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enemy":                          // 적일 경우
                if (coll.enabled)                  // Collider가 활성화되어 있을 때만 이동
                {
                    Vector3 dist = playerPos - myPos; // 플레이어와 현재 위치 차이 (오타: = → - 가 되어야 할 듯)
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0); // 랜덤 좌표 추가
                    transform.Translate(ran + dist * 2); // 거리 방향으로 이동 + 랜덤 이동
                }
                break;
        }
    }
}