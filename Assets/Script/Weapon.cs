using System.Collections;                          // Unity�� �⺻ �÷��� ��� ���
using System.Collections.Generic;                  // List, Dictionary �� ���׸� �÷��� ���
using UnityEngine;                                 // Unity ������ �ٽ� ��� ���

public class Weapon : MonoBehaviour                // ���� ������ �����ϴ� Ŭ����
{
    public int id;                                 // ���� ���� ID
    public int prefabId;                           // �Ѿ� prefab ��ȣ
    public float damage;                           // ������ ���ݷ�
    public int count;                              // �߻�Ǵ� �Ѿ� ���� (�Ǵ� ȸ���� ���� ��)
    public float speed;                            // ȸ�� �ӵ� �Ǵ� �߻� ����

    float timer;                                   // �߻� Ÿ�̸�
    Player player;                                 // �÷��̾� ������ ����

    void Awake()                                   // ���� �� �� �� ����: ���� ����
    {
        player = GameManager.instance.player;      // GameManager�� ���� Player ���� �ޱ�
    }

    void Update()                                  // �� ������ ����
    {
        if (!GameManager.instance.isLive)          // ���� ���� �ƴϸ� ���� ����
            return;

        switch (id)                                // ���� ������ ���� ���� �ٸ��� ó��
        {
            case 0:                                // ȸ���� ������ ���
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // ȸ�� ��Ŵ
                break;
            default:                               // �߻��� ������ ���
                timer += Time.deltaTime;

                if (timer > speed)                 // �߻� �ӵ���ŭ �ð� �����ٸ�
                {
                    timer = 0f;
                    Fire();                        // �Ѿ� �߻�
                }
                break;
        }

        // .. �׽�Ʈ�� �ڵ� (�����̽��� ������ ������ ��Ŵ)
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);                        // ���ݷ� 10, �߻� �� 1 ����
        }
    }

    public void LevelUp(float damage, int count)   // ���� ���׷��̵� (������, �߻� �� ����)
    {
        this.damage = damage * Character.Damage;   // ĳ���� ���ݷ� ���� �ݿ�
        this.count += count;                       // �߻� �� ����

        if (id == 0)
            Batch();                               // ȸ���� ������ ��� ���ġ �ʿ�

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        // �÷��̾� �ʿ� ��� ���� �˸���
    }

    public void Init(ItemData data)                // ���� �ʱ�ȭ �Լ� (���� ���� �� ȣ��)
    {
        //Basic Set
        name = "Weapon " + data.itemId;            // ������Ʈ �̸� ����
        transform.parent = player.transform;       // �÷��̾ ���̱�
        transform.localPosition = Vector3.zero;    // ��ġ �ʱ�ȭ

        //Property Set
        id = data.itemId;                          // ���� ID ����
        damage = data.baseDamage * Character.Damage; // ĳ���� ���ݷ� �ݿ�
        count = data.baseCount + Character.Count;  // �߻� �� ĳ���� ���� �ݿ�

        // �Ѿ� prefabId ã��
        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        // �ӵ� ���� �� ȸ�� ���� �ʱ� ����
        switch (id)
        {
            case 0:                                // ȸ���� ����
                speed = 150 * Character.WeaponSpeed;
                Batch();                           // �Ѿ� ���� �� ��ġ
                break;
            default:                               // �߻��� ����
                speed = 0.4f * Character.WeaponRate; // �߻� ���� ���
                break;
        }

        // Hand(�� �׷���) ����
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;           // �տ� ������ ������ ��������Ʈ ����
        hand.gameObject.SetActive(true);           // �� Ȱ��ȭ

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // ��� ���� �˸���
    }

    void Batch()                                   // ȸ���� ������ �Ѿ��� ��ġ�ϴ� �Լ�
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            if (index < transform.childCount)      // ���� �Ѿ��� ������ ��Ȱ��
            {
                bullet = transform.GetChild(index);
            }
            else                                   // ������ Ǯ���� ���� ������
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;         // ���� ������Ʈ�� ���̱�
            }

            bullet.localPosition = Vector3.zero;   // ��ġ �ʱ�ȭ
            bullet.localRotation = Quaternion.identity; // ȸ�� �ʱ�ȭ

            Vector3 rotVec = Vector3.forward * 360 * index / count; // ȸ�� ���� ���
            bullet.Rotate(rotVec);                // ���� ȸ��
            bullet.Translate(bullet.up * 1.5f, Space.World); // ��ġ �̵�
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
            // �Ѿ� �ʱ�ȭ (-100 = ���� ����)
        }
    }

    void Fire()                                    // �߻��� ���⿡�� �Ѿ��� �߻��ϴ� �Լ�
    {
        if (!player.scanner.nearestTarget)         // ��ó�� ���� ������ �߻� �� ��
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position; // ��ǥ ��ġ
        Vector3 dir = targetPos - transform.position;              // ���� ���
        dir = dir.normalized;                                      // ����ȭ

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;       // �Ѿ� ��ġ ����
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); // �������� ȸ�� ����
        bullet.GetComponent<Bullet>().Init(damage, count, dir);   // �Ѿ� ���� �� �߻�

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range); // �߻� ���� ���
    }
}