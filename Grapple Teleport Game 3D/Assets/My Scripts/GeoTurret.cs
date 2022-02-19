using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoTurret : MonoBehaviour
{
    public float rotateSpeed;
    public Animator shootAnim;

    public int shots;
    public float delay;
    Transform target;
    Transform sphereBase;
    Transform cannon;
    Transform shootPoint;

    public GameObject bullet;


    bool locked;
    public float shootSpeed;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        sphereBase = transform.Find("Sphere").transform;
        cannon = sphereBase.transform.Find("Cannon").transform;
        shootPoint = cannon.transform.Find("ShootPoint").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!locked)
        {
            sphereBase.rotation = Quaternion.RotateTowards(sphereBase.rotation, Quaternion.LookRotation(target.position - transform.position), rotateSpeed);
        }

        if(sphereBase.rotation == Quaternion.LookRotation(target.position - transform.position) && !locked)
        {
            StartCoroutine(Shoot());
            locked = true;
        }
    }

    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(.5f);

        for(int i = 0; i < shots; i++)
        {
            Fire();
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(.5f);
        locked = false;

    }

    private void Fire()
    {
        GameObject projectile = Instantiate(bullet) as GameObject;
        projectile.GetComponent<Rigidbody>().velocity = shootPoint.up * shootSpeed;
        projectile.transform.position = shootPoint.position;
        shootAnim.SetTrigger("Shoot");
    }
}
