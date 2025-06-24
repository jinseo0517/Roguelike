using System.Collections;                          // Unity의 기본 컬렉션 기능 사용
using System.Collections.Generic;                  // 제네릭 컬렉션(List, Dictionary 등) 사용
using UnityEngine;                                 // Unity 엔진 기능 사용

public class Spawner : MonoBehaviour               // Spawner 클래스: 적을 주기적으로 생성하는 역할
{
    public Transform[] spawnPoint;                 // 적이 생성될 수 있는 위치들 (여러 위치 중 랜덤)
    public SpawnData[] spawnData;                  // 시간에 따른 적 생성 정보(속도, 체력 등)
    public float levelTime;                        // 레벨 전환 간격 시간

    int level;                                     // 현재 레벨 인덱스 (게임이 진행됨에 따라 증가)
    float timer;                                   // 경과 시간 저장용 변수

    void Awake()                                   // 스크립트가 시작될 때 실행되는 초기화 함수
    {
        spawnPoint = GetComponentsInChildren<Transform>(); // 이 오브젝트의 자식들에서 위치 좌표들을 가져옴
        levelTime = GameManager.instance.maxGameTime / spawnData.Length; // 전체 게임시간을 레벨 수로 나눠서 단계별 시간 설정
    }

    void Update()                                  // 매 프레임 실행되는 함수 (적 생성 타이밍 체크)
    {
        if (!GameManager.instance.isLive)          // 게임이 진행 중이 아니라면 아무것도 안 함
            return;

        timer += Time.deltaTime;                   // 프레임 간 경과 시간 누적
        level = Mathf.Min(                         // 현재 경과 시간 기준으로 레벨 인덱스 계산
            Mathf.FloorToInt(GameManager.instance.gameTime / levelTime),
            spawnData.Length - 1);                 // 마지막 레벨을 넘지 않도록 제한

        if (timer > spawnData[level].spawnTime)    // 현재 레벨 기준으로 설정된 적 출현 주기 초과 시
        {
            timer = 0;                             // 타이머 초기화
            Spawn();                               // 적 생성 실행
        }
    }

    void Spawn()                                   // 적을 하나 생성하는 함수
    {
        GameObject enemy = GameManager.instance.pool.Get(0); // 오브젝트 풀에서 Enemy 타입 오브젝트 꺼냄
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        // 출현 위치 중 랜덤 선택 (0번은 Spawner 자기 자신이니 제외)
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
        // 현재 레벨에 맞는 스폰 정보(속도, 체력 등) 적용
    }
}

[System.Serializable]                             // Unity 에디터에서 설정값 노출되도록 만들기 위한 어노테이션
public class SpawnData                             // 적 생성 시 필요한 정보들을 묶은 데이터 클래스
{
    public float spawnTime;                        // 적이 얼마나 자주 등장할지 설정하는 시간 (초 단위)
    public int spriteType;                         // 적 외형(스프라이트 종류)을 구분하기 위한 번호
    public int health;                             // 적의 체력
    public float speed;                            // 적의 이동 속도
}