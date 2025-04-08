using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonScripts : MonoBehaviour
{
    public TMP_Dropdown playerDropdown;
    public TMP_Dropdown enemyDropdown;
    public TMP_Dropdown obstacleDropdown;
    public TMP_Text turnText;

    private void Start()
    {
        playerDropdown.ClearOptions();
        enemyDropdown.ClearOptions();
        obstacleDropdown.ClearOptions();
        playerDropdown.AddOptions(new List<string> { "1", "2", "3", "4" });
        playerDropdown.onValueChanged.AddListener(OnPlayerDropdownChanged);
        enemyDropdown.AddOptions(new List<string> { "1", "2", "3", "4" });
        enemyDropdown.onValueChanged.AddListener(OnEnemyDropdownChanged);
        obstacleDropdown.AddOptions(new List<string> { "0", "1", "2", "3", "4", "5", "6" });
        obstacleDropdown.onValueChanged.AddListener(OnObstacleDropdownChanged);
    }

    void OnObstacleDropdownChanged(int obstacleCount)
    {
        Options.obstacleCount = int.Parse(obstacleDropdown.options[obstacleCount].text);
    }

    void OnPlayerDropdownChanged(int playerCount)
    {
        Options.friendlyCount = int.Parse(playerDropdown.options[playerCount].text);
    }

    void OnEnemyDropdownChanged(int enemyCount)
    {
        Options.enemyCount = int.Parse(enemyDropdown.options[enemyCount].text);
    }

    public void ChangeStartingTurn()
    {
        Options.playerStarts = !Options.playerStarts;
        if (!Options.playerStarts)
        {
            turnText.text = "MI";
        }
        else
        {
            turnText.text = "Játékos";
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
       Application.Quit();
    }
}
