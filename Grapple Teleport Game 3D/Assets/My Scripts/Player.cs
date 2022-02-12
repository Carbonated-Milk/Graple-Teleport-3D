using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float JumpPow;
    public float speed;

    public float sensetivity;
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
        if(context.performed)
        {
            rb.AddForce(Vector3.up * JumpPow);
        }

    }

    public void Movement(InputAction.CallbackContext context)
    {
        Vector2 moveVector = context.ReadValue<Vector2>() * speed;
        rb.AddForce(transform.forward * moveVector.y + transform.right * moveVector.x);
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            speed *= 1.5f;
        }
        else if(context.canceled)
        {
            speed /= 1.5f;
        }
    }
    public void Action(InputAction.CallbackContext context)
    {
        IAction action = GetComponent<IAction>();
        if(context.performed)
        {
            action.Action(0);
        }
        else if(context.canceled)
        {
            action.Action(1);
        }
    }
}

public interface IAction
{
    void Action(int state);
}
