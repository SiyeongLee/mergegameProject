using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checker : MonoBehaviour
{
    [Tooltip("병합된 과일이 스폰될 Y 높이")]
    [SerializeField] private float mergeHeight = 1.0f;
    [Tooltip("그릇(판)의 중심 위치")]
    [SerializeField] private Transform bowlCenter;
    [Tooltip("그릇(판)의 반지름")]
    [SerializeField] private float bowlRadius = 2.0f;

    private static bool isMerging;
    private Collider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isMerging) return;
        if (!collision.gameObject.CompareTag("Fruit")) return;

        var otherFruit = collision.gameObject.GetComponent<Fruit>();
        var thisFruit = GetComponent<Fruit>();
        if (otherFruit == null || thisFruit == null) return;
        if (otherFruit.FruitType != thisFruit.FruitType) return;

        isMerging = true;
        myCollider.enabled = false;
        collision.collider.enabled = false;

        if (transform.position.y < collision.transform.position.y)
        {
            int nextType = thisFruit.FruitType + 1;

            // 1) 그릇 안 XZ 위치 계산 (평균 대신 두 과일 중간, 그릇 반지름 안으로 클램프)
            Vector3 mid = (transform.position + collision.transform.position) * 0.5f;
            Vector2 dir = new Vector2(mid.x - bowlCenter.position.x,
                                       mid.z - bowlCenter.position.z);
            if (dir.magnitude > bowlRadius)
                dir = dir.normalized * (bowlRadius * 0.9f);
            Vector3 spawnPos = new Vector3(
                bowlCenter.position.x + dir.x,
                mergeHeight,
                bowlCenter.position.z + dir.y
            );

            // 2) 스폰 (GameManager 내부에서 prefab 스케일 자동 적용)
            GameManager.Instance.SpawnFruit(nextType, spawnPos, transform.parent)
                       ?.GetComponent<Rigidbody>()?.Sleep();
            // Rigidbody.Sleep() 으로 튕기는 관성도 제거

        }

        Destroy(collision.gameObject);
        Destroy(gameObject);
        isMerging = false;
    }
}
