using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    public static Vector3 respawn;
    void Start()
    {
        respawn = transform.position;
    }
    void Update()
    {
        
    }

    public static void Respawn()
    {
        Player.playerObject.transform.position = respawn;
        Time.timeScale = 1;

        StartMenu.instance.CurserLock();
        StartMenu.instance.OpenPanel(StartMenu.instance.transform.Find("PlayerUI").gameObject);
    }
    public void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        StartMenu.instance.transform.Find("You Died").gameObject.SetActive(true);

        Time.timeScale = 0;
    }
}
