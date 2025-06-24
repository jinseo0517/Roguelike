using System.Collections;                          // Unity에서 기본 컬렉션 기능을 사용하기 위한 네임스페이스
using System.Collections.Generic;                  // List, Dictionary 같은 제네릭 컬렉션을 사용하기 위한 네임스페이스
using UnityEngine;                                 // Unity의 핵심 기능을 불러오는 네임스페이스

public class Character : MonoBehaviour             // Character 클래스: Unity 오브젝트에 붙는 스크립트
{
    public static float Speed                      // 이동 속도: playerId가 0번일 경우 1.1배, 아니면 기본 1.0
    {
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
    }

    public static float WeaponSpeed                // 무기 속도: playerId가 1번일 경우 1.1배, 아니면 1.0
    {
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    }

    public static float WeaponRate                 // 무기 공격 간격: playerId가 1번이면 0.9 (더 빠르게), 아니면 1.0
    {
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f; }
    }

    public static float Damage                     // 공격력: playerId가 2번일 경우 1.2배, 아니면 1.0
    {
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f; }
    }

    public static int Count                        // 추가 투사체 수: playerId가 3번이면 1개 추가, 아니면 0
    {
        get { return GameManager.instance.playerId == 3 ? 1 : 0; }
    }
}