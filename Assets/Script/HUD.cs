using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InforType { Exp, Level, Kill, Time, Health }
    public InforType type;

    Text myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InforType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];
                mySlider.value = curExp / maxExp;
                break;
            case InforType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InforType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InforType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InforType.Health:
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
        }


    }
}
