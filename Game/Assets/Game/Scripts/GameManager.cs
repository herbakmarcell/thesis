using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;

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
        solver = new MiniMaxWithAlphaBetaPruning(new TurnOperatorGenerator(), 2);

        currentStatus = Status.PLAYING;
        actionSelected = ActionSelected.NONE;

        playerTurn = Options.playerStarts;
        activePlayer = 0;

        //ProgressGame();
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

    List<PlayerAction> actions = new List<PlayerAction>();

    public void ProgressGame()
    {
        Operator o;
        
        if (playerTurn)
        {
            if (activePlayer < friendlies.Count)
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
                        return;

                } 
                actions.Add(playerAction);
                actionSelected = ActionSelected.NONE;
                activePlayer++;
            }
            if(activePlayer >= friendlies.Count)
            {
                o = new TurnOperator(actions);
                stateRepresentation = o.Apply(stateRepresentation);
                PlayerObject player = (stateRepresentation as StateRepresentation).ListAllPlayerObjects().Find(x=> x.id.Contains("PLAYER"));
                Debug.Log($"Name: {player.id} " + $"X: {player.position.x} " + $"Y: {player.position.y} ");
                activePlayer = 0;
                actionSelected = ActionSelected.NONE;
                playerTurn = false;
                actions.Clear();
            }
        }
        if (!playerTurn) 
        {
            if (CheckStatus(stateRepresentation)) 
            {
                currentStatus = Status.PLAYERWIN;
            }

            stateRepresentation = solver.NextMove(stateRepresentation);

            RedrawTable();

            if (CheckStatus(stateRepresentation))
            {
                currentStatus = Status.AIWIN;
            } 
            else
            {
                playerTurn = true;
            }
        }

    }

    private bool CheckStatus(State state)
    {
        StateRepresentation stateRepresentation = state as StateRepresentation;

        if (stateRepresentation.GetStatus() == Status.PLAYERWIN)
        {
            return true;
        }

        if (stateRepresentation.GetStatus() == Status.AIWIN)
        {
            return true;
        }

        return false;
    }

    void RedrawTable()
    {
        StateRepresentation stateRepresentation = GameManager.Instance.stateRepresentation as StateRepresentation;
        List<PlayerObject> enemyObjects = stateRepresentation.ListPlayerObjects(true);
        for (int i = 0; i < enemyObjects.Count; i++)
        {
            GameObject enemy = enemies[i];
            enemy.transform.position = new Vector3(enemyObjects[i].position.x - 4.5f, enemyObjects[i].position.y - 3f, 0);
        }

        List<PlayerObject> friendlyObjects = stateRepresentation.ListPlayerObjects(false);
        friendlies.ForEach(friendly =>
        {
            friendly.GetComponent<EntityStat>().health = friendlyObjects.Find(x => x.id == friendly.GetComponent<EntityStat>().id).health;
        });
    }
}


