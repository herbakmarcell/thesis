using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class AIHealthUI : MonoBehaviour
{
    public GameObject aiGroupPrefab;
    public List<GameObject> healthList = new List<GameObject>();

    void Awake()
    {
        GenerateHealthUI();
    }

    void Start()
    {
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        ClearHealthUI();
        GenerateHealthUI();
    }

    void GenerateHealthUI()
    {
        List<GameObject> enemies = GameManager.Instance.enemies;

        foreach (var enemy in enemies)
        {
            GameObject singleHealthBar = Instantiate(aiGroupPrefab, transform);
            healthList.Add(singleHealthBar);
            singleHealthBar.GetComponent<PlayerGroupManager>().SetBar(enemy.GetComponent<EntityStat>().health);
        }
    }

    void ClearHealthUI()
    {
        if (healthList.Count == 0)
        {
            return;
        }
        healthList.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}
