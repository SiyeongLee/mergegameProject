using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;




public class PlayerController : MonoBehaviour
{
    public Transform spawnPoint;          // 과일이 생성될 위치
    public GameObject[] fruitPrefabs;     // 과일 프리팹 배열
    public float moveSpeed = 5f;

    private GameObject currentFruit;
    private bool isDropping = false;

    void Start()
    {
        SpawnFruit();
    }

    void Update()
    {
        if (currentFruit != null && !isDropping)
        {
            float move = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            currentFruit.transform.position += new Vector3(move, 0, 0);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DropFruit();
            }
        }
    }

    void SpawnFruit()
    {
        int randomIndex = Random.Range(0, 2); // 처음엔 0~1단계 과일만 나오게
        currentFruit = Instantiate(fruitPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
        currentFruit.tag = "Fruit"; // 태그 설정

        isDropping = false;
    }

    void DropFruit()
    {
        if (currentFruit == null) return;

        Rigidbody rb = currentFruit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        currentFruit = null;
        isDropping = true;

        Invoke(nameof(SpawnFruit), 0.8f); // 잠시 후 다음 과일 생성
    }
}