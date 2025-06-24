using System.Collections;                          // Unity의 기본 컬렉션 기능 사용
using System.Collections.Generic;                  // List, Dictionary 등 제네릭 컬렉션 사용
using UnityEngine;                                 // Unity 엔진의 핵심 기능 사용

[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")] // 프로젝트 내에서 ScriptableObject 생성 메뉴 등록
public class ItemData : ScriptableObject           // 아이템의 데이터를 담기 위한 스크립터블 오브젝트
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal } // 아이템의 종류 (무기/장비/소비 등)

    [Header("# Main Info")]                         // 에디터에서 구분선 표시
    public ItemType itemType;                       // 아이템 종류
    public int itemId;                              // 아이템 고유 번호
    public string itemName;                         // 아이템 이름
    [TextArea]
    public string itemDesc;                         // 아이템 설명 (UI에 표시될 텍스트)
    public Sprite itemIcon;                         // 아이템 아이콘 이미지

    [Header("# Level Data")]                        // 레벨별 능력치 데이터
    public float baseDamage;                        // 기본 데미지
    public int baseCount;                           // 기본 발사 개수 (무기)
    public float[] damages;                         // 레벨별 추가 데미지 비율
    public int[] counts;                            // 레벨별 추가 발사 수

    [Header("# Weapon")]                            // 무기 종류일 경우 추가 정보
    public GameObject projectile;                   // 발사할 총알 prefab
    public Sprite hand;                             // 캐릭터 손에 표시될 무기 그래픽
}