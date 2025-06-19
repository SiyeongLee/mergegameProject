using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    private static bool isMerging = false;

    private void OnCollisionEnter(Collision collision)
    {
        // 1) �ߺ� ���� ����
        if (isMerging) return;
        if (!collision.gameObject.CompareTag("Fruit")) return;

        // 2) Fruit Ÿ�� üũ
        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();
        Fruit thisFruit = GetComponent<Fruit>();
        if (otherFruit == null || thisFruit == null) return;
        if (otherFruit.FruitType != thisFruit.FruitType) return;

        isMerging = true;

        // 3) �浹ü ��Ȱ��ȭ
        Collider colThis = GetComponent<Collider>();
        Collider colOther = collision.collider;
        colThis.enabled = false;
        colOther.enabled = false;

        // 4) y ��ġ ���� ���� �պ� ó��
        if (transform.position.y < collision.transform.position.y)
        {
            int nextType = thisFruit.FruitType + 1;
            if (nextType < GameManager.Instance.fruitPrefabs.Length)
            {
                Vector3 spawnPos = (transform.position + collision.transform.position) * 0.5f;
                GameObject prefab = GameManager.Instance.fruitPrefabs[nextType];

                // �� �� ��Ʈ��, �θ� ���� ���� ��
                GameObject newFruit = Instantiate(prefab, spawnPos, Quaternion.identity);

                // �� Prefab ���¿� ����� �⺻ localScale �״�� ���� ��
                newFruit.transform.localScale = prefab.transform.localScale;
            }
            else
            {
                Debug.LogError("���� Ÿ���� Fruit Prefab�� �����ϴ�!");
            }
        }

        // 5) ���� �� ���� ����
        Destroy(collision.gameObject);
        Destroy(gameObject);

        isMerging = false;
    }
}
