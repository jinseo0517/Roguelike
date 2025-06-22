using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockChracter;   //잠긴 캐릭터를 표시하는 오브젝트 배열
    public GameObject[] unlockChracter; //잠금 해제된 캐릭터를 표시하는 오브젝트 배열
    public GameObject uiNotice;         //알림ui 오브젝트

    enum Achive {  UnlockJaeyong, UnlockSeongeun }  //업적 종류 정의
    Achive[] achives;               //정의한 업적들을 배열로 저장
    WaitForSecondsRealtime wait;    //일정 시간 기다리기 위한 변수(알림용)

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive)); //열거형 값을 배열로 변환
        wait = new WaitForSecondsRealtime(5);               //5초동안 기다리는 객체 생성

        if (!PlayerPrefs.HasKey("MyData"))  //저장된 데이터가 없으면
        {
            Init();                         //초기값 설정
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);                //저장된 데이터 존재 표시

        foreach (Achive achive in achives)              //모든 업적에 대해
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);   //업적 초기값을 0으로 설정(잠김 상태)
        }
    }

    void Start()
    {
        UnlockCharacter();  //시작시 잠금 여부 확인해서 캐릭터 상태 설정
    }

    void UnlockCharacter()
    {
        for (int index = 0; index < lockChracter.Length; index++)   //각 캐릭터마다
        {
            string achiveName = achives[index].ToString();          //업적 이름 가져오기
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;    //해당 업적이 해금되었는지 확인
            lockChracter[index].SetActive(!isUnlock);               //잠긴 캐릭터는 업적 미완료일때 활성화
            unlockChracter[index].SetActive(isUnlock);              //해금된 캐릭터는 업적 완료일떄 활성화
        }
    }

    void LateUpdate()
    {
        foreach (Achive achive in achives)  //매 프레임마다 업적 조건 체크
        {
            CheckAchive(achive);            //해당 업적 충족 여부 확인
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;      //업적 달성 여부

        switch (achive)
        {
            case Achive.UnlockJaeyong:
                isAchive = GameManager.instance.kill >= 10;     //적 10마리 처치시 업적달성
                break;
            case Achive.UnlockSeongeun:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;   //제한시간까지 생존시 업적달성
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0) //업적을 처음 달성했을 경우
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);               //업적을 완료로 저장

            for (int index=0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;               //현재 업적에 해당하는 UI만 활성화
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());    //알림 UI표시 코루틴 실행
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);   //알림 UI보이기
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Levelup);    //공지 레벨업 효과음

        yield return wait;

        uiNotice.SetActive(false);  //알림 UI숨기기
    }
}
