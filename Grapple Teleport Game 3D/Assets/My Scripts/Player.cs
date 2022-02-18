using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float JumpPow;
    public float speed;
    public float physicsSpeed;
    public float interpolationNum;

    private CharacterController moveControl;

    private bool onGround = true;

    public static float sensetivity = .5f;
    public static bool physicsBased = false;
    private Camera cam;
    private Rigidbody rb;
    private PlayerControls playerControls;

    public static GameObject playerObject;

    float xRotation = 0;

    private Transform groundCheck;
    public LayerMask groundMask;

    private void Awake()
    {
        
        if (playerObject == null)
        {
            playerObject = gameObject;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        rb = GetComponent<Rigidbody>();
        cam = transform.GetComponentInChildren<Camera>();
        moveControl = GetComponent<CharacterController>();

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.Jump.performed += Jump;

        playerControls.Player.Shift.started += Sprint;
        playerControls.Player.Shift.canceled += Sprint;

        playerControls.Player.Interact.performed += Action;
        playerControls.Player.Interact.canceled += Action;

        playerControls.Player.Tab.performed += TabMenu;

        groundCheck = transform.Find("Ground Check");
    }

    private void Update()
    {
        onGround = Physics.CheckSphere(groundCheck.position, .4f, groundMask);
        if(onGround && !physicsBased)
        {
            OnGround();
        }

        Vector2 moveVector = playerControls.Player.Movement.ReadValue<Vector2>() * Time.deltaTime * 100;
        if(!onGround)
        {
            rb.AddForce((transform.forward * moveVector.y + transform.right * moveVector.x) * physicsSpeed);
        }

        Vector2 mouseVector = playerControls.Player.Mouse.ReadValue<Vector2>() * sensetivity * Time.deltaTime * 100;
        xRotation += mouseVector.y;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        transform.Rotate(Vector3.up * mouseVector.x);
        cam.transform.localRotation = Quaternion.Euler(Vector3.right * -xRotation);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(Vector3.up * JumpPow);
        }

    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            speed *= 1.5f;
        }
        else if (context.canceled)
        {
            speed /= 1.5f;
        }
    }
    public void TabMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            StartMenu.instance.transform.Find("Game Options").gameObject.SetActive(true);
            StartMenu.instance.transform.Find("PlayerUI").gameObject.SetActive(false);
            Time.timeScale = 0;
        }

    }
    public void Action(InputAction.CallbackContext context)
    {
        IAction action = GetComponent<IAction>();
        if (context.performed)
        {
            action.Action(0);
        }
        else if (context.canceled)
        {
            action.Action(1);
        }
    }

    

    public static void ChangeSensitivity(float newSensetivity)
    {
        Player.sensetivity = newSensetivity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Instant Death"))
        {
            GetComponent<Lives>().Die();
        }
    }

    /*private void OnCollisionEnter(Collision other)
    {
        OnGround();
    }
    private void OnCollisionStay(Collision other)
    {
        OnGround();
    }
    private void OnCollisionExit(Collision other)
    {
        //OffGround();
    }*/

    public void OnGround()
    {
        onGround = true;
        GetComponent<ControllerMove>().enabled = true;
        moveControl.enabled = true;
        rb.isKinematic = true;
    }

    public void OffGround()
    {
        onGround = false;
        GetComponent<ControllerMove>().enabled = false;
        rb.isKinematic = false;
        //rb.velocity = GetComponent<CharacterController>().velocity;
        moveControl.enabled =false;
        
    }
}

public interface IAction
{
    void Action(int state);
}
