using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainScript : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyDamage") playerHealth.PlayerDamage();
    }
}
