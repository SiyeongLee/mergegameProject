using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform[] fruitPoses;
    public Transform spawnPoint;
    public GameObject currentFruit;

    private bool isDropping = false;

    void Start()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint is not assigned in PlayerController.");
            enabled = false;
            return;
        }

        if (fruitPoses == null || fruitPoses.Length == 0)
        {
            Debug.LogError("Fruit poses not assigned or empty in PlayerController.");
            enabled = false;
            return;
        }

        SpawnFruit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentFruit != null && !isDropping)
        {
            DropFruit();
        }

        if (currentFruit != null && !isDropping)
        {
            currentFruit.transform.position = spawnPoint.position;
        }

        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (verticalInput != 0 && currentFruit != null)
        {
            currentFruit.transform.Rotate(Vector3.forward, -verticalInput * 90f * Time.deltaTime);
        }

        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && currentFruit != null)
        {
            float rotateDirection = Input.GetKey(KeyCode.LeftArrow) ? 1f : -1f;
            currentFruit.transform.Rotate(Vector3.forward, rotateDirection * 90f * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.A)) move = -1f;
        else if (Input.GetKey(KeyCode.D)) move = 1f;

        transform.Translate(Vector3.right * move * 5f * Time.deltaTime);
    }

    void DropFruit()
    {
        isDropping = true;

        Rigidbody rb = currentFruit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // 떨어질 수 있도록 중력 활성화
            rb.useGravity = true;
        }

        Fruit fruit = currentFruit.GetComponent<Fruit>();
        if (fruit != null)
        {
            fruit.isHeld = false;
            fruit.dropTime = Time.time;
        }

        currentFruit = null;
        Invoke("SpawnFruit", 0.4f);
    }

    public void SpawnFruit()
    {
        if (fruitPoses == null || fruitPoses.Length == 0 || spawnPoint == null)
        {
            Debug.LogError("Cannot spawn fruit: Check fruitPoses and spawnPoint assignments.");
            return;
        }

        int index = Random.Range(0, fruitPoses.Length);
        Transform prefabTransform = fruitPoses[index];

        if (prefabTransform == null || prefabTransform.gameObject == null)
        {
            Debug.LogError($"Fruit prefab at index {index} is null.");
            return;
        }

        currentFruit = Instantiate(prefabTransform.gameObject, spawnPoint.position, Quaternion.identity);

        Rigidbody2D rigid = currentFruit.GetComponent<Rigidbody2D>();
        if (rigid != null) rigid.simulated = false;

        Fruit fruit = currentFruit.GetComponent<Fruit>();
        if (fruit != null) fruit.isHeld = true;

        isDropping = false;
    }
}
