using UnityEngine;
using System.Collections;

public class checksame : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Fruit")) return;

        Fruit myFruit = GetComponent<Fruit>();
        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

        if (myFruit == null || otherFruit == null) return;
        if (myFruit.fruitIndex != otherFruit.fruitIndex) return;

        if (myFruit.isMerging || otherFruit.isMerging) return;

        myFruit.isMerging = true;
        otherFruit.isMerging = true;

        StartCoroutine(DelayedMerge(myFruit, otherFruit));
    }

    IEnumerator DelayedMerge(Fruit fruitA, Fruit fruitB)
    {
        yield return new WaitForSeconds(0.05f); // 딜레이로 중복 병합 방지
        GameManager.instance.MergeFruits(fruitA.gameObject, fruitB.gameObject);
    }
}