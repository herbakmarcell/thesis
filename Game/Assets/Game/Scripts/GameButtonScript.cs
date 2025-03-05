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
        GameManager.Instance.actionSelected = ActionSelected.MOVE;
        GameManager.Instance.moveTurn = true;
        DisableTurnButtons();
    }

    public void AttackTurnButtonPressed()
    {
        GameManager.Instance.actionSelected = ActionSelected.ATTACK;
        GameManager.Instance.moveTurn = false;
        DisableTurnButtons();
        EnableAttackButtons();
    }

    public void CancelButtonPressed()
    {
        GameManager.Instance.actionSelected = ActionSelected.NONE;
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
