using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonScript : MonoBehaviour
{
    public Button attackTurnButton;
    public Button moveTurnButton;

    public Button cancelButton;

    void Start()
    {
        DisableAttackButtons();

        if (!GameManager.Instance.playerTurn)
        {
            DisableTurnButtons();
        }
    }

    void Update()
    {
        if (TileSelector.playerActionDone)
        {
            PlayerAction();
            TileSelector.playerActionDone = false;
        }
    }

    public void MoveTurnButtonPressed()
    {
        GameManager.Instance.actionSelected = true;
        GameManager.Instance.moveTurn = true;
        DisableTurnButtons();
    }

    public void AttackTurnButtonPressed()
    {
        GameManager.Instance.actionSelected = true;
        GameManager.Instance.moveTurn = false;
        DisableTurnButtons();
        EnableAttackButtons();
    }

    public void HeavyAttackButtonPressed()
    {
        GameManager.Instance.attackType = AttackType.HEAVY;
    }

    public void LightAttackButtonPressed()
    {
        GameManager.Instance.attackType = AttackType.LIGHT;
    }

    public void SpellAttackButtonPressed()
    {
        GameManager.Instance.attackType = AttackType.SPELL;
    }

    public void CancelButtonPressed()
    {
        GameManager.Instance.actionSelected = false;
        if (GameManager.Instance.moveTurn)
        {
            EnableTurnButtons();
            cancelButton.gameObject.SetActive(false);
        }
        else
        {
            DisableAttackButtons();
            EnableTurnButtons();
            cancelButton.gameObject.SetActive(false);
        }
    }

    void DisableTurnButtons()
    {
        attackTurnButton.gameObject.SetActive(false);
        moveTurnButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(true);
    }
    void EnableTurnButtons()
    {
        attackTurnButton.gameObject.SetActive(true);
        moveTurnButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(false);
    }

    void EnableAttackButtons()
    {
        cancelButton.gameObject.SetActive(true);
    }

    void DisableAttackButtons()
    {
        cancelButton.gameObject.SetActive(false);
    }

    public void PlayerAction()
    {
        if (GameManager.Instance.playerTurn)
        {
            EnableTurnButtons();
        }
        DisableAttackButtons();
        cancelButton.gameObject.SetActive(false);
    }
}
