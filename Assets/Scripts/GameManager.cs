using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject[] fruitPrefabs; // 0: 사과, 1: 오렌지, 2: 포도, ...

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void MergeFruits(GameObject fruitA, GameObject fruitB)
    {
        Fruit fruitScript = fruitA.GetComponent<Fruit>();
        if (fruitScript == null) return;

        int nextIndex = fruitScript.fruitIndex + 1;
        if (nextIndex >= fruitPrefabs.Length) return;

        Vector3 spawnPos = (fruitA.transform.position + fruitB.transform.position) / 2f;

        Destroy(fruitA);
        Destroy(fruitB);

        GameObject newFruit = Instantiate(fruitPrefabs[nextIndex], spawnPos, Quaternion.identity);
        newFruit.tag = "Fruit"; // 태그 재지정 (필수)
    }
}