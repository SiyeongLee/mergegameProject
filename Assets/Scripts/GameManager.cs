using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] fruitPrefabs; // ¹è¿­ ÀÎµ¦½º = FruitType

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnMergedFruit(int fruitType, Vector3 position)
    {
        if (fruitType < fruitPrefabs.Length)
        {
            Instantiate(fruitPrefabs[fruitType], position, Quaternion.identity);
        }
    }
}