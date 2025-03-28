public void SelectTile()
{
    if (Mouse.current.leftButton.wasPressedThisFrame)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        worldPos.z = 0;

        Vector3Int currentCell = tilemap.WorldToCell(worldPos);
        Vector3Int playerCell = tilemap.WorldToCell(GameManager.Instance.friendlies[GameManager.Instance.activePlayer].transform.position);
        if (GameManager.Instance.actionSelected == ActionSelected.MOVE)
        {
            if (IsValidMove(currentCell, playerCell))
            {
                GameManager.Instance.actionDirection = GetDirection(currentCell, playerCell);
                GameManager.Instance.friendlies[GameManager.Instance.activePlayer].GetComponent<EntityStat>().position = SetNewObjectPosition(GameManager.Instance.actionDirection);
                GameManager.Instance.friendlies[GameManager.Instance.activePlayer].transform.position = tilemap.GetCellCenterWorld(currentCell);
                GameManager.Instance.ProgressGame();
                GameObject.Find("Canvas").GetComponent<GameButtonScript>().EnableTurnButtons();
            }
        }
        else if (GameManager.Instance.actionSelected == ActionSelected.ATTACK)
        {
            if (IsValidAttack(currentCell, playerCell))
            {
                GameObject enemy = SelectEnemy(currentCell);
                GameManager.Instance.actionDirection = GetDirection(currentCell, playerCell);
                enemy.GetComponent<EntityStat>().health -= GameManager.Instance.friendlies[GameManager.Instance.activePlayer].GetComponent<EntityStat>().attack;
                if (enemy.GetComponent<EntityStat>().health <= 0)
                {
                    GameManager.Instance.enemies.Remove(enemy);
                    Destroy(enemy);
                }
                GameManager.Instance.ProgressGame();
                GameObject.Find("Canvas").GetComponent<GameButtonScript>().EnableTurnButtons();
            }
        }
    }
}