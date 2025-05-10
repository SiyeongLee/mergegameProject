using System.Collections;
using UnityEngine;

public class checksame : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 태그가 "Fruit"인 오브젝트끼리만 병합
        if (!collision.gameObject.CompareTag("Fruit") || !CompareTag("Fruit"))
            return;

        Fruit thisFruit = GetComponent<Fruit>();
        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

        if (thisFruit == null || otherFruit == null)
            return;

        // 이미 병합되었거나 병합 중이면 무시
        if (thisFruit.isMerged || otherFruit.isMerged || thisFruit.isMerging || otherFruit.isMerging)
            return;

        // 과일 단계가 다르면 병합 불가
        if (thisFruit.fruitIndex != otherFruit.fruitIndex)
            return;

        // 속도가 너무 빠르면 병합하지 않음 (충돌 직후 낙하 중인 상태 방지)
        Rigidbody rb1 = GetComponent<Rigidbody>();
        Rigidbody rb2 = collision.gameObject.GetComponent<Rigidbody>();

        if (rb1 == null || rb2 == null)
            return;
        // 양쪽 과일 모두 낙하 후 최소 0.2초 지난 상태여야 병합 허용
        if (Time.time - thisFruit.dropTime < 0.2f || Time.time - otherFruit.dropTime < 0.2f)
            return;
        // 병합 전에 들고 있는 과일은 제외
        if (thisFruit.isHeld || otherFruit.isHeld)
            return;

        // 병합 상태 지정 (중복 병합 방지용)
        thisFruit.isMerging = true;
        otherFruit.isMerging = true;
        thisFruit.isMerged = true;
        otherFruit.isMerged = true;


        // 병합 시작
        if (GameManager.instance != null)
        {
            GameManager.instance.MergeAfterDelay(gameObject, collision.gameObject);
        }
        
    }
}