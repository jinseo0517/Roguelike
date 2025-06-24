using System.Collections;                          // Unity�� �⺻ �÷��� ��� ���
using System.Collections.Generic;                  // ���׸� �÷���(List ��) ���
using UnityEngine;                                 // Unity ���� ��� ���
using UnityEngine.UI;                              // UI(Text, Image, Button ��)�� �ٷ�� ���� ���ӽ����̽�

public class Item : MonoBehaviour                  // ������ UI���� ǥ�õǰ� ���� ������ �������� ��Ÿ���� Ŭ����
{
    public ItemData data;                          // �������� ����(������, �̸�, ������ ��)�� ���� ������
    public int level;                              // ���� ������ ����
    public Weapon weapon;                          // ������ ��� ����� Weapon Ŭ����
    public Gear gear;                              // ����� ��� ����� Gear Ŭ����

    Image icon;                                    // ������ ������ �̹���
    Text textLevel;                                // ������ ������ ǥ���� �ؽ�Ʈ
    Text textName;                                 // ������ �̸��� ǥ���� �ؽ�Ʈ
    Text textDesc;                                 // ������ ������ ǥ���� �ؽ�Ʈ

    void Awake()                                   // ������Ʈ�� ó�� ������ �� ȣ���
    {
        icon = GetComponentsInChildren<Image>()[1];    // �� ��° Image ������Ʈ�� ���������� ����
        icon.sprite = data.itemIcon;                   // ������ �̹��� ����

        Text[] texts = GetComponentsInChildren<Text>(); // �ڽ� ������Ʈ���� �ؽ�Ʈ ������Ʈ ��� ��������
        textLevel = texts[0];                          // ù ��° �ؽ�Ʈ: ����
        textName = texts[1];                           // �� ��° �ؽ�Ʈ: �̸�
        textDesc = texts[2];                           // �� ��° �ؽ�Ʈ: ����
        textName.text = data.itemName;                 // �̸� �ؽ�Ʈ ����
    }

    void OnEnable()                                   // ������Ʈ�� Ȱ��ȭ�� ������ ȣ���
    {
        textLevel.text = "Lv." + (level + 1);         // ���� ����+1 ǥ��

        switch (data.itemType)                        // ������ ������ ���� ���� ��� �ٸ��� ó��
        {
            case ItemData.ItemType.Melee:             // ���� ����
            case ItemData.ItemType.Range:             // ���Ÿ� ����
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:             // �尩
            case ItemData.ItemType.Shoe:              // �Ź�
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:                                  // �Һ� ������ �� ������
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    public void OnClik()                              // �������� ����(Ŭ��)���� �� ȣ���
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:             // ���� ����
            case ItemData.ItemType.Range:             // ���Ÿ� ����
                if (level == 0)                       // ó�� ���õǾ��� ���
                {
                    GameObject newWeapon = new GameObject();         // �� ������Ʈ ����
                    weapon = newWeapon.AddComponent<Weapon>();       // ���� ������Ʈ �߰�
                    weapon.Init(data);                               // �����ͷ� �ʱ�ȭ
                }
                else                                  // �̹� ���⸦ ���� ���¶�� ���׷��̵�
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level]; // ���� ������ ���
                    nextCount += data.counts[level];                     // �߰� �߻� ��

                    weapon.LevelUp(nextDamage, nextCount);               // ������ ����
                }

                level++;                             // ���� ����
                break;

            case ItemData.ItemType.Glove:            // �尩 (���ݼӵ� ��)
            case ItemData.ItemType.Shoe:             // �Ź� (�̵��ӵ� ��)
                if (level == 0)
                {
                    GameObject newGear = new GameObject();             // �� ������Ʈ ����
                    gear = newGear.AddComponent<Gear>();               // Gear ������Ʈ �߰�
                    gear.Init(data);                                   // �����ͷ� �ʱ�ȭ
                }
                else
                {
                    float nextRate = data.damages[level];              // ���� �ܰ� ��ġ
                    gear.LevelUp(nextRate);                            // ������ ����
                }

                level++;                             // ���� ����
                break;

            case ItemData.ItemType.Heal:             // ȸ�� ������
                GameManager.instance.health = GameManager.instance.maxHealth; // ü�� ȸ��
                break;
        }

        if (level == data.damages.Length)            // �ִ� ������ �����ϸ�
        {
            GetComponent<Button>().interactable = false; // �� �̻� ���� �Ұ� (��Ȱ��ȭ)
        }
    }
}