using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject lightTile;
    public GameObject darkTile;

    Grid grid;

    void Start()
    {
        grid = GetComponent<Grid>(); //10x7 grid

        for (int x = 4; x > -6; x--)
        {
            for (int y = -4; y < 3; y++)
            {
                Vector3 pos = grid.GetCellCenterWorld(new Vector3Int(x, y));
                if ((x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0)) Instantiate(lightTile, pos, Quaternion.identity, transform);
                else Instantiate(darkTile, pos, Quaternion.identity, transform);
            }
        }
    }

    void Update()
    {
        
    }
}
