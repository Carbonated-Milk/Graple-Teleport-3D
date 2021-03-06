using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
public class MenuManager : MonoBehaviour
{
    public Levels[] levels;

    public Animator topDoor;

    private GameObject[] panels;

    private float waitTime = 1.5f;
    void Awake()
    {
        //doesn't destroy menu manager
        if(GameManager.menuManager == null)
        {
            GameManager.menuManager = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        panels = new GameObject[transform.childCount - 2];
        for(int i = 0; i < transform.childCount - 2; i++)
        {
            panels[i] = transform.GetChild(i).gameObject;
        }
    }

    public void OpenPanel(GameObject open)
    {
        CloseAllPanels();
        open.SetActive(true);
    }

    public void LoadScene(int sceneNum)
    {
        Levels l = null;
        if(sceneNum == -1)
        {
            l = l = Array.Find(levels, levels => levels.levelIndex == SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            l = Array.Find(levels, levels => levels.levelIndex == sceneNum);
        }
        StartCoroutine(LevelTransitioner(l));
    }

    public IEnumerator LevelTransitioner(Levels l)
    {
        topDoor.SetTrigger("Close");

        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(l.levelIndex);

        CloseAllPanels();

        switch (l.levelIndex)
        {
            case 0:
                transform.Find("Main Menu").gameObject.SetActive(true);
                break;
            default:
                CurserLock();
                transform.Find("PlayerUI").gameObject.SetActive(true);
                break;
        }

        if(GameManager.player != null)
        {
            GameManager.player.playerControls.Player.Disable();
            GameManager.shoot.playerControls.Player.Disable();
        }

        GameManager.audioManager.StopAllSongs();
        GameManager.audioManager.Play(l.startingThemeName);

        topDoor.SetTrigger("Open");
    }
    public void SetTimeScale(float newTime)
    {
        Time.timeScale = newTime;
    }

    public void CurserLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void CurserUnlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    public void Respawn()
    {
        GameManager.lives.Respawn();
    }
}

[System.Serializable]
public class Levels
{
    public string levelName;
    public int levelIndex; 
    public string startingThemeName;
    public bool isLevel;
}
