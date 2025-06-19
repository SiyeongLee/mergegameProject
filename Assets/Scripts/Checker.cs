using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    private static bool isMerging = false;

    private void OnCollisionEnter(Collision collision)
    {
        // 1) 중복 병합 방지
        if (isMerging) return;
        if (!collision.gameObject.CompareTag("Fruit")) return;

        // 2) Fruit 타입 체크
        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();
        Fruit thisFruit = GetComponent<Fruit>();
        if (otherFruit == null || thisFruit == null) return;
        if (otherFruit.FruitType != thisFruit.FruitType) return;

        isMerging = true;

        // 3) 충돌체 비활성화
        Collider colThis = GetComponent<Collider>();
        Collider colOther = collision.collider;
        colThis.enabled = false;
        colOther.enabled = false;

        // 4) y 위치 낮은 쪽이 합병 처리
        if (transform.position.y < collision.transform.position.y)
        {
            int nextType = thisFruit.FruitType + 1;
            if (nextType < GameManager.Instance.fruitPrefabs.Length)
            {
                Vector3 spawnPos = (transform.position + collision.transform.position) * 0.5f;
                GameObject prefab = GameManager.Instance.fruitPrefabs[nextType];

                // ★ 씬 루트에, 부모 없이 생성 ★
                GameObject newFruit = Instantiate(prefab, spawnPos, Quaternion.identity);

                // ★ Prefab 에셋에 저장된 기본 localScale 그대로 적용 ★
                newFruit.transform.localScale = prefab.transform.localScale;
            }
            else
            {
                Debug.LogError("다음 타입의 Fruit Prefab이 없습니다!");
            }
        }

        // 5) 원본 두 과일 제거
        Destroy(collision.gameObject);
        Destroy(gameObject);

        isMerging = false;
    }
}
