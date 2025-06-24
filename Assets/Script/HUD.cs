using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //  TMP 사용을 위한 네임스페이스 추가

public class HUD : MonoBehaviour
{
    public enum InforType { Exp, Level, Kill, Time, Health } // 표시할 정보 종류 정의
    public InforType type; // 이 오브젝트가 어떤 정보를 표시할지 선택하는 변수

    TextMeshProUGUI myText; //  TMP로 변경된 텍스트 컴포넌트
    Slider mySlider;        // 체력, 경험치 바와 같은 슬라이더형 UI 요소

    void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>(); // TMP 컴포넌트 연결
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InforType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                mySlider.value = curExp / maxExp;
                break;

            case InforType.Level:
                myText.text = $"Lv.{GameManager.instance.level:F0}";
                break;

            case InforType.Kill:
                myText.text = $"{GameManager.instance.kill:F0}";
                break;

            case InforType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = $"{min:D2}:{sec:D2}";
                break;

            case InforType.Health:
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}