using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject friendlyModel;

    public GameObject enemyModel;

    public GameObject obstacleModel;
    public GameObject obstacleParent;

    void Start()
    {
        GenerateFriendlyGameObjects();
        GenerateEnemyGameObjects();
        GenerateObstacles();
    }

    void GenerateFriendlyGameObjects()
    {
        List<PlayerObject> friendlyObjects = GameManager.Instance.stateRepresentation.ListPlayerObjects(false);
        for (int i = 0; i < friendlyObjects.Count; i++)
        {
            GameObject newFriendly = Instantiate(friendlyModel, new Vector3(friendlyObjects[i].position.x - 4.5f, friendlyObjects[i].position.y - 4f, 0), Quaternion.identity, gameObject.transform);
            GameManager.Instance.friendlies.Add(newFriendly);
        }
    }

    void GenerateEnemyGameObjects()
    {
        List<PlayerObject> enemyObjects = GameManager.Instance.stateRepresentation.ListPlayerObjects(true);
        for (int i = 0; i < enemyObjects.Count; i++)
        {
            GameObject newEnemy = Instantiate(enemyModel, new Vector3(enemyObjects[i].position.x - 4.5f, enemyObjects[i].position.y -4f, 0), Quaternion.identity, gameObject.transform);
            GameManager.Instance.enemies.Add(newEnemy);
        }
    }

    void GenerateObstacles()
    {
        List<FieldObject> obstacles = GameManager.Instance.stateRepresentation.ListObstacles();
        for (int i = 0; i < obstacles.Count; i++)
        {
            GameObject newObstacle = Instantiate(obstacleModel, new Vector3(obstacles[i].position.x - 4.5f, obstacles[i].position.y - 3f, 0), Quaternion.identity, obstacleParent.transform);
            GameManager.Instance.obstacles.Add(newObstacle);
        }
    }
}
