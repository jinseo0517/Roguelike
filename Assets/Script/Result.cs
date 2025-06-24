using System.Collections;                          // Unity의 기본 컬렉션 기능 사용
using System.Collections.Generic;                  // List, Dictionary 등 제네릭 컬렉션 기능
using UnityEngine;                                 // Unity 엔진의 핵심 기능 사용

public class Result : MonoBehaviour                // 게임 결과(승리 또는 패배) 화면을 관리하는 클래스
{
    public GameObject[] titles;                    // 0번: 패배 화면, 1번: 승리 화면 등을 담은 게임 오브젝트 배열

    public void Lose()                             // 게임 오버 시 호출되는 함수
    {
        titles[0].SetActive(true);                 // 패배 타이틀(첫 번째 오브젝트)을 화면에 표시
    }

    public void Win()                              // 게임 승리 시 호출되는 함수
    {
        titles[1].SetActive(true);                 // 승리 타이틀(두 번째 오브젝트)을 화면에 표시
    }
}