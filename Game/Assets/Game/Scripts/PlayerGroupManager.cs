using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGroupManager : MonoBehaviour
{
    public Image playerIcon;
    public Image healthMask;

    public void SetBar(int health)
    {
        healthMask.fillAmount = health / 10f;
    }
}
