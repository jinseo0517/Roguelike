using System.Collections;                          // Unity의 기본 컬렉션 기능 사용
using System.Collections.Generic;                  // List, Dictionary 등 제네릭 컬렉션 사용
using UnityEngine;                                 // Unity 엔진의 핵심 기능 사용

public class Weapon : MonoBehaviour                // 무기 동작을 제어하는 클래스
{
    public int id;                                 // 무기 종류 ID
    public int prefabId;                           // 총알 prefab 번호
    public float damage;                           // 무기의 공격력
    public int count;                              // 발사되는 총알 개수 (또는 회전형 무기 수)
    public float speed;                            // 회전 속도 또는 발사 간격

    float timer;                                   // 발사 타이머
    Player player;                                 // 플레이어 참조용 변수

    void Awake()                                   // 시작 시 한 번 실행: 참조 연결
    {
        player = GameManager.instance.player;      // GameManager를 통해 Player 참조 받기
    }

    void Update()                                  // 매 프레임 실행
    {
        if (!GameManager.instance.isLive)          // 게임 중이 아니면 실행 중지
            return;

        switch (id)                                // 무기 종류에 따라 동작 다르게 처리
        {
            case 0:                                // 회전형 무기일 경우
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // 회전 시킴
                break;
            default:                               // 발사형 무기일 경우
                timer += Time.deltaTime;

                if (timer > speed)                 // 발사 속도만큼 시간 지났다면
                {
                    timer = 0f;
                    Fire();                        // 총알 발사
                }
                break;
        }

        // .. 테스트용 코드 (스페이스바 누르면 레벨업 시킴)
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);                        // 공격력 10, 발사 수 1 증가
        }
    }

    public void LevelUp(float damage, int count)   // 무기 업그레이드 (데미지, 발사 수 증가)
    {
        this.damage = damage * Character.Damage;   // 캐릭터 공격력 보정 반영
        this.count += count;                       // 발사 수 증가

        if (id == 0)
            Batch();                               // 회전형 무기일 경우 재배치 필요

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        // 플레이어 쪽에 장비 변경 알리기
    }

    public void Init(ItemData data)                // 무기 초기화 함수 (무기 생성 시 호출)
    {
        //Basic Set
        name = "Weapon " + data.itemId;            // 오브젝트 이름 지정
        transform.parent = player.transform;       // 플레이어에 붙이기
        transform.localPosition = Vector3.zero;    // 위치 초기화

        //Property Set
        id = data.itemId;                          // 무기 ID 설정
        damage = data.baseDamage * Character.Damage; // 캐릭터 공격력 반영
        count = data.baseCount + Character.Count;  // 발사 수 캐릭터 보정 반영

        // 총알 prefabId 찾기
        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        // 속도 설정 및 회전 무기 초기 생성
        switch (id)
        {
            case 0:                                // 회전형 무기
                speed = 150 * Character.WeaponSpeed;
                Batch();                           // 총알 여러 개 배치
                break;
            default:                               // 발사형 무기
                speed = 0.4f * Character.WeaponRate; // 발사 간격 계산
                break;
        }

        // Hand(손 그래픽) 설정
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;           // 손에 장착된 무기의 스프라이트 적용
        hand.gameObject.SetActive(true);           // 손 활성화

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // 장비 적용 알리기
    }

    void Batch()                                   // 회전형 무기의 총알을 배치하는 함수
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            if (index < transform.childCount)      // 기존 총알이 있으면 재활용
            {
                bullet = transform.GetChild(index);
            }
            else                                   // 없으면 풀에서 새로 꺼내기
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;         // 무기 오브젝트에 붙이기
            }

            bullet.localPosition = Vector3.zero;   // 위치 초기화
            bullet.localRotation = Quaternion.identity; // 회전 초기화

            Vector3 rotVec = Vector3.forward * 360 * index / count; // 회전 각도 계산
            bullet.Rotate(rotVec);                // 각도 회전
            bullet.Translate(bullet.up * 1.5f, Space.World); // 위치 이동
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
            // 총알 초기화 (-100 = 무한 관통)
        }
    }

    void Fire()                                    // 발사형 무기에서 총알을 발사하는 함수
    {
        if (!player.scanner.nearestTarget)         // 근처에 적이 없으면 발사 안 함
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position; // 목표 위치
        Vector3 dir = targetPos - transform.position;              // 방향 계산
        dir = dir.normalized;                                      // 정규화

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;       // 총알 위치 설정
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // 방향으로 회전 설정
        bullet.GetComponent<Bullet>().Init(damage, count, dir);   // 총알 세팅 및 발사

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range); // 발사 사운드 재생
    }
}