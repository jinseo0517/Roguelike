using System.Collections;                          // Unity���� �⺻ �÷��� ����� ����ϱ� ���� ���ӽ����̽�
using System.Collections.Generic;                  // List, Dictionary ���� ���׸� �÷����� ����ϱ� ���� ���ӽ����̽�
using UnityEngine;                                 // Unity�� �ٽ� ����� �ҷ����� ���ӽ����̽�

public class Character : MonoBehaviour             // Character Ŭ����: Unity ������Ʈ�� �ٴ� ��ũ��Ʈ
{
    public static float Speed                      // �̵� �ӵ�: playerId�� 0���� ��� 1.1��, �ƴϸ� �⺻ 1.0
    {
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
    }

    public static float WeaponSpeed                // ���� �ӵ�: playerId�� 1���� ��� 1.1��, �ƴϸ� 1.0
    {
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    }

    public static float WeaponRate                 // ���� ���� ����: playerId�� 1���̸� 0.9 (�� ������), �ƴϸ� 1.0
    {
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f; }
    }

    public static float Damage                     // ���ݷ�: playerId�� 2���� ��� 1.2��, �ƴϸ� 1.0
    {
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f; }
    }

    public static int Count                        // �߰� ����ü ��: playerId�� 3���̸� 1�� �߰�, �ƴϸ� 0
    {
        get { return GameManager.instance.playerId == 3 ? 1 : 0; }
    }
}