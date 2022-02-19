using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour, IAction
{
    private Transform cam;
    public float ropeLength;
    public LineRenderer lineRen;

    public GameObject plunger;
    [HideInInspector] public GameObject activePlunger;
    public float throwSpeed;

    Rigidbody rb;
    Vector3 lockedPos;
    float shootLength;
    float grapleRadius;

    public static bool grapleCaught;

    private PlayerControls playerControls;

    public void Action(int state)
    {
        if(state == 0)
        {
            if(lockedPos == Vector3.zero)
            {
                StopAllCoroutines();
                ShootPlunger();
            }
            else
            {

                Player.physicsBased = false;
                Detatch();
            }
            
        }
        if (state == 1 && lockedPos != Vector3.zero)
        {
            Detatch();
        }


    }
    void Start()
    {
        //Physics.queriesStartInColliders = false;
        rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().transform;

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.Jump.performed += RetractHook;
    }

    public void RetractHook(InputAction.CallbackContext context)
    {
        if(context.performed && grapleCaught && lockedPos != Vector3.zero)
        {
            Detatch();
        }
    }
    void Update()
    {
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
            lineRen.SetPosition(1, transform.position + shootLength * cam.forward);
        }

        if(grapleCaught)
        {
            Debug.DrawLine(transform.position, lockedPos, Color.cyan);
        }

        Debug.DrawLine(transform.position, transform.position + shootLength * cam.forward);
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
    }

    public void ShootPlunger()
    {
        activePlunger = Instantiate(plunger) as GameObject;
        activePlunger.transform.position = cam.position + cam.forward * 2;
        activePlunger.GetComponent<Rigidbody>().velocity = cam.forward * throwSpeed;
        activePlunger.GetComponent<Plunger>().hookScript = this;
    }

    public void Attatched(Vector3 caughtSpot)
    {
        GetComponent<Player>().OffGround();
        lockedPos = caughtSpot;
        grapleCaught = true;
    }

    public void Detatch()
    {
        TurnOffGrapple();
        lockedPos = Vector3.zero;
    }

    /*IEnumerator ShootHook()
    {
        while (shootLength < ropeLength)
        {

            shootLength += 50 * Time.deltaTime;
            RaycastHit hit;
            Physics.Raycast(transform.position, cam.transform.forward, out hit, shootLength);
            if (hit.collider != null)
            {
                GetComponent<Player>().OffGround();
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
        Player.physicsBased = false;
        while (shootLength > 0f)
        {
            lockedPos = Vector3.zero;
            shootLength -= 50 * Time.deltaTime;
            yield return null;
        }
        shootLength = 0;
    }*/

    public void TurnOffGrapple()
    {
        StopAllCoroutines();
        shootLength = 0;
        grapleRadius = 0f;
        grapleCaught = false;
        lockedPos = Vector3.zero;
        Player.physicsBased = false;
    }

    public void SetGrapleRadius()
    {
        grapleRadius = (lockedPos - transform.position).magnitude;
    }
}
