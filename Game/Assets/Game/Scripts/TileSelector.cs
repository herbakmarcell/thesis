using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    public Tilemap tilemap;
    public Color highlightColor;
    public Color validTileColor;

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

    void SelectTile()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        worldPos.z = 0;

        Vector3Int currentCell = tilemap.WorldToCell(worldPos);
        //if (gameManager.activePlayer >= gameManager.friendlies.Count) return;
        Vector3Int playerCell = tilemap.WorldToCell(GameManager.Instance.friendlies[GameManager.Instance.activePlayer].transform.position);
        if (GameManager.Instance.actionSelected && GameManager.Instance.moveTurn)
        {
            if (IsValidMove(currentCell, playerCell))
            {
                tilemap.SetTileFlags(currentCell, TileFlags.None);
                tilemap.SetColor(currentCell, highlightColor);

                if (previousCell != currentCell)
                {
                    ResetPreviousCell();
                    previousCell = currentCell;
                }

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    GameManager.Instance.friendlies[GameManager.Instance.activePlayer].transform.position = tilemap.GetCellCenterWorld(currentCell);
                    UnityEngine.Debug.Log("NextTurn");
                    GameManager.Instance.NextTurn();
                    playerActionDone = true;
                    ResetPreviousCell();
                }
            }
            else
            {
                ResetPreviousCell();
            }
        } 
        else if(GameManager.Instance.actionSelected && !GameManager.Instance.moveTurn)
        {
            if (IsValidAttack(currentCell, playerCell))
            {
                tilemap.SetTileFlags(currentCell, TileFlags.None);
                tilemap.SetColor(currentCell, highlightColor);

                if (previousCell != currentCell)
                {
                    ResetPreviousCell();
                    previousCell = currentCell;
                }

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    GameObject enemy = SelectEnemy(currentCell);
                    if (enemy != null)
                    {
                        enemy.gameObject.GetComponent<EntityStat>().TakeDamage(1);
                        GameManager.Instance.NextTurn();
                        playerActionDone = true;
                        ResetPreviousCell();
                    }
                }
            }
        }
    }

    private void ResetPreviousCell()
    {
        if (previousCell != new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue))
        {
            if (!IsObstacle(previousCell))
            {
                tilemap.SetColor(previousCell, validTileColor);
            }
        }
    }

    private bool IsValidMove(Vector3Int targetCell, Vector3Int playerCell)
    {
        float distance = Vector2.Distance(tilemap.GetCellCenterWorld(targetCell), tilemap.GetCellCenterWorld(playerCell));
        return distance == 1.0f && !IsObstacle(targetCell) && !IsFriendlyOccupied(targetCell) && !IsEnemyOccupied(targetCell);
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
