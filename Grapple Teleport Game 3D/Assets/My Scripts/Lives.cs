using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lives : MonoBehaviour
{
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
        respawn = transform.position;
        thisGameObject = gameObject.transform;
    }
    void LateUpdate()
    {
        
        if(needstoRespawn)
        {
            GetComponent<CharacterController>().transform.position = respawn;
            
        }
        if(GetComponent<CharacterController>().transform.position == respawn)
        {
            needstoRespawn = false;
        }
    }

    public void Respawn()
    {
        GetComponent<CharacterController>().Move(respawn - GetComponent<CharacterController>().transform.position);
        GetComponent<Rigidbody>().position = respawn;
        transform.position = respawn;

        Time.timeScale = 1;
        StartMenu.instance.CurserLock();
        StartMenu.instance.OpenPanel(StartMenu.instance.transform.Find("PlayerUI").gameObject);

        needstoRespawn = true;
        GetComponent<CharacterController>().Move(respawn - GetComponent<CharacterController>().transform.position);
        GetComponent<Rigidbody>().position = respawn;

        transform.position = respawn;
        GetComponent<CharacterController>().Move(respawn - GetComponent<CharacterController>().transform.position);
        GetComponent<Rigidbody>().position = respawn;
        transform.position = respawn;
        
    }
    public void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        StartMenu.instance.transform.Find("PlayerUI").gameObject.SetActive(false);
        StartMenu.instance.transform.Find("You Died").gameObject.SetActive(true);

        Time.timeScale = 0;

        GetComponent<CharacterController>().Move(respawn - GetComponent<CharacterController>().transform.position);
        GetComponent<Rigidbody>().position = respawn;
        transform.position = respawn;
        
    }
}
