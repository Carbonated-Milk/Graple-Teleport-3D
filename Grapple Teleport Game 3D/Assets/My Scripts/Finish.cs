using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            StartMenu.instance.transform.Find("Level Complete").gameObject.SetActive(true);
            StartMenu.instance.transform.Find("PlayerUI").gameObject.SetActive(false);
            Time.timeScale = 0;
        }
    }
}
