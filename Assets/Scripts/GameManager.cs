using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Tooltip("FruitType 인덱스에 대응되는 프리팹")]
    [SerializeField] private GameObject[] fruitPrefabs;

    [Tooltip("각 프리팹의 기본 로컬 스케일")]
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

        // 유효성 검사
        if (fruitPrefabs == null || fruitPrefabs.Length == 0)
            Debug.LogWarning("fruitPrefabs 배열이 비어 있습니다.");
        if (fruitScales != null && fruitScales.Length < fruitPrefabs.Length)
            Debug.LogWarning("fruitScales 배열 길이가 fruitPrefabs보다 작습니다.");

        for (int i = 0; i < fruitPrefabs.Length; i++)
        {
            if (fruitPrefabs[i] == null)
                Debug.LogWarning($"fruitPrefabs[{i}]가 할당되지 않았습니다.");
        }
    }

    /// <summary>
    /// 지정된 타입의 과일을 생성하고, 해당 스케일을 적용합니다.
    /// </summary>
    public GameObject SpawnFruit(int type, Vector3 position, Transform parent = null)
    {
        if (type < 0 || type >= fruitPrefabs.Length)
        {
            Debug.LogError($"잘못된 FruitType: {type}");
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