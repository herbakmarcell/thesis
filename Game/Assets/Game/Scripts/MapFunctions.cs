using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFunctions : MonoBehaviour
{
    public void EntityMove(string id, bool isAI, Vector2 newPostion)
    {
        if (isAI)
        {
            GameManager.Instance.enemies.Find(x => x.GetComponent<EntityStat>().id == id).transform.position = newPostion;
        } 
        else
        {
            GameManager.Instance.friendlies.Find(x => x.GetComponent<EntityStat>().id == id).transform.position = newPostion;
        }
    }
}
