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
        StateRepresentation stateRepresentation = GameManager.Instance.stateRepresentation as StateRepresentation;
        List<PlayerObject> friendlyObjects = stateRepresentation.ListPlayerObjects(false);
        for (int i = 0; i < friendlyObjects.Count; i++)
        {
            GameObject newFriendly = Instantiate(friendlyModel, new Vector3(friendlyObjects[i].position.x - 4.5f, friendlyObjects[i].position.y - 3f, 0), Quaternion.identity, gameObject.transform);
            newFriendly.GetComponent<EntityStat>().id = friendlyObjects[i].id;
            newFriendly.GetComponent<EntityStat>().position = friendlyObjects[i].position;
            newFriendly.GetComponent<EntityStat>().health = friendlyObjects[i].health;
            newFriendly.GetComponent<EntityStat>().attack = friendlyObjects[i].attack;
            newFriendly.GetComponent<EntityStat>().isAI = friendlyObjects[i].isAI;
            GameManager.Instance.friendlies.Add(newFriendly);
        }
    }

    void GenerateEnemyGameObjects()
    {
        StateRepresentation stateRepresentation = GameManager.Instance.stateRepresentation as StateRepresentation;
        List<PlayerObject> enemyObjects = stateRepresentation.ListPlayerObjects(true);
        for (int i = 0; i < enemyObjects.Count; i++)
        {
            GameObject newEnemy = Instantiate(enemyModel, new Vector3(enemyObjects[i].position.x - 4.5f, enemyObjects[i].position.y -3f, 0), Quaternion.identity, gameObject.transform);
            newEnemy.GetComponent<EntityStat>().id = enemyObjects[i].id;
            newEnemy.GetComponent<EntityStat>().position = enemyObjects[i].position;
            newEnemy.GetComponent<EntityStat>().health = enemyObjects[i].health;
            newEnemy.GetComponent<EntityStat>().attack = enemyObjects[i].attack;
            newEnemy.GetComponent<EntityStat>().isAI = enemyObjects[i].isAI;
            GameManager.Instance.enemies.Add(newEnemy);
        }
    }

    void GenerateObstacles()
    {
        StateRepresentation stateRepresentation = GameManager.Instance.stateRepresentation as StateRepresentation;
        List<FieldObject> obstacles = stateRepresentation.ListObstacles();
        for (int i = 0; i < obstacles.Count; i++)
        {
            GameObject newObstacle = Instantiate(obstacleModel, new Vector3(obstacles[i].position.x - 4.5f, obstacles[i].position.y - 3f, 0), Quaternion.identity, obstacleParent.transform);
            GameManager.Instance.obstacles.Add(newObstacle);
        }
    }
}
