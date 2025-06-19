using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    [Header("Fruit Drop")]
    [SerializeField] private GameObject[] fruitPrefabs;

    public Transform spawnPoint;
    private GameObject currentFruit;
    private bool isHolding = false;
    private int nextFruitIndex;

    [Header("Camera Follow")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 9f, -5.5f);
    public float cameraSmoothSpeed = 5f;

    [Header("Timer")]
    public float gameTimer = 300f; // 5분
    private bool isGameOver = false;



    void Start()
    {
        nextFruitIndex = Random.Range(0, fruitPrefabs.Length);
    }

    void Update()
    {
        if (isGameOver) return;

        gameTimer -= Time.deltaTime;
        if (gameTimer <= 0f)
        {
            isGameOver = true;
            gameTimer = 0f;
            Debug.Log("5분 겨ㅇ과");
            return;
        }

        HandleMovement();
        HandleRotation();
        HandleFruitDrop();
        HandleChangeHeldFruit();
    }

    void LateUpdate()
    {
        if (isGameOver) return;
        HandleCameraFollow();
    }

    void HandleMovement()
    {
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W)) moveZ = 1f;
        if (Input.GetKey(KeyCode.S)) moveZ = -1f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        Vector3 move = (transform.forward * moveZ + transform.right * moveX).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    void HandleRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void HandleFruitDrop()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isHolding)
            {
                HoldFruit();
            }
            else
            {
                DropFruit();
            }
        }
    }

     void HoldFruit()
    {
        if (spawnPoint == null)
        {
            Debug.LogWarning("Spawn Point가 설정되지 않았습니다!");
            return;
        }


        currentFruit = Instantiate(fruitPrefabs[nextFruitIndex], spawnPoint.position, Quaternion.identity);
        currentFruit.transform.SetParent(this.transform, false);
        currentFruit.transform.localPosition = new Vector3(0f, -0.2f, 0.6f);
        currentFruit.transform.localRotation = Quaternion.identity;


        var rb = currentFruit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;

            rb.constraints = RigidbodyConstraints.None;
        }

        var col = currentFruit.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        isHolding = true;
    }

    void DropFruit()
    {
        if (currentFruit == null) return;
        currentFruit.transform.SetParent(null);
        currentFruit.transform.position = transform.position + new Vector3(0f, 1f, 0f);


        var rb = currentFruit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            rb.constraints = RigidbodyConstraints.FreezeRotationX
                           | RigidbodyConstraints.FreezeRotationZ;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        var col = currentFruit.GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        isHolding = false;
        currentFruit = null;

        nextFruitIndex = Random.Range(0, fruitPrefabs.Length);
    }


    void HandleChangeHeldFruit()
    {
        if (Input.GetKeyDown(KeyCode.C) && isHolding)
        {
            Destroy(currentFruit);
            currentFruit = null;

            int newIndex = nextFruitIndex;
            while (fruitPrefabs.Length > 1 && newIndex == nextFruitIndex)
            {
                newIndex = Random.Range(0, fruitPrefabs.Length);
            }

            nextFruitIndex = newIndex;
            HoldFruit();
        }
    }

    void HandleCameraFollow()
    {
        if (cameraTransform == null) return;

        Vector3 desiredPos = transform.position + cameraOffset;
        Vector3 smoothed = Vector3.Lerp(cameraTransform.position, desiredPos, cameraSmoothSpeed * Time.deltaTime);
        cameraTransform.position = smoothed;

        cameraTransform.rotation = Quaternion.Euler(60f, 0f, 0f);
    }
}
