using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [Tooltip("0=EGG, 1=STRAWBERRY, ... 등")]
    public int FruitType;

    [Tooltip("플레이어가 들고 있는 중인지 여부")]
    public bool isHeld = false;

    [Tooltip("떨어진 후 얼마 뒤 자동 제거할지")]
    public float dropTime = 5f;

    [HideInInspector]
    public Vector3 DefaultScale;

    private void Awake()
    {
        DefaultScale = transform.localScale;
    }
}
