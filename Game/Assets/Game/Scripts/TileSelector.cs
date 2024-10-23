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

    private GameManager gameManager;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null)
        {
            cam = FindFirstObjectByType<Camera>();
        }
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (gameManager.playerTurn) SelectTile();        
    }

    void SelectTile()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        worldPos.z = 0;

        Vector3Int currentCell = tilemap.WorldToCell(worldPos);
        Vector3Int playerCell = tilemap.WorldToCell(gameManager.friendlies[gameManager.activePlayer].transform.position);
        if (gameManager.actionSelected && gameManager.moveTurn)
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
                    gameManager.friendlies[gameManager.activePlayer].transform.position = tilemap.GetCellCenterWorld(currentCell);
                    gameManager.NextTurn();
                    ResetPreviousCell();
                }
            }
            else
            {
                ResetPreviousCell();
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
        return distance == 1.0f && !IsObstacle(targetCell) && !IsFriendlyOccupied(targetCell);
    }

    private bool IsObstacle(Vector3Int cell)
    {
        Collider2D collider = Physics2D.OverlapPoint(tilemap.GetCellCenterWorld(cell));
        return collider != null && collider.CompareTag("Obsticle");
    }

    private bool IsFriendlyOccupied(Vector3Int cell)
    {
        foreach (var friendly in gameManager.friendlies)
        {
            Vector3Int friendlyCell = tilemap.WorldToCell(friendly.transform.position);
            if (friendlyCell == cell)
            {
                return true;
            }
        }
        return false;
    }
}
