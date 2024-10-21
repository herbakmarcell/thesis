using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    public GameManager()
    {
        playerCount = 1;
        enemyCount = 1;
    }

    public int playerCount;
    public int enemyCount;

    public bool playerTurn = true;
    public int activePlayer;

    public List<GameObject> friendlies = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();



}

