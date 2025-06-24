using System.Collections;                          // Unity �⺻ �÷��� ���
using System.Collections.Generic;                  // ���׸� �÷��� ���
using UnityEngine;                                 // Unity�� �ٽ� ��� ���

public class Reposition : MonoBehaviour            // �� �Ǵ� ���� ���ġ�ϴ� ��ũ��Ʈ
{
    Collider2D coll;                               // �ڽ��� Collider2D�� ������ ����

    void Awake()
    {
        coll = GetComponent<Collider2D>();         // �ڽ��� Collider2D ������Ʈ�� ������
    }

    private void OnTriggerExit2D(Collider2D collision) // �浹�� ������Ʈ�� �ڽ��� ������ ��� �� ����
    {
        if (!collision.CompareTag("Area"))         // "Area"�� �ƴ� ��� ���� (��� ������ �������� Ȯ�ο�)
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position; // �÷��̾� ��ġ
        Vector3 myPos = transform.position;        // ���� ������Ʈ ��ġ

        switch (transform.tag)                     // Ground(��) �Ǵ� Enemy(��) �±׿� ���� �ٸ��� ó��
        {
            case "Ground":                         // �� Ÿ���� ��� ��ġ ���ġ
                float diffX = playerPos.x - myPos.x; // X�� �Ÿ� ����
                float diffY = playerPos.y - myPos.y; // Y�� �Ÿ� ����
                float dirX = diffX < 0 ? -1 : 1;   // X�� ���� (-1�̸� ����, 1�̸� ������)
                float dirY = diffY < 0 ? -1 : 1;   // Y�� ����

                diffX = Mathf.Abs(diffX);         // �Ÿ� ���� ���밪 ���
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY)                // X�� ���̰� �� Ŭ ��� �� �¿�� �̵�
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)           // Y�� ���̰� �� Ŭ ��� �� ���Ʒ��� �̵�
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enemy":                          // ���� ���
                if (coll.enabled)                  // Collider�� Ȱ��ȭ�Ǿ� ���� ���� �̵�
                {
                    Vector3 dist = playerPos - myPos; // �÷��̾�� ���� ��ġ ���� (��Ÿ: = �� - �� �Ǿ�� �� ��)
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0); // ���� ��ǥ �߰�
                    transform.Translate(ran + dist * 2); // �Ÿ� �������� �̵� + ���� �̵�
                }
                break;
        }
    }
}