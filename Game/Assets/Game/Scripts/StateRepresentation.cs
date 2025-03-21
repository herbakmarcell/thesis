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
        if (ListPlayerObjects(true).Count == 0)
        {
            return Status.PLAYERWIN;
        }

        if (ListPlayerObjects(false).Count == 0)
        {
            return Status.AIWIN;
        }

        return Status.PLAYING;
    }

    static int WIN = 100;
    static int LOSE = -100;



    public override int GetHeuristics(Turn player)
    {
        if (GetStatus() == Status.PLAYERWIN && player == Turn.PLAYER) return WIN;
        if (GetStatus() == Status.AIWIN && player == Turn.AI) return WIN;

        if (GetStatus() == Status.PLAYERWIN && player != Turn.PLAYER) return LOSE;
        if (GetStatus() == Status.AIWIN && player != Turn.AI) return LOSE;

        int result = 0;

        List<PlayerObject> playerObjects = ListPlayerObjects(false);
        List<PlayerObject> AIObjects = ListPlayerObjects(false);

        int playerCount = playerObjects.Count;
        int AICount = AIObjects.Count;

        if (player == Turn.PLAYER)
        {
            result += HealthBonus(player);

            result += AttackBonus(player, playerObjects, AIObjects); 

            result += PlayerCount(player, playerCount, AICount);

            result += BadPosition(player, playerObjects, AIObjects);
        }
        else
        {
            result += HealthBonus(player);

            result += AttackBonus(player, playerObjects, AIObjects);

            result += PlayerCount(player, playerCount, AICount);

            result += BadPosition(player, playerObjects, AIObjects);
        }

        return result;
    }

    int BadPosition(Turn player, List<PlayerObject> playerObjects, List<PlayerObject> AIObjects)
    {
        int result = 0;

        if (player == Turn.PLAYER)
        {
            foreach (PlayerObject playerO in playerObjects)
            {
                List<PlayerObject> nearbyEnemies = new List<PlayerObject>();
                List<PlayerObject> nearbyFriendlies = new List<PlayerObject>();
                result += CheckPossibleMoves(playerO);
                if (HasNearbyEnemy(player, playerO, nearbyEnemies))
                {
                    result -= 5 * nearbyEnemies.Count;
                }
                if (HasNearbyFriendly(player, playerO, nearbyFriendlies))
                {
                    result -= 3 * nearbyFriendlies.Count;
                }
            }
        } 
        else
        {
            foreach (PlayerObject playerO in AIObjects)
            {
                List<PlayerObject> nearbyEnemies = new List<PlayerObject>();
                List<PlayerObject> nearbyFriendlies = new List<PlayerObject>();
                result += CheckPossibleMoves(playerO);
                if (HasNearbyEnemy(player, playerO, nearbyEnemies))
                {
                    result -= 5 * nearbyEnemies.Count;
                }
                if (HasNearbyFriendly(player, playerO, nearbyFriendlies))
                {
                    result -= 3 * nearbyFriendlies.Count;
                }
            }
        }

        return result;
    }


    int CheckPossibleMoves(PlayerObject player)
    {
        int result = 0;

        if (!InvalidPosition(player.position + Vector2.up)) result += 1;
        else result -= 2;
        if (!InvalidPosition(player.position + Vector2.down)) result += 1;
        else result -= 2;
        if (!InvalidPosition(player.position + Vector2.left)) result += 1;
        else result -= 2;
        if (!InvalidPosition(player.position + Vector2.right)) result += 1;
        else result -= 2;

        return result;
    }

    bool InvalidPosition(Vector2 position)
    {
        if (position.x < 0 || position.x > 9 || position.y < 0 || position.y > 6) return true;
        return false;
    }


    int PlayerCount(Turn player, int playerCount, int AICount)
    {
        if (player == Turn.PLAYER) return 3 * (playerCount - AICount);
        else return 3 * (AICount - playerCount);
    }

    int AttackBonus(Turn player, List<PlayerObject> playerObjects, List<PlayerObject> AIObjects)
    {
        int result = 0;

        List<PlayerObject> nearbyEnemies = new List<PlayerObject>();

        if (player == Turn.PLAYER)
        {
            foreach (PlayerObject playerO in playerObjects)
            {
                if (HasNearbyEnemy(player, playerO, nearbyEnemies))
                {
                    if (playerO.health == 1) result -= 5 * nearbyEnemies.Count;
                    else result += 3 * (playerO.health - nearbyEnemies.Count);
                }
            }
        } 
        else
        {
            foreach (PlayerObject playerO in AIObjects)
            {
                if (HasNearbyEnemy(player, playerO, nearbyEnemies))
                {
                    if (playerO.health == 1) result -= 5 * nearbyEnemies.Count;
                    else result += 3 * (playerO.health - nearbyEnemies.Count);
                }
            }
        }
        return result;
    }

    bool HasNearbyEnemy(Turn playerTurn, PlayerObject player, List<PlayerObject> nearbyEnemies)
    {
        bool hasEnemyNearby = false;
        List<PlayerObject> enemies;
        if (playerTurn == Turn.PLAYER) enemies = ListPlayerObjects(true);
        else enemies = ListPlayerObjects(false);


        foreach (PlayerObject enemy in enemies)
        {
            if ((Math.Abs(player.position.x - enemy.position.x) == 1 && player.position.y == enemy.position.y) || 
                (Math.Abs(player.position.y - enemy.position.y) == 1 && player.position.x == enemy.position.x))
            {
                hasEnemyNearby = true;
                nearbyEnemies.Add(enemy);
            }
        }

        return hasEnemyNearby;
    }

    bool HasNearbyFriendly(Turn playerTurn, PlayerObject player, List<PlayerObject> nearbyFriendlies)
    {
        bool hasFriendlyNearby = false;
        List<PlayerObject> friendlies;
        if (playerTurn == Turn.PLAYER) friendlies = ListPlayerObjects(false);
        else friendlies = ListPlayerObjects(true);


        foreach (PlayerObject friendly in friendlies)
        {
            if ((Math.Abs(player.position.x - friendly.position.x) == 1 && player.position.y == friendly.position.y) ||
                (Math.Abs(player.position.y - friendly.position.y) == 1 && player.position.x == friendly.position.x))
            {
                hasFriendlyNearby = true;
                nearbyFriendlies.Add(friendly);
            }
        }

        return hasFriendlyNearby;
    }


    int HealthBonus(Turn player)
    {
        int result = 0;
        if (player == Turn.PLAYER)
        {
            foreach (PlayerObject playerO in ListPlayerObjects(false))
            {
                result += playerO.health;
            }
            foreach (PlayerObject playerO in ListPlayerObjects(true))
            {
                result -= playerO.health;
            }
        } 
        else
        {
            foreach (PlayerObject playerO in ListPlayerObjects(true))
            {
                result += playerO.health;
            }
            foreach (PlayerObject playerO in ListPlayerObjects(false))
            {
                result -= playerO.health;
            }
        }
        return result;
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
            

            PlayerObject newFriendly = new PlayerObject("PLAYER" + x, new Vector2(j, i), 10, 10, false);
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
