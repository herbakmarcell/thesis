using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStat : MonoBehaviour
{
    public string id;
    public Vector2 position;
    public int health;
    public int attack;
    public bool isAI;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Dead");
        }
    }
}
