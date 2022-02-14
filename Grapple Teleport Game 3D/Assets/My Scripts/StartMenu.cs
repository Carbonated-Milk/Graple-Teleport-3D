using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
public class StartMenu : MonoBehaviour
{
    public Levels[] levels;

    public static StartMenu instance;
    void Awake()
    {
        //doesn't destroy menu manager
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void OpenPanel(GameObject open)
    {
        open.SetActive(true);
    }

    public void ClosePanel(GameObject close)
    {
        close.SetActive(false);
    }

    public void LoadScene(int sceneNum)
    {
        FindObjectOfType<AudioManager>().StopAllSongs();
        Levels l = Array.Find(levels, levels => levels.levelIndex == sceneNum);
        FindObjectOfType<AudioManager>().Play(l.startingThemeName);
        SceneManager.LoadScene(l.levelIndex);
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

}

[System.Serializable]
public class Levels
{
    public string levelName;
    public int levelIndex; 
    public string startingThemeName;
}
