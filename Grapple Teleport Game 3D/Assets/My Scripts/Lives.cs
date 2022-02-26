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

    public GameObject healthBar;
    void Awake()
    {
        GameManager.lives = this;
        respawn = transform.position;
        thisGameObject = gameObject.transform;
    }
    void Start()
    {
        currentHealth = health;
        respawn = transform.position;
        thisGameObject = gameObject.transform;
        healthBar = GameManager.menuManager.transform.Find("PlayerUI/Health Bar Holder/HealthBar").gameObject;
    }

    private void Update()
    {
        float healthRatio = (float)currentHealth / (float)health;
        float healthX = Mathf.Lerp(healthBar.transform.localScale.x, healthRatio, .05f);
        healthBar.transform.localScale = new Vector3(healthX, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    public void Respawn()
    {
        Time.timeScale = 1;
        currentHealth = health;
        GameManager.menuManager.CurserLock();
        GameManager.menuManager.OpenPanel(GameManager.menuManager.transform.Find("PlayerUI").gameObject);


        GetComponent<CharacterController>().Move(respawn - GetComponent<CharacterController>().transform.position);
        GetComponent<Rigidbody>().position = respawn;
        transform.position = respawn;


    }
    public void Die()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameManager.menuManager.transform.Find("PlayerUI").gameObject.SetActive(false);
        GameManager.menuManager.transform.Find("You Died").gameObject.SetActive(true);

        Time.timeScale = 0;

    }

    public void Damaged(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
}

public interface IDamagable
{
    void Damaged(int damage);

    void Die();
}
