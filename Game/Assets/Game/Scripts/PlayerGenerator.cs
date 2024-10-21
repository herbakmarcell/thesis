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
        int friendlyCount = GameManager.Instance.playerCount;
        for (int i = 0; i < friendlyCount; i++)
        {

            float setY = i - (friendlyCount - 1) / 2f;
            int y = setY < 0 ? Mathf.FloorToInt(setY) : Mathf.CeilToInt(setY);

            GameObject newFriendly = Instantiate(friendlyModel, new Vector3(x + 0.5f, y, 0), Quaternion.identity, gridParent.transform);
            friendlies.Add(newFriendly);

        }
        GameManager.Instance.friendlies = friendlies;
    }

    void GenerateEnemies()
    {
        int x = 5;
        int enemyCount = GameManager.Instance.enemyCount;
        for (int i = 0; i < enemyCount; i++)
        {

            float setY = i - (enemyCount - 1) / 2f;
            int y = setY < 0 ? Mathf.FloorToInt(setY) : Mathf.CeilToInt(setY);

            GameObject newEnemy = Instantiate(enemyModel, new Vector3(x + 0.5f, y, 0), Quaternion.identity, gridParent.transform);
            enemies.Add(newEnemy);
        }
        GameManager.Instance.enemies = enemies;
    }
}
