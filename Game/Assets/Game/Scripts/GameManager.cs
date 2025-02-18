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
        friendlies = new List<GameObject>();
        enemies = new List<GameObject>();
        obstacles = new List<GameObject>();
        // EL KELL TÁVOLÍTANI MAJD EZT A SORT
        // A MENÜNEK MEGFELELÕEN KELL MAJD MENNIE
        stateRepresentation = new StateRepresentation();
    }

    public StateRepresentation stateRepresentation;

    public bool playerTurn = true;
    public int activePlayer;

    public bool actionSelected;
    public bool moveTurn;
    public AttackType attackType;


    public List<GameObject> friendlies;
    public List<GameObject> enemies;
    public List<GameObject> obstacles;

    public void NextTurn()
    {
        if (playerTurn)
        {
            if (activePlayer + 1 < Options.friendlyCount)
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

