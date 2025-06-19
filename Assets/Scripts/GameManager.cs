using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Tooltip("FruitType �ε����� �����Ǵ� ������")]
    [SerializeField] private GameObject[] fruitPrefabs;

    [Tooltip("�� �������� �⺻ ���� ������")]
    [SerializeField] private Vector3[] fruitScales;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // ��ȿ�� �˻�
        if (fruitPrefabs == null || fruitPrefabs.Length == 0)
            Debug.LogWarning("fruitPrefabs �迭�� ��� �ֽ��ϴ�.");
        if (fruitScales != null && fruitScales.Length < fruitPrefabs.Length)
            Debug.LogWarning("fruitScales �迭 ���̰� fruitPrefabs���� �۽��ϴ�.");

        for (int i = 0; i < fruitPrefabs.Length; i++)
        {
            if (fruitPrefabs[i] == null)
                Debug.LogWarning($"fruitPrefabs[{i}]�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    /// <summary>
    /// ������ Ÿ���� ������ �����ϰ�, �ش� �������� �����մϴ�.
    /// </summary>
    public GameObject SpawnFruit(int type, Vector3 position, Transform parent = null)
    {
        if (type < 0 || type >= fruitPrefabs.Length)
        {
            Debug.LogError($"�߸��� FruitType: {type}");
            return null;
        }

        GameObject prefab = fruitPrefabs[type];
        Vector3 scale = prefab.transform.localScale;
        if (fruitScales != null && type < fruitScales.Length)
        {
            scale = fruitScales[type];
        }

        GameObject go = Instantiate(prefab, position, Quaternion.identity, parent);
        go.transform.localScale = scale;
        return go;
    }
}