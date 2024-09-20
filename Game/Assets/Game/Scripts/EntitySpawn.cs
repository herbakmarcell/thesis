using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawn : MonoBehaviour
{
    Grid grid;
    public GameObject playerPrefab;

    void Start()
    {
        grid = GetComponent<Grid>();

        Vector3 playerPos = grid.GetCellCenterWorld(new Vector3Int(-3,-1));
        Instantiate(playerPrefab, playerPos, Quaternion.identity, transform);
    }

    void Update()
    {
        
    }
}
