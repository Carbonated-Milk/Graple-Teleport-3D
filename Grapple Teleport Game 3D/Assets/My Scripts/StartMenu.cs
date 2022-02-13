using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class StartMenu : MonoBehaviour
{
    private Camera menuCam;
    public float menuSensitivity;
    public float lerpSpeed;

    private PlayerControls playerControls;

    private Quaternion targetQuaternion;
    // Start is called before the first frame update
    void Awake()
    {
        menuCam = FindObjectOfType<Camera>();

        playerControls = new PlayerControls();
        playerControls.Player.Enable();

        targetQuaternion = menuCam.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseVector = playerControls.Player.Mouse.ReadValue<Vector2>() * menuSensitivity * Time.deltaTime * 100;
        targetQuaternion *= Quaternion.Euler(Vector3.up * mouseVector.x + Vector3.right * -mouseVector.y);
        menuCam.transform.localRotation = Quaternion.Slerp(menuCam.transform.localRotation, targetQuaternion, lerpSpeed);
    }

    public void OpenPanel(GameObject open)
    {
        open.SetActive(true);
    }

    public void ClosePanel(GameObject close)
    {
        close.SetActive(false);
    }

    public static void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

}
