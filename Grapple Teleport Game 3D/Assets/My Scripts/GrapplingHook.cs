using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour, IAction
{
    private Camera cam;
    public float ropeLength;
    public LineRenderer lineRen;

    Rigidbody rb;
    Vector3 lockedPos;
    float shootLength;
    float grapleRadius;

    public static bool grapleCaught;

    public void Action(int state)
    {
        if(state == 0)
        {
            StopAllCoroutines();
            StartCoroutine(ShootHook());
        }
        if(state == 1)
        {
            StopAllCoroutines();
            StartCoroutine(RetractHook());
        }
        
    }
    void Start()
    {
        //Physics.queriesStartInColliders = false;
        rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    void Update()
    {
        /*if(Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            StartCoroutine(ShootHook());
        }
        if(Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();
            StartCoroutine(RetractHook());
        }*/
        

        if (Vector3.Dot(rb.velocity, lockedPos - transform.position) <= 0  && lockedPos != Vector3.zero)
        {
            if(grapleRadius == 0)
            {
                SetGrapleRadius();
            }
            GrappleVelocity(lockedPos - transform.position);
        }

        if(grapleCaught)
        {
            lineRen.SetPosition(0, transform.position);
            lineRen.SetPosition(1, lockedPos);
        }
        else
        {
            lineRen.SetPosition(0, transform.position);
            lineRen.SetPosition(1, transform.position + shootLength * cam.transform.forward);
        }

        if(grapleCaught)
        {
            Debug.DrawLine(transform.position, lockedPos, Color.cyan);
        }

        Debug.DrawLine(transform.position, transform.position + shootLength * cam.transform.forward);
    }

    public void GrappleVelocity(Vector3 grapleVector)
    {
        Vector3 AddVelocity = Vector3.zero;
        Vector3 grapleDirection = grapleVector.normalized;
        

        // Projection to make rope super tight and hold on to thing.
        //This code only works because grapleDirection is normalized.
        rb.velocity = rb.velocity - grapleDirection * Vector3.Dot(rb.velocity, grapleDirection);
        if((transform.position - lockedPos).sqrMagnitude > grapleRadius * grapleRadius)
        {
            transform.position = lockedPos + (transform.position - lockedPos).normalized * grapleRadius;
        }
        
        Debug.DrawLine(transform.position, transform.position + (Vector3)(grapleDirection * rb.velocity.sqrMagnitude / grapleRadius), Color.red);
        /*
        AddVelocity += grapleDirection * rb.velocity.sqrMagnitude / grapleRadius;
        
        //AddVelocity += Vector3.Dot(rb.velocity.normalized, Vector3.down) * Vector3.down * Physics.gravity;
        Debug.DrawLine(transform.position, transform.position + Vector3ify(Vector3.Dot(rb.velocity.normalized, Vector3.down) * Vector3.down * Physics.gravity), Color.green);
        */
        //rb.AddForce(AddVelocity);
    }

    IEnumerator ShootHook()
    {
        while (shootLength < ropeLength)
        {

            shootLength += 50 * Time.deltaTime;
            RaycastHit hit;
            Physics.Raycast(transform.position, cam.transform.forward, out hit, shootLength);
            if (hit.collider != null)
            {
                
                lockedPos = hit.point;
                grapleCaught = true;
            }
            
            yield return null;
        }
        if(!grapleCaught)
        {
            StartCoroutine(RetractHook());
        } 
    }
    IEnumerator RetractHook()
    {
        shootLength = (lockedPos - transform.position).magnitude;
        grapleRadius = 0f;
        grapleCaught = false;
        while (shootLength > 0f)
        {
            lockedPos = Vector3.zero;
            shootLength -= 50 * Time.deltaTime;
            yield return null;
        }
        shootLength = 0;
    }

    public void TurnOffGrapple()
    {
        StopAllCoroutines();
        shootLength = 0;
        grapleRadius = 0f;
        grapleCaught = false;
        lockedPos = Vector3.zero;
    }

    public void SetGrapleRadius()
    {
        grapleRadius = (lockedPos - transform.position).magnitude;
    }
}
