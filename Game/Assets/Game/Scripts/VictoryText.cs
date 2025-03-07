using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryText : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Awake()
    {
        text.text = GameManager.Instance.currentStatus == Status.PLAYERWIN ? "Nyertél!" : "Vesztettél!";
    }
}
