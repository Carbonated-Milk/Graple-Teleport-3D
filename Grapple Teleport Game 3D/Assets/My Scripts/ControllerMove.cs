using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerMove : MonoBehaviour
{
    public float speed;
    public float fallMultiplier;
    public float jumpPow;
    public float physicsSpeed;
    public float interpolationNum;

    private float gravity;
    Vector3 velocity;

    bool isGrounded;
    private Transform groundCheck;
    public LayerMask groundMask;

    private CharacterController moveControl;
    private PlayerControls playerControls;

    private void Awake()
    {
        groundCheck = transform.Find("Ground Check");
        gravity = Physics.gravity.y;
        moveControl = GetComponent<CharacterController>();

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.Jump.performed += Jump;


        //speed = GetComponent<Player>().speed;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, .4f, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -.1f;
        }

        Vector2 moveVector = playerControls.Player.Movement.ReadValue<Vector2>();
        Vector3 playerDir = transform.forward * moveVector.y + transform.right * moveVector.x;
        playerDir *= speed;



        moveControl.Move(playerDir * Time.deltaTime * 5);
        velocity.y += gravity * Time.deltaTime * fallMultiplier;

        moveControl.Move(velocity * Time.deltaTime);


    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            velocity.y = jumpPow;
        }
    }
}
