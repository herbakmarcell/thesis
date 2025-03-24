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
    public TMP_Text turnText;

    private void Start()
    {
        playerDropdown.ClearOptions();
        enemyDropdown.ClearOptions();
        playerDropdown.AddOptions(new List<string> { "1", "2", "3", "4" });
        playerDropdown.onValueChanged.AddListener(OnPlayerDropdownChanged);
        enemyDropdown.AddOptions(new List<string> { "1", "2", "3", "4" });
        enemyDropdown.onValueChanged.AddListener(OnEnemyDropdownChanged);
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
        if (Options.playerStarts)
        {
            Options.playerStarts = false;
            turnText.text = "MI";
        }
        else
        {
            Options.playerStarts = true;
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
