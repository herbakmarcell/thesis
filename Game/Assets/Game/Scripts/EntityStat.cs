using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStat : MonoBehaviour
{
    public int health;

    void Awake()
    {
        health = 10;
    }
}
