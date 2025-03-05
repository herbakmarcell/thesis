using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public enum ActionSelected{
    MOVE, ATTACK, NONE
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

        stateRepresentation = new StateRepresentation();
        solver = new MiniMaxWithAlphaBetaPruning(new TurnOperatorGenerator(), 3);

        currentStatus = Status.PLAYING;
        actionSelected = ActionSelected.NONE;

        playerTurn = Options.playerStarts;
    }

    [SerializeField]
    public State stateRepresentation;
    public Solver solver;

    public bool playerTurn = true;
    public int activePlayer;

    public Status currentStatus;

    public ActionSelected actionSelected;
    public ActionDirection actionDirection;

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
                PlayerAction playerAction = null;
                switch (actionSelected)
                {
                    case ActionSelected.MOVE:
                        playerAction = new PlayerAction("PLAYER" + (activePlayer + 1), ActionType.MOVE, actionDirection);
                        break;
                    case ActionSelected.ATTACK:
                        playerAction = new PlayerAction("PLAYER" + (activePlayer + 1), ActionType.ATTACK, actionDirection);
                        break;
                    case ActionSelected.NONE:
                    default:
                        break;

                }
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
            actionSelected = ActionSelected.NONE;
        } 
        else
        {
            Debug.Log("AI TURN");
            stateRepresentation = solver.NextMove(stateRepresentation);
        }
    }
}

