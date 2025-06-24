using System.Collections;                          // Unity�� �⺻ �÷��� ���
using System.Collections.Generic;                  // ���׸� �÷��� (List ��)
using UnityEngine;                                 // Unity ���� ��� ���

public class LevelUp : MonoBehaviour               // ������ UI�� �����ϴ� Ŭ����
{
    RectTransform rect;                            // �� ������Ʈ�� UI ��ġ/ũ�� ����
    Item[] items;                                  // ������ �� ������ ������ ��ϵ�

    void Awake()                                   // ��ũ��Ʈ Ȱ��ȭ �� �� �� �����
    {
        rect = GetComponent<RectTransform>();      // �� UI ������Ʈ�� RectTransform ����
        items = GetComponentsInChildren<Item>(true); // �ڽ� �� Item ��ũ��Ʈ�� ���� �͵� ��� �������� (��Ȱ�� ����)
    }

    public void Show()                             // ������ UI ǥ�� �Լ�
    {
        Next();                                     // ���� ������ 3�� ����
        rect.localScale = Vector3.one;              // UI�� ���̵��� ũ�� ����
        GameManager.instance.Stop();                // ���� �Ͻ�����
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Levelup); // ������ ȿ���� ���
        AudioManager.instance.EffectBgm(true);      // ����� ���̵�/���� �� ���
    }

    public void Hide()                             // ������ UI ����� �Լ�
    {
        rect.localScale = Vector3.zero;             // UI ��Ȱ��ȭ (ũ�� 0����)
        GameManager.instance.Resume();              // ���� �����
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select); // ���� ȿ���� ���
        AudioManager.instance.EffectBgm(false);     // ���� BGM ����
    }

    public void Select(int index)                  // ������ �������� �������� �� ȣ���
    {
        items[index].OnClik();                      // �ش� �������� Ŭ�� ���� ����
    }

    void Next()                                    // ���� ������ ������ �ĺ� 3�� ����
    {
        // 1. ��� ������ ��Ȱ��ȭ
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. ���߿��� �����ϰ� ���� �ٸ� 3���� ������ �ε��� �̱�
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;                              // 3���� ���� �ٸ��� ����
        }

        for (int index = 0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[index]];       // �������� ���õ� ������

            // 3. ���õ� �������� ������ ���, ��ü�� �Һ� ������ ������
            if (ranItem.level == ranItem.data.damages.Length) // ���� ������ �ִ� �����̸�
            {
                items[4].gameObject.SetActive(true); // ���� �Һ� ������ (ex: ȸ�� ��) Ȱ��ȭ
            }
            else
            {
                ranItem.gameObject.SetActive(true);  // �Ϲ� �������� �״�� ǥ��
            }
        }
    }
}