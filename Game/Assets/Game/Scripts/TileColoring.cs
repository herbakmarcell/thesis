using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileColoring : MonoBehaviour
{
    public Tilemap tilemap;

    public Color normalColor;
    public Color highlightColor;
    public Color validColor;
    public Color invalidColor;

    Camera camera;
    Vector3Int previousCell = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();

        camera = Camera.main;
        if (camera == null)
        {
            camera = FindFirstObjectByType<Camera>();
        }
    }

    void Update()
    {
        HighlightTile();
    }

    void HighlightTile()
    {
        if (GameManager.Instance.activePlayer >= GameManager.Instance.friendlies.Count) return;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camera.nearClipPlane));
        worldPos.z = 0;

        Vector3Int currentCell = tilemap.WorldToCell(worldPos);
        Vector3Int playerCell = tilemap.WorldToCell(GameManager.Instance.friendlies[GameManager.Instance.activePlayer].transform.position);
        tilemap.SetTileFlags(currentCell, TileFlags.None);
        switch (GameManager.Instance.actionSelected)
        {
            case ActionSelected.MOVE:
                if (IsValidMove(currentCell, playerCell)) tilemap.SetColor(currentCell, validColor);
                else tilemap.SetColor(currentCell, invalidColor);
                break;
            case ActionSelected.ATTACK:
                if (IsValidAttack(currentCell, playerCell)) tilemap.SetColor(currentCell, validColor);
                else tilemap.SetColor(currentCell, invalidColor);
                break;
            case ActionSelected.NONE:
            default:
                tilemap.SetColor(currentCell, highlightColor);
                break;
        }
        
        if (previousCell != currentCell)
        {
            tilemap.SetColor(previousCell, normalColor);
            previousCell = currentCell;
        }
    }

    private bool IsValidMove(Vector3Int targetCell, Vector3Int playerCell)
    {
        float distance = Vector2.Distance(tilemap.GetCellCenterWorld(targetCell), tilemap.GetCellCenterWorld(playerCell));
        return distance == 1.0f && !IsObstacle(targetCell) && !IsFriendlyOccupied(targetCell) && !IsEnemyOccupied(targetCell); ;
    }

    private bool IsValidAttack(Vector3Int targetCell, Vector3Int playerCell)
    {
        float distance = Vector2.Distance(tilemap.GetCellCenterWorld(targetCell), tilemap.GetCellCenterWorld(playerCell));
        return distance == 1.0f && IsEnemyOccupied(targetCell);
    }

    private bool IsObstacle(Vector3Int cell)
    {
        Collider2D collider = Physics2D.OverlapPoint(tilemap.GetCellCenterWorld(cell));
        return collider != null && collider.CompareTag("Obsticle");
    }

    private bool IsFriendlyOccupied(Vector3Int cell)
    {
        foreach (var friendly in GameManager.Instance.friendlies)
        {
            Vector3Int friendlyCell = tilemap.WorldToCell(friendly.transform.position);
            if (friendlyCell == cell)
            {
                return true;
            }
        }
        return false;
    }

    bool IsEnemyOccupied(Vector3Int cell)
    {
        foreach (var enemy in GameManager.Instance.enemies)
        {
            Vector3Int enemyCell = tilemap.WorldToCell(enemy.transform.position);
            if (enemyCell == cell)
            {
                return true;
            }
        }
        return false;
    }
}
