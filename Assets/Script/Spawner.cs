using System.Collections;                          // Unity�� �⺻ �÷��� ��� ���
using System.Collections.Generic;                  // ���׸� �÷���(List, Dictionary ��) ���
using UnityEngine;                                 // Unity ���� ��� ���

public class Spawner : MonoBehaviour               // Spawner Ŭ����: ���� �ֱ������� �����ϴ� ����
{
    public Transform[] spawnPoint;                 // ���� ������ �� �ִ� ��ġ�� (���� ��ġ �� ����)
    public SpawnData[] spawnData;                  // �ð��� ���� �� ���� ����(�ӵ�, ü�� ��)
    public float levelTime;                        // ���� ��ȯ ���� �ð�

    int level;                                     // ���� ���� �ε��� (������ ����ʿ� ���� ����)
    float timer;                                   // ��� �ð� ����� ����

    void Awake()                                   // ��ũ��Ʈ�� ���۵� �� ����Ǵ� �ʱ�ȭ �Լ�
    {
        spawnPoint = GetComponentsInChildren<Transform>(); // �� ������Ʈ�� �ڽĵ鿡�� ��ġ ��ǥ���� ������
        levelTime = GameManager.instance.maxGameTime / spawnData.Length; // ��ü ���ӽð��� ���� ���� ������ �ܰ躰 �ð� ����
    }

    void Update()                                  // �� ������ ����Ǵ� �Լ� (�� ���� Ÿ�̹� üũ)
    {
        if (!GameManager.instance.isLive)          // ������ ���� ���� �ƴ϶�� �ƹ��͵� �� ��
            return;

        timer += Time.deltaTime;                   // ������ �� ��� �ð� ����
        level = Mathf.Min(                         // ���� ��� �ð� �������� ���� �ε��� ���
            Mathf.FloorToInt(GameManager.instance.gameTime / levelTime),
            spawnData.Length - 1);                 // ������ ������ ���� �ʵ��� ����

        if (timer > spawnData[level].spawnTime)    // ���� ���� �������� ������ �� ���� �ֱ� �ʰ� ��
        {
            timer = 0;                             // Ÿ�̸� �ʱ�ȭ
            Spawn();                               // �� ���� ����
        }
    }

    void Spawn()                                   // ���� �ϳ� �����ϴ� �Լ�
    {
        GameObject enemy = GameManager.instance.pool.Get(0); // ������Ʈ Ǯ���� Enemy Ÿ�� ������Ʈ ����
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        // ���� ��ġ �� ���� ���� (0���� Spawner �ڱ� �ڽ��̴� ����)
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
        // ���� ������ �´� ���� ����(�ӵ�, ü�� ��) ����
    }
}

[System.Serializable]                             // Unity �����Ϳ��� ������ ����ǵ��� ����� ���� ������̼�
public class SpawnData                             // �� ���� �� �ʿ��� �������� ���� ������ Ŭ����
{
    public float spawnTime;                        // ���� �󸶳� ���� �������� �����ϴ� �ð� (�� ����)
    public int spriteType;                         // �� ����(��������Ʈ ����)�� �����ϱ� ���� ��ȣ
    public int health;                             // ���� ü��
    public float speed;                            // ���� �̵� �ӵ�
}