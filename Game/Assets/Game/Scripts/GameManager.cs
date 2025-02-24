using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

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

        stateRepresentation = new StateRepresentation();
        solver = new MiniMaxWithAlphaBetaPruning(new TurnOperatorGenerator(), 3);

        actionPosition = new Vector2(-1, -1);
    }

    [SerializeField]
    public State stateRepresentation;
    public Solver solver;

    public bool playerTurn = true;
    public int activePlayer;

    public bool actionSelected;
    public bool moveTurn;

    public Vector2 actionPosition;


    public List<GameObject> friendlies;
    public List<GameObject> enemies;
    public List<GameObject> obstacles;

    public void NextTurn()
    {
        Operator o;
        List<PlayerAction> actions = new List<PlayerAction>();
        if (playerTurn)
        {
            if (activePlayer < Options.friendlyCount)
            {
                if(moveTurn)
                {
                    PlayerAction playerAction = new PlayerAction("PLAYER" + (activePlayer + 1), ActionType.MOVE, actionPosition);
                    actions.Add(playerAction);
                }
                activePlayer++;
            }
            else
            {
                o = new TurnOperator(actions);
                stateRepresentation = o.Apply(stateRepresentation);
                activePlayer = 0;
                playerTurn = false;
            }
            actionSelected = false;
            moveTurn = false;
        } 
        else
        {
            Debug.Log("AI TURN");
            stateRepresentation = solver.NextMove(stateRepresentation);
        }
    }
}

