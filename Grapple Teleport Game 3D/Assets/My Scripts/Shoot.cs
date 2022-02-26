using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public int gunDamage;
    private Camera cam;
    public PlayerControls playerControls;

    public ParticleSystem muzzle;
    public GameObject hitDust;

    private
    void Awake()
    {
        if (GameManager.shoot == null)
        {
            GameManager.shoot = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        cam = transform.GetComponentInChildren<Camera>();

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.LeftClick.performed += ShootGun;
    }

    public void ShootGun(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        FindObjectOfType<AudioManager>().Play("Gunshot");
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            if(hit.collider.gameObject.GetComponent<IDamagable>() != null)
            {
                hit.collider.gameObject.GetComponent<IDamagable>().Damaged(gunDamage);
            }
            muzzle.Play();
            GameObject particals = Instantiate(hitDust);
            particals.transform.position = hit.point;
            Destroy(particals, 2);
        }

    }
    void Update()
    {

    }
}
