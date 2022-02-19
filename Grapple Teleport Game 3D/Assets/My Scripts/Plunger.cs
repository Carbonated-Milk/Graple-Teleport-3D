using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plunger : MonoBehaviour
{
    bool caught;
    Rigidbody rb;

    [HideInInspector] public GrapplingHook hookScript;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!caught)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        caught = true;
        Vector3 parentPos = hookScript.gameObject.transform.position;
        float dist = Vector3.Distance(parentPos, transform.position);
        if (col.gameObject.CompareTag("Instant Death"))
        {
            Destroy(this.gameObject);
        }
        else if (dist > 1 && hookScript.activePlunger == this.gameObject)
        {
            hookScript.Attatched(col.contacts[0].point);
        }
        rb.isKinematic = true;
        transform.LookAt(col.contacts[0].normal);
    }
}
