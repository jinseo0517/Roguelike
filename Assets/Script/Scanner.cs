using System.Collections;                          // Unity�� �⺻ �÷��� ��� ���
using System.Collections.Generic;                  // ���׸� �÷���(List ��) ���
using UnityEngine;                                 // Unity ���� ��� ���

public class Scanner : MonoBehaviour               // ��ó�� ���� �ڵ����� Ž���ϴ� ����� �ϴ� Ŭ����
{
    public float scanRange;                        // Ž�� ���� (������)
    public LayerMask targetLayer;                  // � ���̾��� ������Ʈ�� Ž������ ����
    public RaycastHit2D[] targets;                 // Ž���� ���� ����
    public Transform nearestTarget;                // ���� ����� ����� Transform

    void FixedUpdate()                             // ���� ���� �ֱ⸶�� ����
    {
        targets = Physics2D.CircleCastAll(
            transform.position,                    // ���� ��ġ�� ��������
            scanRange,                             // ������ ������
            Vector2.zero,                          // ���� ���� (�� �߽� Ž��)
            0,
            targetLayer                            // ������ ���̾ Ž��
        );

        nearestTarget = GetNearest();              // ���� ����� ��� ã��
    }

    Transform GetNearest()                         // ���� ����� ��� ���
    {
        Transform result = null;                   // ��� ����� ����
        float diff = 100;                          // ������� ���� ª�� �Ÿ� (�ʱⰪ ����� ũ�� ����)

        foreach (RaycastHit2D target in targets)   // Ž���� ��� ��� �߿���
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos); // ���� �Ÿ� ���

            if (curDiff < diff)                    // �� ����� ����̸�
            {
                diff = curDiff;                    // ���� ª�� �Ÿ� ����
                result = target.transform;         // ����� ����
            }
        }

        return result;                             // ���� ����� ��� ��ȯ
    }
}