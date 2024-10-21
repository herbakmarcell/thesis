using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    public GameObject friendlyModel;
    public List<GameObject> friendlies;
    public GameObject enemyModel;
    public List<GameObject> enemies;

    public GameObject gridParent;

    private void Awake()
    {
        friendlies = new List<GameObject>();
        enemies = new List<GameObject>();
    }

    void Start()
    {
        GenerateFriendlies();
        GenerateEnemies();
    }


    void Update()
    {

    }

    void GenerateFriendlies()
    {
        int x = -2;
        int y;
        GameObject newPlayer;
        switch (GameManager.Instance.playerCount)
        {
            case 1:
                y = 0;
                newPlayer = Instantiate(friendlyModel, new Vector3(x + 0.5f, y, 0), Quaternion.identity, gridParent.transform);
                friendlies.Add(newPlayer);
                break;
            case 2:
                for (int i = -1; i <= 1; i+=2)
                {
                    y = i;
                    newPlayer = Instantiate(friendlyModel, new Vector3(x + 0.5f, y, 0), Quaternion.identity, gridParent.transform);
                    friendlies.Add(newPlayer);
                }
                break; 
            case 3:
                for (int i = -2; i <= 2; i+=2)
                {
                    y = i;
                    newPlayer = Instantiate(friendlyModel, new Vector3(x + 0.5f, y, 0), Quaternion.identity, gridParent.transform);
                    friendlies.Add(newPlayer);
                }
                break;
            case 4:
            default:
                for (int i = -3; i <= 3; i+=2)
                {
                    y = i;
                    newPlayer = Instantiate(friendlyModel, new Vector3(x + 0.5f, y, 0), Quaternion.identity, gridParent.transform);
                    friendlies.Add(newPlayer);
                }
                break;
        }
        GameManager.Instance.friendlies = friendlies;
    }

    void GenerateEnemies()
    {
        int x = 5;
        int y;
        GameObject newEnemy;
        switch (GameManager.Instance.enemyCount)
        {
            case 1:
                y = 0;
                newEnemy = Instantiate(enemyModel, new Vector3(x + 0.5f, y, 0), Quaternion.identity, gridParent.transform);
                enemies.Add(newEnemy);
                break;
            case 2:
                for (int i = -1; i <= 1; i += 2)
                {
                    y = i;
                    newEnemy = Instantiate(enemyModel, new Vector3(x + 0.5f, y, 0), Quaternion.identity, gridParent.transform);
                    friendlies.Add(newEnemy);
                }
                break;
            case 3:
                for (int i = -2; i <= 2; i += 2)
                {
                    y = i;
                    newEnemy = Instantiate(enemyModel, new Vector3(x + 0.5f, y, 0), Quaternion.identity, gridParent.transform);
                    friendlies.Add(newEnemy);
                }
                break;
            case 4:
            default:
                for (int i = -3; i <= 3; i += 2)
                {
                    y = i;
                    newEnemy = Instantiate(enemyModel, new Vector3(x + 0.5f, y, 0), Quaternion.identity, gridParent.transform);
                    friendlies.Add(newEnemy);
                }
                break;
        }
        GameManager.Instance.enemies = enemies;
    }
}
