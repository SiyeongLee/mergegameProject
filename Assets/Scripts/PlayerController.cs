using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    [Header("Fruit Drop")]
    public GameObject[] fruitPrefabs;
    public Transform spawnPoint;
    private GameObject currentFruit;
    private bool isHolding = false;

    [Header("Camera Follow")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 10f, -6f);
    public float cameraSmoothSpeed = 5f;

    [Header("Next Fruit Preview")]
    public Transform previewPosition;
    private GameObject nextPreviewFruit;
    private int nextFruitIndex;

    void Start()
    {
        // 첫 번째 과일 미리 선택
        nextFruitIndex = Random.Range(0, fruitPrefabs.Length);
        ShowNextFruitPreview();
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleFruitDrop();
    }

    void LateUpdate()
    {
        HandleCameraFollow();
    }

    // -------------------- Movement --------------------
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

    // -------------------- Fruit Drop --------------------
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
        int spawnIndex = nextFruitIndex;

        currentFruit = Instantiate(fruitPrefabs[spawnIndex], spawnPoint.position, Quaternion.identity);

        Rigidbody rb = currentFruit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        isHolding = true;
    }

    void DropFruit()
    {
        if (currentFruit == null) return;

        Rigidbody rb = currentFruit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        isHolding = false;
        currentFruit = null;

        // 낙하 직후 다음 과일 준비 & 프리뷰
        nextFruitIndex = Random.Range(0, fruitPrefabs.Length);
        ShowNextFruitPreview();
    }

    // -------------------- Camera --------------------
    void HandleCameraFollow()
    {
        if (cameraTransform == null) return;

        Vector3 desiredPos = transform.position + cameraOffset;
        Vector3 smoothed = Vector3.Lerp(cameraTransform.position, desiredPos, cameraSmoothSpeed * Time.deltaTime);
        cameraTransform.position = smoothed;

        cameraTransform.rotation = Quaternion.Euler(60f, 0f, 0f); // 탑다운 고정 각도
    }

    // -------------------- Preview --------------------
    void ShowNextFruitPreview()
    {
        if (fruitPrefabs == null || fruitPrefabs.Length == 0 || previewPosition == null)
        {
            Debug.LogWarning("Fruit preview skipped: missing prefab or preview position.");
            return;
        }

        if (nextPreviewFruit != null)
        {
            DestroyImmediate(nextPreviewFruit);
            nextPreviewFruit = null;
        }

        GameObject preview = Instantiate(fruitPrefabs[nextFruitIndex], previewPosition.position, Quaternion.identity);

        DestroyImmediate(preview.GetComponent<Rigidbody>());
        DestroyImmediate(preview.GetComponent<Collider>());

        preview.transform.localScale *= 0.7f;
        preview.transform.rotation = Quaternion.Euler(20f, 0f, 0f);
        preview.name = "[Preview] " + fruitPrefabs[nextFruitIndex].name;

        nextPreviewFruit = preview;
    }
}
