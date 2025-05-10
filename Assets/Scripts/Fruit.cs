using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int fruitIndex;
    public bool isMerging = false;
    public bool isMerged = false;

    public bool isHeld = false;     // 플레이어가 들고 있는 상태
    public float dropTime = 0f;     // 낙하 시간
}