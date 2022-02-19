using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lives : MonoBehaviour, IDamagable
{
    public int health;
    int currentHealth;
    public Vector3 respawn;
    public static Transform thisGameObject;

    bool needstoRespawn;

    void Awake()
    {
        respawn = transform.position;
        thisGameObject = gameObject.transform;
    }
    void Start()
    {
        currentHealth = health;
        respawn = transform.position;
        thisGameObject = gameObject.transform;
    }

    private void Update()
    {
        //Respawn();
    }

    public void Respawn()
    {
        Time.timeScale = 1;
        StartMenu.instance.CurserLock();
        StartMenu.instance.OpenPanel(StartMenu.instance.transform.Find("PlayerUI").gameObject);

        needstoRespawn = true;


        GetComponent<CharacterController>().Move(respawn - GetComponent<CharacterController>().transform.position);
        GetComponent<Rigidbody>().position = respawn;
        transform.position = respawn;


    }
    public void Die()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartMenu.instance.transform.Find("PlayerUI").gameObject.SetActive(false);
        StartMenu.instance.transform.Find("You Died").gameObject.SetActive(true);

        Time.timeScale = 0;

    }

    public void Damaged(int damage)
    {
        currentHealth -= damage;
    }
}

public interface IDamagable
{
    void Damaged(int damage);
}
