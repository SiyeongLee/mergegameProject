using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public GameObject[] fruit;
    public GameObject hold_fruit;
    private bool hold = false;
    private Transform holdtra;
    

    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
        }
        if (!hold) Invoke("MakeFuit", 0.5f);
        
        else
        {
            holdtra.position = transform.position + Vector3.down;
        }
        if(Input.GetKeyDown(KeyCode.Space) && hold)
        {
            holdrbody = hold_fruit.GetComponent<Rigidbody>();
            holdrbody.
        }
        void MakeFruit()
        {
            GameObject newfruit = Instantiate(fruit[Random.Range(0, 3)], transform.position, transform.rotation);


            hold_fruit = newfruit;
          
            holdtra = hold_fruit.GetComponent<Transform>();
        }
    }
}