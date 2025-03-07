using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public GameObject playerGroupPrefab;
    public List<GameObject> healthList = new List<GameObject>();

    void Start()
    {
        GenerateHealthUI();
    }

    public void UpdateHealthUI()
    {
        ClearHealthUI();
        GenerateHealthUI();
    }

    void GenerateHealthUI()
    {
        List<GameObject> friendlies = GameManager.Instance.friendlies;

        foreach (var friendly in friendlies)
        {
            GameObject singleHealthBar = Instantiate(playerGroupPrefab, transform);
            healthList.Add(singleHealthBar);
            singleHealthBar.GetComponent<PlayerGroupManager>().SetBar(friendly.GetComponent<EntityStat>().health);
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
