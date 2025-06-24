using System.Collections;                          // Unity의 기본 컬렉션 기능 사용
using System.Collections.Generic;                  // 제네릭 컬렉션(List 등) 사용
using UnityEngine;                                 // Unity 엔진 기능 사용
using UnityEngine.UI;                              // UI(Text, Image, Button 등)을 다루기 위한 네임스페이스

public class Item : MonoBehaviour                  // 레벨업 UI에서 표시되고 선택 가능한 아이템을 나타내는 클래스
{
    public ItemData data;                          // 아이템의 정보(데미지, 이름, 아이콘 등)를 담은 데이터
    public int level;                              // 현재 아이템 레벨
    public Weapon weapon;                          // 무기일 경우 연결될 Weapon 클래스
    public Gear gear;                              // 장비일 경우 연결될 Gear 클래스

    Image icon;                                    // 아이템 아이콘 이미지
    Text textLevel;                                // 아이템 레벨을 표시할 텍스트
    Text textName;                                 // 아이템 이름을 표시할 텍스트
    Text textDesc;                                 // 아이템 설명을 표시할 텍스트

    void Awake()                                   // 오브젝트가 처음 생성될 때 호출됨
    {
        icon = GetComponentsInChildren<Image>()[1];    // 두 번째 Image 컴포넌트를 아이콘으로 설정
        icon.sprite = data.itemIcon;                   // 아이템 이미지 설정

        Text[] texts = GetComponentsInChildren<Text>(); // 자식 오브젝트들의 텍스트 컴포넌트 모두 가져오기
        textLevel = texts[0];                          // 첫 번째 텍스트: 레벨
        textName = texts[1];                           // 두 번째 텍스트: 이름
        textDesc = texts[2];                           // 세 번째 텍스트: 설명
        textName.text = data.itemName;                 // 이름 텍스트 설정
    }

    void OnEnable()                                   // 오브젝트가 활성화될 때마다 호출됨
    {
        textLevel.text = "Lv." + (level + 1);         // 현재 레벨+1 표시

        switch (data.itemType)                        // 아이템 종류에 따라 설명 방식 다르게 처리
        {
            case ItemData.ItemType.Melee:             // 근접 무기
            case ItemData.ItemType.Range:             // 원거리 무기
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:             // 장갑
            case ItemData.ItemType.Shoe:              // 신발
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:                                  // 소비 아이템 등 나머지
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    public void OnClik()                              // 아이템을 선택(클릭)했을 때 호출됨
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:             // 근접 무기
            case ItemData.ItemType.Range:             // 원거리 무기
                if (level == 0)                       // 처음 선택되었을 경우
                {
                    GameObject newWeapon = new GameObject();         // 새 오브젝트 생성
                    weapon = newWeapon.AddComponent<Weapon>();       // 무기 컴포넌트 추가
                    weapon.Init(data);                               // 데이터로 초기화
                }
                else                                  // 이미 무기를 가진 상태라면 업그레이드
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level]; // 다음 데미지 계산
                    nextCount += data.counts[level];                     // 추가 발사 수

                    weapon.LevelUp(nextDamage, nextCount);               // 레벨업 적용
                }

                level++;                             // 레벨 증가
                break;

            case ItemData.ItemType.Glove:            // 장갑 (공격속도 등)
            case ItemData.ItemType.Shoe:             // 신발 (이동속도 등)
                if (level == 0)
                {
                    GameObject newGear = new GameObject();             // 새 오브젝트 생성
                    gear = newGear.AddComponent<Gear>();               // Gear 컴포넌트 추가
                    gear.Init(data);                                   // 데이터로 초기화
                }
                else
                {
                    float nextRate = data.damages[level];              // 다음 단계 수치
                    gear.LevelUp(nextRate);                            // 레벨업 적용
                }

                level++;                             // 레벨 증가
                break;

            case ItemData.ItemType.Heal:             // 회복 아이템
                GameManager.instance.health = GameManager.instance.maxHealth; // 체력 회복
                break;
        }

        if (level == data.damages.Length)            // 최대 레벨에 도달하면
        {
            GetComponent<Button>().interactable = false; // 더 이상 선택 불가 (비활성화)
        }
    }
}