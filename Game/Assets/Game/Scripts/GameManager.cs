using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

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

    private GameManager()
    {
        friendlies = new List<GameObject>();
        enemies = new List<GameObject>();
        obstacles = new List<GameObject>();

        aiHealthUI = GameObject.FindObjectOfType<AIHealthUI>();
        playerHealthUI = GameObject.FindObjectOfType<PlayerHealthUI>();

        stateRepresentation = new StateRepresentation();
        solver = new MiniMaxWithAlphaBetaPruning(new TurnOperatorGenerator(), 2);

        currentStatus = Status.PLAYING;
        actionSelected = ActionSelected.NONE;

        playerTurn = Options.playerStarts;
        activePlayer = 0;

        aiCount = Options.enemyCount;
        playerCount = Options.friendlyCount;

        firstTurn = true;
    }

    public State stateRepresentation;
    public Solver solver;

    public bool playerTurn;
    public bool firstTurn;
    public int activePlayer;

    public Status currentStatus;

    public ActionSelected actionSelected;
    public ActionDirection actionDirection;

    public List<GameObject> friendlies;
    public List<GameObject> enemies;
    public List<GameObject> obstacles;

    List<PlayerAction> actions = new List<PlayerAction>();

    AIHealthUI aiHealthUI;
    PlayerHealthUI playerHealthUI;

    int aiCount;
    int playerCount;

    public void ProgressGame()
    {
        Operator o;

        if (playerTurn)
        {
            if (!firstTurn)
            {
                firstTurn = false;
                RedrawTable();
            }
            if (activePlayer < friendlies.Count)
            {
                if (activePlayer == 0)
                {
                    aiCount = (stateRepresentation as StateRepresentation).ListPlayerObjects(true).Count;
                }
                PlayerAction playerAction = null;
                switch (actionSelected)
                {
                    case ActionSelected.MOVE:
                        playerAction = new PlayerAction(friendlies[activePlayer].GetComponent<EntityStat>().id, ActionType.MOVE, actionDirection);
                        break;
                    case ActionSelected.ATTACK:
                        playerAction = new PlayerAction(friendlies[activePlayer].GetComponent<EntityStat>().id, ActionType.ATTACK, actionDirection);
                        break;
                    case ActionSelected.NONE:
                    default:
                        return;

                }
                if (enemies.Count == 0)
                {
                    currentStatus = Status.PLAYERWIN;
                    SceneManager.LoadScene("EndScene");
                    SceneManager.UnloadSceneAsync("GameScene");
                }
                RedrawTable();

                actions.Add(playerAction);
                actionSelected = ActionSelected.NONE;
                activePlayer++;
            }
            
            if (activePlayer >= friendlies.Count)
            {
                o = new TurnOperator(actions);
                stateRepresentation = o.Apply(stateRepresentation);
                activePlayer = 0;
                actionSelected = ActionSelected.NONE;
                playerTurn = false;
                actions.Clear();
            }
        }
        if (CheckStatus(stateRepresentation))
        {
            currentStatus = Status.PLAYERWIN;
            SceneManager.LoadScene("EndScene");
            SceneManager.UnloadSceneAsync("GameScene");
        }

        if (aiCount != (stateRepresentation as StateRepresentation).ListPlayerObjects(true).Count)
        {
            StateRepresentation s = GameManager.Instance.stateRepresentation as StateRepresentation;
            solver = new MiniMaxWithAlphaBetaPruning(new TurnOperatorGenerator(s.ListPlayerObjects(false), s.ListPlayerObjects(true)), 2);
        }

        if (!playerTurn) 
        {
            playerCount = (stateRepresentation as StateRepresentation).ListPlayerObjects(false).Count;
            stateRepresentation = solver.NextMove(stateRepresentation);

            if (CheckStatus(stateRepresentation))
            {
                currentStatus = Status.AIWIN;
                SceneManager.LoadScene("EndScene");
                SceneManager.UnloadSceneAsync("GameScene");
            }
            else
            {
                if (playerCount != (stateRepresentation as StateRepresentation).ListPlayerObjects(true).Count)
                {
                    StateRepresentation s = GameManager.Instance.stateRepresentation as StateRepresentation;
                    solver = new MiniMaxWithAlphaBetaPruning(new TurnOperatorGenerator(s.ListPlayerObjects(false), s.ListPlayerObjects(true)), 2);
                }
                playerTurn = true;
            }
            RedrawTable();
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

    public void RedrawTable()
    {
        StateRepresentation stateRepresentation = GameManager.Instance.stateRepresentation as StateRepresentation;

        List<PlayerObject> enemyObjects = stateRepresentation.ListPlayerObjects(true);
        List<PlayerObject> friendlyObjects = stateRepresentation.ListPlayerObjects(false);


        List<GameObject> deletableEnemyObjects = new List<GameObject>();
        List<GameObject> deletableFriendlyObjects = new List<GameObject>();

        foreach (var gameObject in enemies)
        {
            bool contains = enemyObjects.Any(x => x.id == gameObject.GetComponent<EntityStat>().id);
            if (!contains)
            {
                deletableEnemyObjects.Add(gameObject);
            }
        }

        foreach (var gameObject in friendlies)
        {
            bool contains = friendlyObjects.Any(x => x.id == gameObject.GetComponent<EntityStat>().id);
            if (!contains)
            {
                deletableFriendlyObjects.Add(gameObject);
            }
        }

        deletableEnemyObjects.ForEach(enemy =>
        {
            enemies.Remove(enemy);
            GameObject.Destroy(enemy);
        });

        deletableFriendlyObjects.ForEach(friendly =>
        {
            friendlies.Remove(friendly);
            GameObject.Destroy(friendly);
        });

        enemies.ForEach(enemy =>
        {
            enemy.GetComponent<EntityStat>().health = enemies.Find(x => x.GetComponent<EntityStat>().id == enemy.GetComponent<EntityStat>().id).GetComponent<EntityStat>().health;
        });

        friendlies.ForEach(friendly =>
        {
            friendly.GetComponent<EntityStat>().health = friendlyObjects.Find(x => x.id == friendly.GetComponent<EntityStat>().id).health;
        });

        aiHealthUI.UpdateHealthUI();
        playerHealthUI.UpdateHealthUI();

        foreach (var enemy in stateRepresentation.ListPlayerObjects(true))
        {
            enemies.ForEach(e =>
            {
                if (e.GetComponent<EntityStat>().id == enemy.id)
                {
                    e.GetComponent<EntityStat>().position = enemy.position;
                }
            });
        }

        foreach (var enemy in enemies)
        {
            enemy.transform.position = new Vector3(enemy.GetComponent<EntityStat>().position.x - 4.5f, enemy.GetComponent<EntityStat>().position.y - 3f, 0);
        }
    }
}


