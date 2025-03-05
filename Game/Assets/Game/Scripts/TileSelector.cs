using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    public Tilemap tilemap;

    private Vector3Int previousCell = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
    private Camera cam;

    public static bool playerActionDone = false;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null)
        {
            cam = FindFirstObjectByType<Camera>();
        }
    }

    void Update()
    {
        if (GameManager.Instance.playerTurn) SelectTile();        
    }

    public void SelectTile()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (GameManager.Instance.actionSelected == ActionSelected.MOVE)
            {
                Vector2 mousePos = Mouse.current.position.ReadValue();
                Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
                worldPos.z = 0;

                Vector3Int currentCell = tilemap.WorldToCell(worldPos);
                Vector3Int playerCell = tilemap.WorldToCell(GameManager.Instance.friendlies[GameManager.Instance.activePlayer].transform.position);

                if (IsValidMove(currentCell, playerCell))
                {
                    GameManager.Instance.actionDirection = GetDirection(currentCell, playerCell);
                    GameManager.Instance.friendlies[GameManager.Instance.activePlayer].GetComponent<EntityStat>().position = SetNewObjectPosition(GameManager.Instance.actionDirection);
                    GameManager.Instance.friendlies[GameManager.Instance.activePlayer].transform.position = tilemap.GetCellCenterWorld(currentCell);
                    GameManager.Instance.ProgressGame();
                }
            }
        }
    }

    private bool IsValidMove(Vector3Int targetCell, Vector3Int playerCell)
    {
        float distance = Vector2.Distance(tilemap.GetCellCenterWorld(targetCell), tilemap.GetCellCenterWorld(playerCell));
        return distance == 1.0f && !IsObstacle(targetCell) && !IsFriendlyOccupied(targetCell) && !IsEnemyOccupied(targetCell);
    }

    private ActionDirection GetDirection(Vector3Int targetCell, Vector3Int playerCell)
    {
        if (targetCell.x > playerCell.x) return ActionDirection.RIGHT;
        if (targetCell.x < playerCell.x) return ActionDirection.LEFT;
        if (targetCell.y > playerCell.y) return ActionDirection.UP;
        if (targetCell.y < playerCell.y) return ActionDirection.DOWN;
        throw new System.Exception("Invalid direction");
    }

    private Vector2 SetNewObjectPosition(ActionDirection actionDirection)
    {
        Vector2 position = GameManager.Instance.friendlies[GameManager.Instance.activePlayer].GetComponent<EntityStat>().position;
        switch (actionDirection)
        {
            case ActionDirection.UP:
                position.y += 1;
                break;
            case ActionDirection.DOWN:
                position.y -= 1;
                break;
            case ActionDirection.LEFT:
                position.x -= 1;
                break;
            case ActionDirection.RIGHT:
                position.x += 1;
                break;
        }
        return position;
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

    GameObject SelectEnemy(Vector3Int cell)
    {
        return GameManager.Instance.enemies.FirstOrDefault(enemy => tilemap.WorldToCell(enemy.transform.position) == cell);
    }

    bool IsValidAttack(Vector3Int targetCell, Vector3Int playerCell)
    {
        float distance = Vector2.Distance(tilemap.GetCellCenterWorld(targetCell), tilemap.GetCellCenterWorld(playerCell));
        return distance == 1.0f && !IsObstacle(targetCell) && IsEnemyOccupied(targetCell);
    }
}
