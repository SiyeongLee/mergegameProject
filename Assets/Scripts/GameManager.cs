using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] fruitPrefabs;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void MergeAfterDelay(GameObject fruitA, GameObject fruitB)
    {
        StartCoroutine(MergeCoroutine(fruitA, fruitB));
    }

    private IEnumerator MergeCoroutine(GameObject fruitA, GameObject fruitB)
    {
        yield return new WaitForSeconds(0.1f);

        if (fruitA == null || fruitB == null) yield break;

        Fruit f1 = fruitA.GetComponent<Fruit>();
        Fruit f2 = fruitB.GetComponent<Fruit>();
        if (f1 == null || f2 == null || f1.fruitIndex != f2.fruitIndex) yield break;

        int nextIndex = f1.fruitIndex + 1;
        if (nextIndex >= fruitPrefabs.Length) yield break;

        Vector3 spawnPos = (fruitA.transform.position + fruitB.transform.position) / 2f;

        // ✅ 병합 대상이 currentFruit이라면 제거
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null && (player.currentFruit == fruitA || player.currentFruit == fruitB))
        {
            player.currentFruit = null;
            player.hasDropped = true;
        }

        // ✅ 속도 제거
        Rigidbody rb1 = fruitA.GetComponent<Rigidbody>();
        Rigidbody rb2 = fruitB.GetComponent<Rigidbody>();
        if (rb1 != null) rb1.velocity = rb1.angularVelocity = Vector3.zero;
        if (rb2 != null) rb2.velocity = rb2.angularVelocity = Vector3.zero;

        Destroy(fruitA);
        Destroy(fruitB);

        yield return new WaitForSeconds(0.05f);

        GameObject newFruit = Instantiate(fruitPrefabs[nextIndex], spawnPos, Quaternion.identity);
        newFruit.tag = "Fruit";

        if (player != null)
            player.SpawnFruit();
    }
}