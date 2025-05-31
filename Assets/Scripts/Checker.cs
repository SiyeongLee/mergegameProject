using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    private static bool isMerging = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Fruit")) return;

        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();
        Fruit thisFruit = GetComponent<Fruit>();

        if (otherFruit != null && thisFruit != null && otherFruit.FruitType == thisFruit.FruitType)
        {
            // 충돌 후 잠시 병합 충돌체를 비활성화 (옵션)
            Collider thisCollider = GetComponent<Collider>();
            Collider otherCollider = collision.collider;
            thisCollider.enabled = false;
            otherCollider.enabled = false;

            if (transform.position.y < collision.transform.position.y)
            {
                int nextType = thisFruit.FruitType + 1;
                if (nextType >= GameManager.Instance.fruitPrefabs.Length)
                {
                    Debug.LogError("다음 Prefab이 존재하지 않음!");
                    return;
                }

                Vector3 spawnPos = (transform.position + collision.transform.position) / 2f;
                Instantiate(GameManager.Instance.fruitPrefabs[nextType], spawnPos, Quaternion.identity);
            }

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}