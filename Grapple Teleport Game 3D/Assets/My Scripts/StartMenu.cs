using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
public class StartMenu : MonoBehaviour
{
    public Levels[] levels;

    public Animator topDoor;
    public Animator bottomDoor;

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
        StartCoroutine(LevelTransitioner(l.levelIndex));
        FindObjectOfType<AudioManager>().Play(l.startingThemeName);
    }

    public IEnumerator LevelTransitioner(int levelIndex)
    {
        topDoor.SetTrigger("Open");
        bottomDoor.SetTrigger("Open");

        yield return new WaitForSeconds(.6f);

        topDoor.SetTrigger("Close");
        bottomDoor.SetTrigger("Close");
    }

    public void LoadLevel(int sceneNum)
    {
        CurserLock();
        instance.transform.Find("PlayerUI").gameObject.SetActive(true);

        FindObjectOfType<AudioManager>().StopAllSongs();
        Levels l = Array.Find(levels, levels => levels.levelIndex == sceneNum);
        StartCoroutine(LevelTransitioner(l.levelIndex));
        FindObjectOfType<AudioManager>().Play(l.startingThemeName);
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
