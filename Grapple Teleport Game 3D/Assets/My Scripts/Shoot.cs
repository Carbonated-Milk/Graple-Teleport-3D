using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    private Camera cam;
    private PlayerControls playerControls;

    public ParticleSystem muzzle;
    public GameObject hitDust;

    private
    void Awake()
    {
        cam = transform.GetComponentInChildren<Camera>();

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.LeftClick.performed += ShootGun;
    }

    public void ShootGun(InputAction.CallbackContext context)
    {
        /*RaycastHit hit;
        FindObjectOfType<AudioManager>().Play("Gunshot");
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            muzzle.Play();
            GameObject particals = Instantiate(hitDust);
            particals.transform.position = hit.point;
            Destroy(particals, 2);
        }*/

    }
    void Update()
    {

    }
}
