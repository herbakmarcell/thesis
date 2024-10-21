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
        GameManager.Instance.playerCount = int.Parse(playerDropdown.options[playerCount].text);
        Debug.Log(GameManager.Instance.playerCount);
    }

    void OnEnemyDropdownChanged(int enemyCount)
    {
        GameManager.Instance.enemyCount = int.Parse(enemyDropdown.options[enemyCount].text);
        Debug.Log(GameManager.Instance.enemyCount);
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
