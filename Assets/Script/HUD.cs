using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    public enum InforType { Exp, Level, Kill, Time, Health }    //표시할 정보 종류 정의
    public InforType type;  //이 오브젝트가 어떤 종류 정보를 표시할지 선택하는 변수

    Text myText;        //텍스트를 표시할 UI요소 (ex: Lv.5, 3:40 같은 글자
    Slider mySlider;    //체력,경험치 바와 같은 슬라이더형 UI 요소

    void Awake()        //게임이 시작될떄 한번 실행됨
    {
        myText = GetComponent<Text>();  //현재오브젝트에서 Text 컴포넌트를 찾아 myText에 연결
        mySlider = GetComponent<Slider>();  //현재 오브젝트에서 Slider컴포넌트를 찾아 mySlider에 연결
    }

    void LateUpdate()   //화면이 그려지기 직전에 매 프레임마다 실행됨
    {
        switch (type)   //선택된 정보종류(type)에 따라 화면에 표시할 내용을 다르게 처리
        {
            case InforType.Exp:     //경험치바를 표시할 경우
                float curExp = GameManager.instance.exp;    //현재 경험치 가져오기
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];    //현재 레벨에 따른 최대 경험치값 가져오기
                mySlider.value = curExp / maxExp;   //현재 경험치를 비율로 계산하여 슬라이더에 반영
                break;
            case InforType.Level:   //현재 레벨을 표시할 경우
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);   //Lv.5 같은 텍스트 표시
                break;
            case InforType.Kill:    //적 처치 수를 표시할 경우
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);       //999처럼 숫자만 표시
                break;
            case InforType.Time:    //남은 시간을 시:분:초 형태로 표시할 경우
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;    //전체 시간에서 지난시간 빼기
                int min = Mathf.FloorToInt(remainTime / 60);    //남은 시간중 분 단위 계산
                int sec = Mathf.FloorToInt(remainTime % 60);    //남은 시간중 초 단위 계산
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);     //00:00 처럼 포맷팅하여 표시
                break;
            case InforType.Health:
                float curHealth = GameManager.instance.health;      //체력바를 표시할 경우
                float maxHealth = GameManager.instance.maxHealth;   //최대 체력 가져오기
                mySlider.value = curHealth / maxHealth;             //체력 비율로 슬라이더에 반영
                break;
        }


    }
}
