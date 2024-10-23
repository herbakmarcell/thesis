using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public GameObject playerGroupPrefab;
    public List<GameObject> healthList = new List<GameObject>();

    void Start()
    {
        GenerateHealthUI();
    }

    void Update()
    {
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < healthList.Count; i++)
        {
            healthList[i].GetComponent<PlayerGroupManager>().SetBar(GameManager.Instance.friendlies[i].GetComponent<EntityStat>().health);
        }
    }

    void GenerateHealthUI()
    {
        ClearHealthUI();

        for (int i = 0; i < GameManager.Instance.friendlies.Count; i++)
        {
            GameObject singleHealthBar = Instantiate(playerGroupPrefab, transform);
            healthList.Add(singleHealthBar);
            singleHealthBar.GetComponent<PlayerGroupManager>().SetBar(GameManager.Instance.friendlies[i].GetComponent<EntityStat>().health);
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
