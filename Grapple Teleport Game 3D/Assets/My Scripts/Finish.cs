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

            GameManager.menuManager.transform.Find("Level Complete").gameObject.SetActive(true);
            GameManager.menuManager.transform.Find("PlayerUI").gameObject.SetActive(false);
            Time.timeScale = 0;
        }
    }
}
