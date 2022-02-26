using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLives : MonoBehaviour, IDamagable
{
    public float health;
    private float currentHealth;

    public void Start()
    {
        currentHealth = health;
    }
    public void Damaged(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
