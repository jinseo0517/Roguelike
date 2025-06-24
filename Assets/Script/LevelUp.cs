using System.Collections;                          // Unity의 기본 컬렉션 기능
using System.Collections.Generic;                  // 제네릭 컬렉션 (List 등)
using UnityEngine;                                 // Unity 엔진 기능 사용

public class LevelUp : MonoBehaviour               // 레벨업 UI를 관리하는 클래스
{
    RectTransform rect;                            // 이 오브젝트의 UI 위치/크기 정보
    Item[] items;                                  // 레벨업 시 등장할 아이템 목록들

    void Awake()                                   // 스크립트 활성화 시 한 번 실행됨
    {
        rect = GetComponent<RectTransform>();      // 이 UI 오브젝트의 RectTransform 연결
        items = GetComponentsInChildren<Item>(true); // 자식 중 Item 스크립트가 붙은 것들 모두 가져오기 (비활성 포함)
    }

    public void Show()                             // 레벨업 UI 표시 함수
    {
        Next();                                     // 랜덤 아이템 3개 선택
        rect.localScale = Vector3.one;              // UI를 보이도록 크기 설정
        GameManager.instance.Stop();                // 게임 일시정지
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Levelup); // 레벨업 효과음 재생
        AudioManager.instance.EffectBgm(true);      // 배경음 페이드/보조 음 재생
    }

    public void Hide()                             // 레벨업 UI 숨기기 함수
    {
        rect.localScale = Vector3.zero;             // UI 비활성화 (크기 0으로)
        GameManager.instance.Resume();              // 게임 재시작
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select); // 선택 효과음 재생
        AudioManager.instance.EffectBgm(false);     // 보조 BGM 정지
    }

    public void Select(int index)                  // 유저가 아이템을 선택했을 때 호출됨
    {
        items[index].OnClik();                      // 해당 아이템의 클릭 동작 실행
    }

    void Next()                                    // 다음 레벨업 아이템 후보 3개 선택
    {
        // 1. 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. 그중에서 랜덤하게 서로 다른 3개의 아이템 인덱스 뽑기
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;                              // 3개가 서로 다르면 종료
        }

        for (int index = 0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[index]];       // 랜덤으로 선택된 아이템

            // 3. 선택된 아이템이 만렙일 경우, 대체로 소비 아이템 보여줌
            if (ranItem.level == ranItem.data.damages.Length) // 현재 레벨이 최대 레벨이면
            {
                items[4].gameObject.SetActive(true); // 예비 소비 아이템 (ex: 회복 등) 활성화
            }
            else
            {
                ranItem.gameObject.SetActive(true);  // 일반 아이템은 그대로 표시
            }
        }
    }
}