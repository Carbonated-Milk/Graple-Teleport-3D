using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    public float menuSensitivity;
    private Camera menuCam;
    public float lerpSpeed;
    private Quaternion targetQuaternion;

    private PlayerControls playerControls;
    // Start is called before the first frame update
    void Start()
    {
        menuCam = GetComponent<Camera>();
        targetQuaternion = menuCam.transform.rotation;

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseVector = playerControls.Player.Mouse.ReadValue<Vector2>() * menuSensitivity * Time.deltaTime * 100;
        targetQuaternion *= Quaternion.Euler(Vector3.up * mouseVector.x + Vector3.right * -mouseVector.y);
        menuCam.transform.localRotation = Quaternion.Slerp(menuCam.transform.localRotation, targetQuaternion, lerpSpeed);
    }
}
