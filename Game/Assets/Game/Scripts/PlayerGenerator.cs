using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    public GameObject friendlyModel;
    public List<GameObject> friendlies;

    public GameObject enemyModel;
    public List<GameObject> enemies;

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

    void GenerateFriendlies()
    {
        int x = -4;
        int friendlyCount = GameManager.Instance.playerCount;
        for (int i = 0; i < friendlyCount; i++)
        {
            float y = (i * 2) - (friendlyCount - 1);

            GameObject newFriendly = Instantiate(friendlyModel, new Vector3(x + 0.5f, y, 0), Quaternion.identity, gameObject.transform);
            friendlies.Add(newFriendly);
        }
        GameManager.Instance.friendlies = friendlies;
    }

    void GenerateEnemies()
    {
        int x = 3;
        int enemyCount = GameManager.Instance.enemyCount;
        for (int i = 0; i < enemyCount; i++)
        {
            float y = (i * 2) - (enemyCount - 1);

            GameObject newEnemy = Instantiate(enemyModel, new Vector3(x + 0.5f, y, 0), Quaternion.identity, gameObject.transform);
            enemies.Add(newEnemy);
        }
        GameManager.Instance.enemies = enemies;
    }
}
