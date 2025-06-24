using System.Collections;                          // Unity�� �⺻ �÷��� ��� ���
using System.Collections.Generic;                  // ���׸� �÷���(List ��) ���
using UnityEngine;                                 // Unity ���� ��� ���

public class Gear : MonoBehaviour                  // ���(Gear)�� ������ �����ϴ� Ŭ����
{
    public ItemData.ItemType type;                 // ��� ���� (�尩/�Ź� ��)
    public float rate;                             // �ɷ�ġ�� ������ ����(�ӵ� ��� ���� ��)

    public void Init(ItemData data)                // ��� ���� �������� �� �ʱ� ����
    {
        //Basic Set
        name = "Gear " + data.itemId;              // ������Ʈ �̸� ����
        transform.parent = GameManager.instance.player.transform; // �÷��̾ ���̱�
        transform.localPosition = Vector3.zero;    // ��ġ �ʱ�ȭ

        //Property Set
        type = data.itemType;                      // ��� Ÿ�� ����
        rate = data.damages[0];                    // ù ���� ���� �ɷ�ġ ���� ����
        ApplyGear();                               // ��� ȿ�� ����
    }

    public void LevelUp(float rate)                // ��� ���׷��̵�� �� ȣ��
    {
        this.rate = rate;                          // �� ���� ����
        ApplyGear();                               // �ٽ� ȿ�� ����
    }

    void ApplyGear()                               // ��� ������ ���� ȿ�� ���� ��� �б�
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:          // �尩�̸� ���� �߻� �ӵ� ����
                RateUp();
                break;
            case ItemData.ItemType.Shoe:           // �Ź��̸� �̵� �ӵ� ���
                SpeedUp();
                break;
        }
    }

    void RateUp()                                  // ���� �ӵ� ���� (���� �� ����)
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>(); // �÷��̾��� ��� ���� ��������

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:                             // ȸ�� ���� (id 0���� ���)
                    float speed = 150 * Character.WeaponSpeed;       // �⺻ �ӵ� ���
                    weapon.speed = speed + (speed * rate);           // �ӵ��� ������ŭ ����
                    break;
                default:                            // �߻��� ����
                    speed = 0.5f * Character.WeaponRate;             // �⺻ �߻� ����
                    weapon.speed = speed * (1f - rate);              // �߻� ������ ���̱� (�� ������)
                    break;
            }
        }
    }

    void SpeedUp()                                 // �̵� �ӵ� ���� ó��
    {
        float speed = 3 * Character.Speed;          // �⺻ �̵� �ӵ� ���
        GameManager.instance.player.speed = speed + speed * rate; // ������ŭ ����
    }
}