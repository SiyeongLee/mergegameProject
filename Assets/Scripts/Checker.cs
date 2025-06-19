using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checker : MonoBehaviour
{
    [Tooltip("���յ� ������ ������ Y ����")]
    [SerializeField] private float mergeHeight = 1.0f;
    [Tooltip("�׸�(��)�� �߽� ��ġ")]
    [SerializeField] private Transform bowlCenter;
    [Tooltip("�׸�(��)�� ������")]
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

            // 1) �׸� �� XZ ��ġ ��� (��� ��� �� ���� �߰�, �׸� ������ ������ Ŭ����)
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

            // 2) ���� (GameManager ���ο��� prefab ������ �ڵ� ����)
            GameManager.Instance.SpawnFruit(nextType, spawnPos, transform.parent)
                       ?.GetComponent<Rigidbody>()?.Sleep();
            // Rigidbody.Sleep() ���� ƨ��� ������ ����

        }

        Destroy(collision.gameObject);
        Destroy(gameObject);
        isMerging = false;
    }
}
