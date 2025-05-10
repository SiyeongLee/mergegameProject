using System.Collections;
using UnityEngine;

public class checksame : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // �±װ� "Fruit"�� ������Ʈ������ ����
        if (!collision.gameObject.CompareTag("Fruit") || !CompareTag("Fruit"))
            return;

        Fruit thisFruit = GetComponent<Fruit>();
        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

        if (thisFruit == null || otherFruit == null)
            return;

        // �̹� ���յǾ��ų� ���� ���̸� ����
        if (thisFruit.isMerged || otherFruit.isMerged || thisFruit.isMerging || otherFruit.isMerging)
            return;

        // ���� �ܰ谡 �ٸ��� ���� �Ұ�
        if (thisFruit.fruitIndex != otherFruit.fruitIndex)
            return;

        // �ӵ��� �ʹ� ������ �������� ���� (�浹 ���� ���� ���� ���� ����)
        Rigidbody rb1 = GetComponent<Rigidbody>();
        Rigidbody rb2 = collision.gameObject.GetComponent<Rigidbody>();

        if (rb1 == null || rb2 == null)
            return;
        // ���� ���� ��� ���� �� �ּ� 0.2�� ���� ���¿��� ���� ���
        if (Time.time - thisFruit.dropTime < 0.2f || Time.time - otherFruit.dropTime < 0.2f)
            return;
        // ���� ���� ��� �ִ� ������ ����
        if (thisFruit.isHeld || otherFruit.isHeld)
            return;

        // ���� ���� ���� (�ߺ� ���� ������)
        thisFruit.isMerging = true;
        otherFruit.isMerging = true;
        thisFruit.isMerged = true;
        otherFruit.isMerged = true;


        // ���� ����
        if (GameManager.instance != null)
        {
            GameManager.instance.MergeAfterDelay(gameObject, collision.gameObject);
        }
        
    }
}