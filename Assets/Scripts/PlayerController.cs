
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("과일 관련")]
    public GameObject[] fruitPrefabs;
    public Transform spawnPoint;
    public float spawnDelay = 0.3f;

    [Header("플레이어 이동")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;

    [Header("카메라")]
    public Transform cameraTransform;
    public float cameraRotateSpeed = 50f;

    [HideInInspector] public GameObject currentFruit;
    [HideInInspector] public bool hasDropped = false;

    void Start()
    {
        SpawnFruit();
    }

    void Update()
    {
        HandleMovement();         // WASD 이동 + 회전
        HandleCameraRotation();   // 화살표로 카메라 회전

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropFruit();
        }

        // 과일이 낙하 전이면 spawnPoint 위치에 유지
        if (currentFruit != null && !hasDropped)
        {
            if (currentFruit.activeInHierarchy)
                currentFruit.transform.position = spawnPoint.position;
            else
                currentFruit = null;
        }
    }

    void HandleMovement()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) move += transform.forward;
        if (Input.GetKey(KeyCode.S)) move -= transform.forward;
        if (Input.GetKey(KeyCode.A)) move -= transform.right;
        if (Input.GetKey(KeyCode.D)) move += transform.right;

        transform.position += move * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q)) transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.E)) transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    void HandleCameraRotation()
    {
        if (cameraTransform == null) return;

        if (Input.GetKey(KeyCode.LeftArrow))
            cameraTransform.RotateAround(transform.position, Vector3.up, -cameraRotateSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.RightArrow))
            cameraTransform.RotateAround(transform.position, Vector3.up, cameraRotateSpeed * Time.deltaTime);
    }

    public void SpawnFruit()
    {
        int index = Random.Range(0, fruitPrefabs.Length);
        currentFruit = Instantiate(fruitPrefabs[index], spawnPoint.position, Quaternion.identity);
        currentFruit.tag = "Fruit";

        Rigidbody rb = currentFruit.GetComponent<Rigidbody>();
        if (rb == null) rb = currentFruit.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        Fruit fruitScript = currentFruit.GetComponent<Fruit>();
        if (fruitScript != null)
        {
            fruitScript.fruitIndex = index;
            fruitScript.isHeld = true;  // ✅ 과일 들고 있는 상태 표시
        }

        hasDropped = false;
    }

    public void DropFruit()
    {
        if (currentFruit == null) return;

        Rigidbody rb = currentFruit.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        Fruit fruitScript = currentFruit.GetComponent<Fruit>();
        if (fruitScript != null)
        {
            fruitScript.isHeld = false;             // ✅ 손에서 놓음
            fruitScript.dropTime = Time.time;       // ✅ 낙하 시간 기록
        }

        currentFruit = null;
        hasDropped = true;

        Invoke(nameof(SpawnFruit), spawnDelay);
    }
}