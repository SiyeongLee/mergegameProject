using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checker : MonoBehaviour
{
    [SerializeField] private float mergeHeight = 1f;
    [SerializeField] private Transform bowlCenter;
    [SerializeField] private float bowlRadius = 2f;

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

        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();
        Fruit thisFruit = GetComponent<Fruit>();
        if (otherFruit == null || thisFruit == null) return;
        if (otherFruit.FruitType != thisFruit.FruitType) return;

        isMerging = true;
        myCollider.enabled = false;
        collision.collider.enabled = false;

        if (transform.position.y < collision.transform.position.y)
        {
            int nextType = thisFruit.FruitType + 1;

            
            if (nextType >= GameManager.Instance.FruitCount)   
            {
                Debug.Log("마지막 단계라 병합ㄴ");
                myCollider.enabled = true;
                collision.collider.enabled = true;
                isMerging = false;
                return;
            }

           
            Vector3 mid = (transform.position + collision.transform.position) * 0.5f;
            Vector2 dir = new Vector2(mid.x - bowlCenter.position.x, mid.z - bowlCenter.position.z);
            if (dir.magnitude > bowlRadius)
                dir = dir.normalized * (bowlRadius * 0.9f);
            Vector3 spawnPos = new Vector3(bowlCenter.position.x + dir.x, mergeHeight, bowlCenter.position.z + dir.y);

           
            GameManager.Instance.SpawnFruit(nextType, spawnPos, transform.parent);

           
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        isMerging = false;
    }
}
