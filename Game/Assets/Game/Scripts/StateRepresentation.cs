using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum Turn
{
    PLAYER, AI
}

public class StateRepresentation : State
{
    public FieldObject[,] board = new FieldObject[7,10];

    public StateRepresentation()
    {
        GenerateBoard();

        GenerateFriendlies();
        GenerateEnemies();

        GenerateObstacles();

        CurrentTurn = Options.playerStarts ? Turn.PLAYER : Turn.AI;
    }

    public void ChangeTurn()
    {
        if (CurrentTurn == Turn.PLAYER)
        {
            CurrentTurn = Turn.AI;
        }
        else
        {
            CurrentTurn = Turn.PLAYER;
        }
    }

    public override Status GetStatus()
    {
        if ((GameManager.Instance.enemies.Count == 0))
        {
            return Status.PLAYERWIN;
        }

        if (GameManager.Instance.friendlies.Count == 0)
        {
            return Status.AIWIN;
        }

        return Status.PLAYING;
    }

    public override int GetHeuristics(Turn player)
    {
        //TODO
        return UnityEngine.Random.Range(0,100);
    }

    void GenerateBoard()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                board[i, j] = new FieldObject("EMPTY", new Vector2(j, i));
            }
        }
    }

    void GenerateFriendlies()
    {
        int friendlyCount = Options.friendlyCount;

        int j = 1;
        for (int x = 1; x <= friendlyCount; x++)
        {
            int i = ((x + 3) - (friendlyCount - x)) - 1;
            

            PlayerObject newFriendly = new PlayerObject("PLAYER" + x, new Vector2(j, i), 10, 1, false);
            board[i, j] = newFriendly;
        }
    }

    void GenerateEnemies()
    {
        int enemyCount = Options.enemyCount;

        int j = 8;
        for (int x = 1; x <= enemyCount; x++)
        {
            int i = ((x + 3) - (enemyCount - x)) - 1;

            PlayerObject newEnemy = new PlayerObject("ENEMY" + x, new Vector2(j, i), 10, 1, true);
            board[i, j] = newEnemy;
        }
    }

    void GenerateObstacles()
    {
        int obstacleCount = Options.obstacleCount;
        for (int i = 0; i < obstacleCount; i++)
        {
            int x = UnityEngine.Random.Range(2, 8);
            int y = UnityEngine.Random.Range(0, 7);

            if (board[y, x].id == "EMPTY")
            {
                board[y, x] = new FieldObject("OBSTACLE", new Vector2(x, y));
            }
        }
    }

    public List<PlayerObject> ListPlayerObjects(bool isAI)
    {
        return board.Cast<FieldObject>().Where(x => x is PlayerObject && (x as PlayerObject).isAI == isAI).Select(x => x as PlayerObject).ToList();
    }
    public List<PlayerObject> ListAllPlayerObjects()
    {
        return board.Cast<FieldObject>().Where(x => x is PlayerObject).Select(x => x as PlayerObject).ToList();
    }
    public List<FieldObject> ListObstacles()
    {
        return board.Cast<FieldObject>().Where(x => x.id == "OBSTACLE").ToList();
    }
    public override object Clone()
    {
        StateRepresentation newState = new StateRepresentation();
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        FieldObject[,] clonedBoard = new FieldObject[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (board[i, j] is PlayerObject)
                {
                    clonedBoard[i, j] = board[i, j].Clone() as PlayerObject;
                }
                else clonedBoard[i, j] = board[i, j].Clone() as FieldObject;

            }
        }
        newState.board = clonedBoard;
        newState.CurrentTurn = CurrentTurn;

        return newState;
    }

    
}
