using System;
using UnityEngine;
public class Laptop : MonoBehaviour
{
    private AudioSource keyboardClicks;
    private LayerMask enemyMask;
    private void Start()
    {
        keyboardClicks = GetComponent<AudioSource>();
        enemyMask = LayerMask.GetMask("Enemy");
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            keyboardClicks.Play();
            keyboardClicks.time = 14;
            Collider[] enemyInRadius = Physics.OverlapSphere(transform.position, 1000, enemyMask);
            for (int i = 0; i < enemyInRadius.Length; i++)
            {
                enemyInRadius[i].GetComponent<EnemyPlayerSpotted>().enabled = false;
                enemyInRadius[i].GetComponent<EnemyPlayerSpotted>().enabled = true;
            }
        }
    }
}
