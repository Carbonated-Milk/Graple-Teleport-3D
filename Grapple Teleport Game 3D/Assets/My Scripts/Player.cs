using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float JumpPow;
    public float speed;
    public float interpolationNum;

    public static float sensetivity = .5f;
    public static bool physicsBased = false;
    private Camera cam;
    private Rigidbody rb;
    private PlayerControls playerControls;

    public static GameObject playerObject;

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

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.Jump.performed += Jump;

        playerControls.Player.Shift.started += Sprint;
        playerControls.Player.Shift.canceled += Sprint;

        playerControls.Player.Interact.performed += Action;
        playerControls.Player.Interact.canceled += Action;

        playerControls.Player.Tab.performed += TabMenu;
    }

    private void Update()
    {
        Vector2 moveVector = playerControls.Player.Movement.ReadValue<Vector2>() * speed * Time.deltaTime * 100;
        if(physicsBased)
        {
            rb.AddForce(transform.forward * moveVector.y + transform.right * moveVector.x);
        }
        else
        {
            Vector3 xzVel = Vector3.right * rb.velocity.x + Vector3.forward * rb.velocity.z;
            Vector3 playerDir = transform.forward * moveVector.y + transform.right * moveVector.x;
            playerDir *= speed;
            if(xzVel.sqrMagnitude < playerDir.magnitude || Vector3.Dot(playerDir, xzVel) < 0)
            {
                rb.velocity = Vector3.Lerp(xzVel, playerDir, interpolationNum) + rb.velocity.y * Vector3.up;
            }
        }

        Vector2 mouseVector = playerControls.Player.Mouse.ReadValue<Vector2>() * sensetivity * Time.deltaTime * 100;
        transform.rotation *= Quaternion.Euler(Vector3.up * mouseVector.x);
        cam.transform.localRotation *= Quaternion.Euler(Vector3.right * -mouseVector.y);

        /*if(cam.transform.localRotation.eulerAngles.x > 89 && cam.transform.localRotation.eulerAngles.x < 150)
        {
            cam.transform.localRotation = Quaternion.Euler(Vector3.right * 89);
        }
        else if(cam.transform.localRotation.eulerAngles.x < 310 && cam.transform.localRotation.eulerAngles.x > 100)
        {
            cam.transform.localRotation = Quaternion.Euler(Vector3.right * -89);
        }
        Debug.Log(cam.transform.localRotation.eulerAngles.x);*/
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
}

public interface IAction
{
    void Action(int state);
}
