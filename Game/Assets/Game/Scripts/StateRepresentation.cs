using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public enum Player
{
    PLAYER, AI
}

public class StateRepresentation //: ICloneable
{
    public FieldObject[,] board = new FieldObject[7,10];

    public Player currentPlayer;

    public StateRepresentation()
    {
        GenerateBoard();

        GenerateFriendlies();
        GenerateEnemies();

        GenerateObstacles();

        currentPlayer = Options.playerStarts ? Player.PLAYER : Player.AI;
    }

    int GetHeuristics(Player player)
    {
        return 0;
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
            int i = (x + 3) - (friendlyCount - x);
            

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
            int i = (x + 3) - (enemyCount - x);

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
    public List<FieldObject> ListObstacles()
    {
        return board.Cast<FieldObject>().Where(x => x.id == "OBSTACLE").ToList();
    }

    //public object Clone()
    //{
    //    StateRepresentation newState = new StateRepresentation();
    //    newState.board = board.Clone() as FieldObject[,];
    //}
}
