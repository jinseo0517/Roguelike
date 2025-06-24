using System.Collections;                          // Unity의 기본 컬렉션 기능 사용
using System.Collections.Generic;                  // 제네릭 컬렉션(List 등) 사용
using UnityEngine;                                 // Unity 엔진 기능 사용

public class Gear : MonoBehaviour                  // 장비(Gear)의 동작을 정의하는 클래스
{
    public ItemData.ItemType type;                 // 장비 종류 (장갑/신발 등)
    public float rate;                             // 능력치에 적용할 비율(속도 향상 비율 등)

    public void Init(ItemData data)                // 장비를 새로 생성했을 때 초기 설정
    {
        //Basic Set
        name = "Gear " + data.itemId;              // 오브젝트 이름 지정
        transform.parent = GameManager.instance.player.transform; // 플레이어에 붙이기
        transform.localPosition = Vector3.zero;    // 위치 초기화

        //Property Set
        type = data.itemType;                      // 장비 타입 설정
        rate = data.damages[0];                    // 첫 레벨 기준 능력치 비율 설정
        ApplyGear();                               // 장비 효과 적용
    }

    public void LevelUp(float rate)                // 장비가 업그레이드될 때 호출
    {
        this.rate = rate;                          // 새 비율 저장
        ApplyGear();                               // 다시 효과 적용
    }

    void ApplyGear()                               // 장비 종류에 따라 효과 적용 방식 분기
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:          // 장갑이면 무기 발사 속도 영향
                RateUp();
                break;
            case ItemData.ItemType.Shoe:           // 신발이면 이동 속도 향상
                SpeedUp();
                break;
        }
    }

    void RateUp()                                  // 무기 속도 증가 (공격 빈도 높임)
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>(); // 플레이어의 모든 무기 가져오기

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:                             // 회전 무기 (id 0번일 경우)
                    float speed = 150 * Character.WeaponSpeed;       // 기본 속도 계산
                    weapon.speed = speed + (speed * rate);           // 속도를 비율만큼 증가
                    break;
                default:                            // 발사형 무기
                    speed = 0.5f * Character.WeaponRate;             // 기본 발사 간격
                    weapon.speed = speed * (1f - rate);              // 발사 간격을 줄이기 (더 빠르게)
                    break;
            }
        }
    }

    void SpeedUp()                                 // 이동 속도 증가 처리
    {
        float speed = 3 * Character.Speed;          // 기본 이동 속도 계산
        GameManager.instance.player.speed = speed + speed * rate; // 비율만큼 증가
    }
}