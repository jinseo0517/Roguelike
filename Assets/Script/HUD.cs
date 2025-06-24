using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //  TMP ����� ���� ���ӽ����̽� �߰�

public class HUD : MonoBehaviour
{
    public enum InforType { Exp, Level, Kill, Time, Health } // ǥ���� ���� ���� ����
    public InforType type; // �� ������Ʈ�� � ������ ǥ������ �����ϴ� ����

    TextMeshProUGUI myText; //  TMP�� ����� �ؽ�Ʈ ������Ʈ
    Slider mySlider;        // ü��, ����ġ �ٿ� ���� �����̴��� UI ���

    void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>(); // TMP ������Ʈ ����
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