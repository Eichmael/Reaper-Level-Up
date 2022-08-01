using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    public int maxHealth = 40;
    private int currentHealth;

    private void OnDisable()
    {
        PlayerScore.IncreasePlayerXpBy15();
    }

    void Start()
    {
        //Setting max health according to player level
        maxHealth += ((PlayerScore.CurrentPlayerLevel * 15) / 100);
        //Setting current health
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
            PlayerScore.IncreasePlayerScoreWhenEnemyDies();
            //Telling game to update score immediately
            GetComponent<EnemyManager>().Player.GetComponent<PlayerCombat>().UpdateScoreImmediately();
            //Starting death routine
            GetComponent<EnemyManager>().StartDeathRoutine();
            return;
        }

        //Knockback force
        GetComponent<EnemyManager>().StartHurtRoutine();
    }

    void Die()
    {
        Destroy(this.gameObject, 0.2f);
    }
}