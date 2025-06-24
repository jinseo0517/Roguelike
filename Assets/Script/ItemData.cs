using System.Collections;                          // Unity�� �⺻ �÷��� ��� ���
using System.Collections.Generic;                  // List, Dictionary �� ���׸� �÷��� ���
using UnityEngine;                                 // Unity ������ �ٽ� ��� ���

[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")] // ������Ʈ ������ ScriptableObject ���� �޴� ���
public class ItemData : ScriptableObject           // �������� �����͸� ��� ���� ��ũ���ͺ� ������Ʈ
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal } // �������� ���� (����/���/�Һ� ��)

    [Header("# Main Info")]                         // �����Ϳ��� ���м� ǥ��
    public ItemType itemType;                       // ������ ����
    public int itemId;                              // ������ ���� ��ȣ
    public string itemName;                         // ������ �̸�
    [TextArea]
    public string itemDesc;                         // ������ ���� (UI�� ǥ�õ� �ؽ�Ʈ)
    public Sprite itemIcon;                         // ������ ������ �̹���

    [Header("# Level Data")]                        // ������ �ɷ�ġ ������
    public float baseDamage;                        // �⺻ ������
    public int baseCount;                           // �⺻ �߻� ���� (����)
    public float[] damages;                         // ������ �߰� ������ ����
    public int[] counts;                            // ������ �߰� �߻� ��

    [Header("# Weapon")]                            // ���� ������ ��� �߰� ����
    public GameObject projectile;                   // �߻��� �Ѿ� prefab
    public Sprite hand;                             // ĳ���� �տ� ǥ�õ� ���� �׷���
}