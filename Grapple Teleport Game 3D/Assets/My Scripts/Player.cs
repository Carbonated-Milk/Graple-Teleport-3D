using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float JumpPow;
    public float speed;

    public static float sensetivity = .5f;
    private Camera cam;
    private Rigidbody rb;
    private PlayerControls playerControls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = transform.GetComponentInChildren<Camera>();

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.Jump.performed += Jump;

        playerControls.Player.Movement.performed += Movement;

        playerControls.Player.Shift.started += Sprint;
        playerControls.Player.Shift.canceled += Sprint;

        playerControls.Player.Interact.performed += Action;
        playerControls.Player.Interact.canceled += Action;

        playerControls.Player.Tab.performed += TabMenu;
    }

    private void Update()
    {
        Vector2 moveVector = playerControls.Player.Movement.ReadValue<Vector2>() * speed * Time.deltaTime * 100;
        rb.AddForce(transform.forward * moveVector.y + transform.right * moveVector.x);


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

    public void Movement(InputAction.CallbackContext context)
    {
        Vector2 moveVector = context.ReadValue<Vector2>() * speed;
        Vector2 xzVel = Vector2.right * rb.velocity.x + Vector2.up * rb.velocity.z;
        //rb.velocity = rb.velocity.
        rb.velocity = (Vector3)Vector2.Lerp(xzVel.normalized, (Vector2)transform.forward * moveVector.y + (Vector2)transform.right * moveVector.x, .5f) * xzVel.magnitude + Vector3.up * rb.velocity.y;
        //rb.AddForce(transform.forward * moveVector.y + transform.right * moveVector.x);
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
}

public interface IAction
{
    void Action(int state);
}
