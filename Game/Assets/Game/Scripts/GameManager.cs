using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    HEAVY,
    LIGHT,
    SPELL,
    NONE
}

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

    public bool actionSelected;
    public bool moveTurn;
    public AttackType attackType;


    public List<GameObject> friendlies = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    public void NextTurn()
    {
        if (playerTurn)
        {
            if (activePlayer + 1 < playerCount)
            {
                activePlayer++;
                Debug.Log(activePlayer);
            }
            else
            {
                activePlayer = 0;
                playerTurn = false;
            }
            actionSelected = false;
            moveTurn = false;
            attackType = AttackType.NONE;
        }
    }
}

