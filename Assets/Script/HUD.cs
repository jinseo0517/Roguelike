using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    public enum InforType { Exp, Level, Kill, Time, Health }    //ǥ���� ���� ���� ����
    public InforType type;  //�� ������Ʈ�� � ���� ������ ǥ������ �����ϴ� ����

    Text myText;        //�ؽ�Ʈ�� ǥ���� UI��� (ex: Lv.5, 3:40 ���� ����
    Slider mySlider;    //ü��,����ġ �ٿ� ���� �����̴��� UI ���

    void Awake()        //������ ���۵ɋ� �ѹ� �����
    {
        myText = GetComponent<Text>();  //���������Ʈ���� Text ������Ʈ�� ã�� myText�� ����
        mySlider = GetComponent<Slider>();  //���� ������Ʈ���� Slider������Ʈ�� ã�� mySlider�� ����
    }

    void LateUpdate()   //ȭ���� �׷����� ������ �� �����Ӹ��� �����
    {
        switch (type)   //���õ� ��������(type)�� ���� ȭ�鿡 ǥ���� ������ �ٸ��� ó��
        {
            case InforType.Exp:     //����ġ�ٸ� ǥ���� ���
                float curExp = GameManager.instance.exp;    //���� ����ġ ��������
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];    //���� ������ ���� �ִ� ����ġ�� ��������
                mySlider.value = curExp / maxExp;   //���� ����ġ�� ������ ����Ͽ� �����̴��� �ݿ�
                break;
            case InforType.Level:   //���� ������ ǥ���� ���
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);   //Lv.5 ���� �ؽ�Ʈ ǥ��
                break;
            case InforType.Kill:    //�� óġ ���� ǥ���� ���
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);       //999ó�� ���ڸ� ǥ��
                break;
            case InforType.Time:    //���� �ð��� ��:��:�� ���·� ǥ���� ���
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;    //��ü �ð����� �����ð� ����
                int min = Mathf.FloorToInt(remainTime / 60);    //���� �ð��� �� ���� ���
                int sec = Mathf.FloorToInt(remainTime % 60);    //���� �ð��� �� ���� ���
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);     //00:00 ó�� �������Ͽ� ǥ��
                break;
            case InforType.Health:
                float curHealth = GameManager.instance.health;      //ü�¹ٸ� ǥ���� ���
                float maxHealth = GameManager.instance.maxHealth;   //�ִ� ü�� ��������
                mySlider.value = curHealth / maxHealth;             //ü�� ������ �����̴��� �ݿ�
                break;
        }


    }
}
