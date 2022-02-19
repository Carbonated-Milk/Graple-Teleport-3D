using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    public int hurtAmount;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 7);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }
    
    void OnCollisionEnter(Collision hit)
    {
        if(hit.gameObject.GetComponent<IDamagable>() != null)
        {
            hit.gameObject.GetComponent<IDamagable>().Damaged(hurtAmount);
        }
        Destroy(gameObject);
    }
}
