using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int fruitIndex;
    public bool isMerging = false;
    public bool isMerged = false;

    public bool isHeld = false;     // �÷��̾ ��� �ִ� ����
    public float dropTime = 0f;     // ���� �ð�
}